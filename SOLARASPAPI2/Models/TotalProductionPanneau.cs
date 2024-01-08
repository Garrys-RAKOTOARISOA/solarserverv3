using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models;

[Table("totalproductionpanneau")]
public class TotalProductionPanneau
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idmodule")]
    [DisplayName("module")]
    public int IdModule { get; set; }
    
    [Column("total")]
    public double Total { get; set; }
    
    [Column("date")]
    public DateOnly Date { get; set; }
    
    [ForeignKey("IdModule")]
    public virtual ModuleSolar? Module { get; set; }
}