using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models;

[Table("planningprise")]
public class PlanningPrise
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
    
    [Column("valeurconsommation")]
    public double ValeurConsommation { get; set; }
    
    [Column("done")]
    public bool Done { get; set; }
    
    [ForeignKey("IdModule")]
    public virtual ModuleSolar? Module { get; set; }
}