using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ChatFrequency
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Status { get; set; }

    public int ChatId { get; set; }
    public Chat Chat { get; set; }

    public int OuderId { get; set; }
    public Ouder Ouder { get; set; }

    public int HulpverlenerId { get; set; }
    public Hulpverlener Hulpverlener { get; set; }

}