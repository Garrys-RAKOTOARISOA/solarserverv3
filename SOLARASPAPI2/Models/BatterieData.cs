using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models;

[Table("batteriedata")]
public class BatterieData
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idmodule")]
    [DisplayName("module")]
    public int IdModule { get; set; }
    
    [Column("tension")]
    public double Tension { get; set; }
    
    [Column("energie")]
    public double Energie { get; set; }
    
    [Column("courant")]
    public double Courant { get; set; }
    
    [Column("pourcentage")]
    public double Pourcentage { get; set; }
    
    [Column("temps")]
    public DateTime Temps { get; set; }
    
    [Column("puissance")]
    public double Puissance { get; set; }
    
    [ForeignKey("IdModule")]
    public virtual ModuleSolar? Module { get; set; }
}