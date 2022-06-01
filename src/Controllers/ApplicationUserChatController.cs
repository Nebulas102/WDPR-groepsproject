using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace zmdh.Controllers
{
    public class ApplicationUserChatController : Controller
    {
        private readonly DBManager _context;

        public ApplicationUserChatController(DBManager context)
        {
            _context = context;
        }

        // GET: ApplicationUserChat
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Index()
        {
            var dBManager = _context.ApplicationUserChats.Include(a => a.ApplicationUser).Include(a => a.Chat);
            return View(await dBManager.ToListAsync());
        }

        // GET: ApplicationUserChat/Details/userid+chatid
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Details(string userId, int chatId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var applicationUserChat = await _context.ApplicationUserChats
                .Include(a => a.ApplicationUser)
                .Include(a => a.Chat)
                .FirstOrDefaultAsync(m => m.ApplicationUserId == userId && m.ChatId == chatId);
            if (applicationUserChat == null)
            {
                return NotFound();
            }

            return View(applicationUserChat);
        }

        // GET: ApplicationUserChat/Create
        [Authorize(Roles = "Admin,Hulpverlener")]
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName");
            ViewData["ChatId"] = new SelectList(_context.Chats, "Id", "Name");
            return View();
        }

        // POST: ApplicationUserChat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Create([Bind("ApplicationUserId,ApplicationUserChatStatus,ChatId")] ApplicationUserChat applicationUserChat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationUserChat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", applicationUserChat.ApplicationUserId);
            ViewData["ChatId"] = new SelectList(_context.Chats, "Id", "Name", applicationUserChat.ChatId);
            return View(applicationUserChat);
        }

        // GET: ApplicationUserChat/Edit/userid+chatid
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Edit(string userId, int chatId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var applicationUserChat = await _context.ApplicationUserChats.Where(auc => auc.ApplicationUserId == userId
            && auc.ChatId == chatId).SingleOrDefaultAsync();
            if (applicationUserChat == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", applicationUserChat.ApplicationUserId);
            ViewData["ChatId"] = new SelectList(_context.Chats, "Id", "Name", applicationUserChat.ChatId);
            return View(applicationUserChat);
        }

        // POST: ApplicationUserChat/Edit/userid+chatid
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Edit(string userId, int chatId, [Bind("ApplicationUserId,ApplicationUserChatStatus,ChatId")] ApplicationUserChat applicationUserChat)
        {
            if (userId != applicationUserChat.ApplicationUserId || chatId != applicationUserChat.ChatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUserChat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserChatExists(applicationUserChat.ApplicationUserId, applicationUserChat.ChatId))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", applicationUserChat.ApplicationUserId);
            ViewData["ChatId"] = new SelectList(_context.Chats, "Id", "Name", applicationUserChat.ChatId);
            return View(applicationUserChat);
        }

        // GET: ApplicationUserChat/Delete/userid+chatid
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> Delete(string userId, int chatId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var applicationUserChat = await _context.ApplicationUserChats
                .Include(a => a.ApplicationUser)
                .Include(a => a.Chat)
                .FirstOrDefaultAsync(m => m.ApplicationUserId == userId && m.ChatId == chatId);
            if (applicationUserChat == null)
            {
                return NotFound();
            }

            return View(applicationUserChat);
        }

        // POST: ApplicationUserChat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Hulpverlener")]
        public async Task<IActionResult> DeleteConfirmed(string userId, int chatId)
        {
            var applicationUserChat = await _context.ApplicationUserChats.Where(auc => auc.ApplicationUserId == userId
           && auc.ChatId == chatId).SingleOrDefaultAsync();
            _context.ApplicationUserChats.Remove(applicationUserChat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserChatExists(string userId, int chatId)
        {
            return _context.ApplicationUserChats.Any(e => e.ApplicationUserId == userId && e.ChatId == chatId);
        }
    }
}
