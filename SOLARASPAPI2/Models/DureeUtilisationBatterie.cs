using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models;

[Table("dureeutilisationbatterie")]
public class DureeUtilisationBatterie
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idmodule")]
    [DisplayName("module")]
    public int IdModule { get; set; }
    
    [Column("duree")]
    public double Duree { get; set; }
    
    [Column("date")]
    public DateOnly Date { get; set; }
    
    [ForeignKey("IdModule")]
    public virtual ModuleSolar? Module { get; set; }
}