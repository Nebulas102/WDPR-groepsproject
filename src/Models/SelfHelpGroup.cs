using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;

public class SelfHelpGroup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(20)]
    [MinLength(5)]
    [Required]
    public string Name { get; set; }

    [MaxLength(40)]
    [MinLength(5)]
    [Required]
    public string Description { get; set; }

    public bool AboveSixteen { get; set; }

    public int ChatId { get; set; }
    public Chat Chat { get; set; }
}