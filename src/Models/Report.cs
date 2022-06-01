using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Report
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    // [MaxLength(60)]
    // [MinLength(10)]
    public string Content { get; set; }

    [Required]
    public bool isHandled { get; set; }

    public int MessageId { get; set; }
    public Message Message { get; set; }

    public string HandlerId { get; set; }
    public ApplicationUser Handler { get; set; }
}