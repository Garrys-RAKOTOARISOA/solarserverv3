using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models;

[Table("panneaudata")]
public class PanneauData
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idmodule")]
    [DisplayName("module")]
    public int IdModule { get; set; }
    
    [Column("production")]
    public double Production { get; set; }
    
    [Column("tension")]
    public double Tension { get; set; }
    
    [Column("puissance")]
    public double Puissance { get; set; }
    
    [Column("courant")]
    public double Courant { get; set; }
    
    [Column("temps")]
    public DateTime Temps { get; set; }
    
    [ForeignKey("IdModule")]
    public virtual ModuleSolar? Module { get; set; }
}