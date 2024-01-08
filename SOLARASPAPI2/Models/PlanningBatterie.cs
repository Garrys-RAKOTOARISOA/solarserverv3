using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models;

[Table("planningbatterie")]
public class PlanningBatterie
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("idmodule")]
    [DisplayName("module")]
    public int IdModule { get; set; }
    
    [Column("datedebut")]
    public DateTime DateDebut { get; set; }
    
    [Column("datefin")]
    public DateTime DateFin { get; set; }
    
    [Column("dateaction")]
    public DateTime DateAction { get; set; }
    
    [Column("valeurenergie")]
    public double ValeurEnergie { get; set; }
    
    [Column("done")]
    public bool Done { get; set; }
    
    [ForeignKey("IdModule")]
    public virtual ModuleSolar? Module { get; set; }
}