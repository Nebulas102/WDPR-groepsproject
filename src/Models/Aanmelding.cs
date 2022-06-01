using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class Aanmelding
{
    [Key]
    public int Id { get; set; }

    [Required]
    public bool Agree { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    public int? OuderId { get; set; }
    public Ouder Ouder { get; set; }

    public int HulpverlenerId { get; set; }
    public Hulpverlener Hulpverlener { get; set; }
}