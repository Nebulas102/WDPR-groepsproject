using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Ouder
{
    [Key]
    public int Id { get; set; }

    [MaxLength(30)]
    [MinLength(5)]
    [Required]
    public string FullName { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    public List<ChatFrequency> ChatFrequencies { get; set; }

    public ApplicationUser Account { get; set; }

    public Aanmelding Aanmelding { get; set; }
}