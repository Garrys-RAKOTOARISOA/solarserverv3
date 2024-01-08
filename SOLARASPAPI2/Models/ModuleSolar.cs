using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models;

[Table("module")]
public class ModuleSolar
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("qrcode")]
    public string QrCode { get; set; }
    
    [Column("nommodule")]
    public string NomModule { get; set; }
    
    [Column("idbatterie")]
    [DisplayName("typebatterie")]
    public int IdBatterie { get; set; } 
    
    [ForeignKey("IdBatterie")]
    public virtual Batterie? Batterie { get; set; }
}