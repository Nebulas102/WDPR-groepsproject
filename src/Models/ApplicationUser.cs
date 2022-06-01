using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public virtual IList<ApplicationUserChat> ApplicationUserChats { get; set; }
    public IList<Report> HandledReports { get; set; }
    public int? ClientId { get; set; }
    public Client Client { get; set; }
    public int? HulpverlenerId { get; set; }
    public Hulpverlener Hulpverlener { get; set; }
    public int? OuderId { get; set; }
    public Ouder Ouder { get; set; }
}