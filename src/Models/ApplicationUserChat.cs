using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ApplicationUserChat
{
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public string ApplicationUserChatStatus { get; set; }

    public int ChatId { get; set; }
    public Chat Chat { get; set; }
}