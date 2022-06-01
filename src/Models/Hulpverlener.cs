using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Hulpverlener
{
    [Key]
    public int Id { get; set; }

    [MaxLength(30)]
    [MinLength(5)]
    [Required]
    public string Name { get; set; }

    [MaxLength(35)]
    [MinLength(5)]
    [Required]
    public string Adres { get; set; }

    public ApplicationUser Account { get; set; }

    public List<Client> Clients { get; set; }

    [MaxLength(15)]
    [Required]
    public string Specialisatie { get; set; }

    [Required]
    public string Intro { get; set; }

    [Required]
    public string Study { get; set; }

    [Required]
    public string OverJou { get; set; }

    [Required]
    public string Behandeling { get; set; }

    [Required]
    public string Foto { get; set; }

    [Required]
    public int VestigingId { get; set; }

    public List<ChatFrequency> ChatFrequencies { get; set; }
    public List<Melding> Meldingen { get; set; }
}