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
    public class ChatFrequencyController : Controller
    {
        private readonly DBManager _context;
        private UserManager<ApplicationUser> _userManager;

        public ChatFrequencyController(DBManager context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin,Ouder,Hulpverlener")]
        public async Task<IActionResult> Frequency(int id)
        {
            FrequencyViewModel frequencyViewModel = new FrequencyViewModel();

            var chatFrequency = await _context.ChatFrequencies.Where(cf => cf.Id == id && cf.Status == "accepted")
            .Include(cf => cf.Chat)
            .Include(cf => cf.Hulpverlener)
            .Include(cf => cf.Ouder).ThenInclude(o => o.Account)
            .Include(cf => cf.Ouder).ThenInclude(o => o.Client).ThenInclude(c => c.Account)
            .SingleOrDefaultAsync();

            var applicationUserChat = await _context.ApplicationUserChats
            .Where(auc => auc.ApplicationUserId == chatFrequency.Ouder.Client.Account.Id &&
            auc.ChatId == chatFrequency.Chat.Id).SingleOrDefaultAsync();

            var messages = await _context.Messages.Where(m => m.AuthorId == applicationUserChat.ApplicationUserId
            && m.ChatId == applicationUserChat.ChatId).ToListAsync();

            frequencyViewModel.ChatFrequency = chatFrequency;
            frequencyViewModel.ApplicationUserChat = applicationUserChat;
            frequencyViewModel.Messages = messages;

            return View(frequencyViewModel);
        }

        [Authorize(Roles = "Ouder")]
        public async Task<IActionResult> RequestFrequency()
        {
            var ouder = await _userManager.GetUserAsync(HttpContext.User);
            var client = await _context.Ouders.Where(o => o.Account.Id == ouder.Id)
            .Include(o => o.Client).ThenInclude(c => c.Account).Select(o => o.Client).SingleOrDefaultAsync();

            var clientChats = await _context.ApplicationUserChats.Where(auc => auc.ApplicationUserId == client.Account.Id)
            .Include(auc => auc.Chat).ThenInclude(c => c.ChatFrequency)
            .Include(auc => auc.Chat).ThenInclude(c => c.SelfHelpGroup)
            .Include(auc => auc.Chat)
            .Include(auc => auc.ApplicationUser)
            .ToListAsync();

            ViewData["ouder"] = ouder;
            ViewData["client"] = client;

            return View(clientChats);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Ouder")]
        public async Task<IActionResult> RequestFrequency(int clientId, string ouderId, int chatId, [Bind("Id")] ChatFrequency chatFrequency)
        {
            var hulpverlener = await _context.Clienten.Where(c => c.ClientId == clientId)
            .Include(c => c.Hulpverlener).ThenInclude(h => h.Account).SingleOrDefaultAsync();
            var ouder = await _context.Ouders.Where(o => o.Account.Id == ouderId).SingleOrDefaultAsync();
            var chat = await _context.Chats.Where(c => c.Id == chatId).SingleOrDefaultAsync();

            if (ModelState.IsValid)
            {
                chatFrequency.Hulpverlener = hulpverlener.Hulpverlener;
                chatFrequency.Ouder = ouder;
                chatFrequency.Chat = chat;
                chatFrequency.Status = "pending";

                _context.Add(chatFrequency);
                await _context.SaveChangesAsync();
                return RedirectToAction("RequestFrequency", "chatFrequency");
            }
            return View(chatFrequency);
        }

        [Authorize(Roles = "Hulpverlener")]
        public async Task<IActionResult> FrequencyRequests()
        {
            var chatFrequencyRequests = await _context.ChatFrequencies.Include(cf => cf.Chat).ToListAsync();
            return View(chatFrequencyRequests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Hulpverlener")]
        public async Task<IActionResult> DeclineFrequencyRequest(int id)
        {
            var chatFrequency = await _context.ChatFrequencies.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    chatFrequency.Status = "declined";
                    _context.Update(chatFrequency);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("RequestFrequency", "chatFrequency");
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
        [Authorize(Roles = "Hulpverlener")]
        public async Task<IActionResult> AcceptFrequencyRequest(int id)
        {
            var chatFrequency = await _context.ChatFrequencies.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    chatFrequency.Status = "accepted";
                    _context.Update(chatFrequency);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("RequestFrequency", "chatFrequency");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return View();
        }
    }
}