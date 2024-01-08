using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models;

[Table("typebatterie")]
public class Batterie
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("valeur")]
    public double Valeur { get; set; }
}