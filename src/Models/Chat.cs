using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;

public class Chat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; }

    public SelfHelpGroup SelfHelpGroup { get; set; }

    public virtual IList<ApplicationUserChat> ApplicationUserChats { get; set; }


    public int? ChatFrequencyId { get; set; }
    public virtual ChatFrequency ChatFrequency { get; set; }
}