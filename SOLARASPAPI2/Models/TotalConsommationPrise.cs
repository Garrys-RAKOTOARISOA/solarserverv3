using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models;

[Table("totalconsommationprise")]
public class TotalConsommationPrise
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
    public ModuleSolar? Module { get; set; }
}