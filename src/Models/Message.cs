using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Content { get; set; }

    [Required]
    public DateTime Created { get; set; }

    public IList<Report> Reports { get; set; }

    public string AuthorId { get; set; }
    public ApplicationUser Author { get; set; }

    public int ChatId { get; set; }
    public Chat Chat { get; set; }
}