using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace zmdh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatHandlerController : ControllerBase
    {
        private readonly DBManager _context;
        private UserManager<ApplicationUser> _userManager;

        public ChatHandlerController(DBManager context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/ChatHandler/GetReport/{id}
        [HttpGet("GetReport/{id}")]
        [Authorize(Roles = "Admin,Client,Hulpverlener")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            return report;
        }

        //POST: api/ChatHandler/PostReport
        [HttpPost("PostReport")]
        [Authorize(Roles = "Admin,Client,Hulpverlener")]
        public async Task<ActionResult<Report>> PostReport(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReport", new { id = report.Id }, report);
        }

        //GET: api/ChatHandler/
        [Authorize(Roles = "Admin,Client,Hulpverlener")]
        [HttpGet("getChatHandlerViewModel")]
        public async Task<ActionResult<ChatHandlerViewModel>> GetChatHandlerViewModel(int chatId)
        {
            ChatHandlerViewModel chatHandlerViewModel = new ChatHandlerViewModel();

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var applicationUserChat = await _context.ApplicationUserChats
                .Include(c => c.Chat).ThenInclude(s => s.SelfHelpGroup)
                .Where(ac => ac.ChatId == chatId && ac.ApplicationUserId == currentUser.Id && ac.ApplicationUserChatStatus == "active")
                .FirstOrDefaultAsync();

            var currentChat = applicationUserChat.Chat;
            var currentSelfHelpGroup = applicationUserChat.Chat.SelfHelpGroup;

            chatHandlerViewModel.CurrentApplicationUserChat = applicationUserChat;
            chatHandlerViewModel.CurrentSelfHelpGroup = currentSelfHelpGroup;
            chatHandlerViewModel.CurrentChat = currentChat;

            return chatHandlerViewModel;
        }

        //GET: api/ChatHandler/
        [Authorize(Roles = "Admin,Client,Hulpverlener")]
        [HttpGet("getChatViewModel")]
        public async Task<ActionResult<ChatViewModel>> GetChatViewModel()
        {
            ChatViewModel chatViewModel = new ChatViewModel();

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var applicationUserChats = await _context.ApplicationUserChats
                .Include(c => c.Chat).ThenInclude(s => s.SelfHelpGroup)
                .Where(ac => ac.ApplicationUserId == currentUser.Id && ac.ApplicationUserChatStatus == "active").ToListAsync();


            var chats = applicationUserChats.Select(ac => ac.Chat).ToList();
            var selfHelpGroups = applicationUserChats.Select(ac => ac.Chat.SelfHelpGroup).ToList();

            var allUserInChat = applicationUserChats.Select(ac => ac.ApplicationUser).ToList();

            chatViewModel.ApplicationUserChats = applicationUserChats;
            chatViewModel.SelfHelpGroups = selfHelpGroups;
            chatViewModel.Chats = chats;

            return chatViewModel;
        }
    }
}