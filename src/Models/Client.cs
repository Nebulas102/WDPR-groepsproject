using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class Client
{
    [Key]  
    public int ClientId { get; set; }

    [MaxLength(30)]
    [MinLength(5)]
    [Required]
    public string Adres { get; set; }

    [MaxLength(30)]
    [MinLength(5)]
    [Required]
    public string Residence { get; set; }

    [Required]
    public bool AboveSixteen { get; set; }

    public int HulpverlenerId { get; set; }
    public Hulpverlener Hulpverlener { get; set; }

    public ApplicationUser Account { get; set; }
    
    public Ouder Ouder { get; set; }

    public Aanmelding Aanmelding { get; set; }
}
