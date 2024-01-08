using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models;

[Table("prisedata")]
public class PriseData
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idmodule")]
    [DisplayName("module")]
    public int IdModule { get; set; }
    
    [Column("consommation")]
    public double Consommation { get; set; }
    
    [Column("tension")]
    public double Tension { get; set; }
    
    [Column("puissance")]
    public double Puissance { get; set; }
    
    [Column("courant")]
    public double Courant { get; set; }
    
    [Column("temps")]
    public DateTime Temps { get; set; }
    
    [NotMapped]
    public string Couleur { get; set; }
    
    [ForeignKey("IdModule")]
    public virtual ModuleSolar? Module { get; set; }
}