using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace zmdh.Controllers
{
    public class ChatController : Controller
    {
        private readonly DBManager _context;
        private UserManager<ApplicationUser> _userManager;

        public ChatController(DBManager context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Chat
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chats.ToListAsync());
        }

        [Authorize(Roles = "Admin,Hulpverlener,Client")]
        [HttpGet]
        public async Task<IActionResult> SelfHelpGroups(string CurrentNameFilterString, bool CurrentLeeftijdsgroepString)
        {
            var SelfHelpGroupViewModel = new SelfHelpGroupViewModel();
            var CurrentUser = await _userManager.GetUserAsync(HttpContext.User);
            var SelfHelpGroups = await _context.SelfHelpGroups.Include(c => c.Chat).ThenInclude(c => c.ApplicationUserChats).ToListAsync();
            var ApplicationUserChats = await _context.ApplicationUserChats
            .Where(auc => auc.ApplicationUserId == CurrentUser.Id).ToListAsync();

            ViewData["CurrentUser"] = CurrentUser;
            ViewData["CurrentNameFilter"] = CurrentNameFilterString;
            ViewData["CurrentLeeftijdsgroepString"] = CurrentLeeftijdsgroepString;

            if (!String.IsNullOrEmpty(CurrentNameFilterString))
            {
                SelfHelpGroups = await _context.SelfHelpGroups.Where(s => s.Name.Contains(CurrentNameFilterString) && s.AboveSixteen == (bool)CurrentLeeftijdsgroepString)
                .Include(s => s.Chat).ThenInclude(c => c.ApplicationUserChats).AsNoTracking().ToListAsync();
            }
            else
            {
                SelfHelpGroups = await _context.SelfHelpGroups.Where(s => s.AboveSixteen == (bool)CurrentLeeftijdsgroepString)
                .Include(s => s.Chat).ThenInclude(c => c.ApplicationUserChats).AsNoTracking().ToListAsync();
            }

            SelfHelpGroupViewModel.SelfHelpGroups = SelfHelpGroups;
            SelfHelpGroupViewModel.ApplicationUserChats = ApplicationUserChats;

            return View(SelfHelpGroupViewModel);
        }

        [Authorize(Roles = "Admin,Hulpverlener,Client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelfHelpGroups(int chatId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (ModelState.IsValid)
            {
                ApplicationUserChat applicationUserChat = new ApplicationUserChat();

                applicationUserChat.ChatId = chatId;
                applicationUserChat.ApplicationUserId = currentUser.Id;
                applicationUserChat.ApplicationUserChatStatus = "pending";

                _context.ApplicationUserChats.Add(applicationUserChat);
                await _context.SaveChangesAsync();
                return RedirectToAction("SelfHelpGroups", "Chat");
            }
            return View();
        }

        [Authorize(Roles = "Admin,Hulpverlener")]
        [HttpGet]
        public async Task<IActionResult> SelfHelpGroupsDetermine()
        {
            var applicationUserChats = await _context.ApplicationUserChats.Where(auc => auc.ApplicationUserChatStatus == "pending")
            .Include(auc => auc.Chat).ThenInclude(c => c.SelfHelpGroup).ToListAsync();

            return View(applicationUserChats);
        }

        [Authorize(Roles = "Admin,Hulpverlener")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelfHelpGroupAccept(int selfHelpGroupId)
        {
            // var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var applicationUserChat = await _context.ApplicationUserChats.Include(auc => auc.Chat)
            .ThenInclude(c => c.SelfHelpGroup).Where(auc => auc.Chat.SelfHelpGroup.Id == selfHelpGroupId).SingleOrDefaultAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    applicationUserChat.ApplicationUserChatStatus = "active";
                    _context.Update(applicationUserChat);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("SelfHelpGroupsDetermine", "Chat");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View();
        }

        [Authorize(Roles = "Admin,Hulpverlener")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelfHelpGroupDecline(int selfHelpGroupId)
        {
            // var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var applicationUserChat = await _context.ApplicationUserChats.Include(auc => auc.Chat)
            .ThenInclude(c => c.SelfHelpGroup).Where(auc => auc.Chat.SelfHelpGroup.Id == selfHelpGroupId).SingleOrDefaultAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    applicationUserChat.ApplicationUserChatStatus = "declined";
                    _context.Update(applicationUserChat);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("SelfHelpGroupsDetermine", "Chat");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View();
        }


        [Authorize(Roles = "Admin,Hulpverlener,Client")]
        public async Task<IActionResult> ChatRoom()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewData["currentUser"] = currentUser;

            return View();
        }

        [Authorize(Roles = "Admin,Hulpverlener,Moderator")]
        [HttpGet]
        public async Task<IActionResult> Blockusers()
        {
            var applicationUserChat = await _context.ApplicationUserChats.Where(auc => auc.ApplicationUserChatStatus == "active"
            || auc.ApplicationUserChatStatus == "blocked").Include(auc => auc.Chat).ThenInclude(c => c.SelfHelpGroup)
            .Include(auc => auc.ApplicationUser).ToListAsync();
            return View(applicationUserChat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Hulpverlener,Moderator")]
        public async Task<IActionResult> BlockUser(string userId, int chatId)
        {
            var applicationUserChat = await _context.ApplicationUserChats
            .Include(auc => auc.Chat).ThenInclude(c => c.SelfHelpGroup)
            .Include(auc => auc.ApplicationUser)
            .Include(auc => auc.ApplicationUser).ThenInclude(au => au.Client)
            .Include(auc => auc.ApplicationUser).ThenInclude(au => au.Client).ThenInclude(c => c.Hulpverlener)
            .Where(auc => auc.ChatId == chatId && auc.ApplicationUserId == userId).SingleOrDefaultAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    applicationUserChat.ApplicationUserChatStatus = "blocked";
                    _context.Update(applicationUserChat);

                    if (applicationUserChat.ApplicationUser.Client != null)
                    {
                        Melding melding = new Melding();
                        melding.Content = "Clïent " + applicationUserChat.ApplicationUser.UserName + " is geblokkeert van chat/zelfhulpgroep " + applicationUserChat.Chat.Name;
                        melding.Hulpverlener = applicationUserChat.ApplicationUser.Client.Hulpverlener;
                        _context.Meldingen.Add(melding);

                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Blockusers", "Chat");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Hulpverlener,Moderator")]
        public async Task<IActionResult> UnBlockUser(string userId, int chatId)
        {
            var applicationUserChat = await _context.ApplicationUserChats
            .Include(auc => auc.Chat).ThenInclude(c => c.SelfHelpGroup)
            .Include(auc => auc.ApplicationUser)
            .Include(auc => auc.ApplicationUser).ThenInclude(au => au.Client)
            .Include(auc => auc.ApplicationUser).ThenInclude(au => au.Client).ThenInclude(c => c.Hulpverlener)
            .Where(auc => auc.ChatId == chatId && auc.ApplicationUserId == userId).SingleOrDefaultAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    applicationUserChat.ApplicationUserChatStatus = "active";
                    _context.Update(applicationUserChat);

                    if (applicationUserChat.ApplicationUser.Client != null)
                    {
                        Melding melding = new Melding();
                        melding.Content = "Clïent " + applicationUserChat.ApplicationUser.UserName + " is deblokkeert van chat/zelfhulpgroep " + applicationUserChat.Chat.Name;
                        melding.Hulpverlener = applicationUserChat.ApplicationUser.Client.Hulpverlener;
                        _context.Meldingen.Add(melding);

                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Blockusers", "Chat");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View();
        }


        // GET: Chat/Details/5
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // GET: Chat/Create
        [Authorize(Roles = "Admin,Hulpverlener")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Create([Bind("ID,Name")] Chat chat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

        // GET: Chat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return View(chat);
        }

        // POST: Chat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatExists(chat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chat);
        }

        // GET: Chat/Delete/5
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // POST: Chat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatExists(int id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }
    }
}
