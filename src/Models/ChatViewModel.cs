using System.Collections.Generic;

public class ChatViewModel
{
    public IList<ApplicationUserChat> ApplicationUserChats { get; set; }
    public IList<Chat> Chats { get; set; }
    public IList<SelfHelpGroup> SelfHelpGroups { get; set; }
}