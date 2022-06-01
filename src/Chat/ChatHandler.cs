using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class Chathandler : Hub
{
    private readonly DBManager _context;
    private UserManager<ApplicationUser> _userManager;

    public Chathandler(DBManager context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public void AddToChat(string chatname)
    {
        var chat = _context.Chats.Include(c => c.ApplicationUserChats).Where(c => c.Name == chatname).FirstOrDefault();

        if (chat != null)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, chat.Name);
        }
    }

    public void RemoveFromChat(string chatname)
    {
        var chat = _context.Chats.Include(c => c.ApplicationUserChats).Where(c => c.Name == chatname).FirstOrDefault();

        if (chat != null)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, chat.Name);
        }
    }

    // [Authorize(Roles = "Admin,Hulpverlener,Client,Moderator")]
    public async Task SendMessage(string user, string message, string chat)
    {
        await Clients.Group(chat).SendAsync("newMessage", user, message).ConfigureAwait(true);
    }
}