using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPAPI.Models;

[Table("client")]
public class Client
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("nom")]
    public string Nom { get; set; }
    
    [Column("prenom")]
    public string Prenom { get; set; }
    
    [Column("email")]
    public string Email { get; set; }
    
    [Column("pass")]
    public string Pass { get; set; }
    
    [Column("codepostal")]
    public string CodePostal { get; set; }
    
    [Column("lienimage")]
    public string LienImage { get; set; }
    
    [Column("idmodule")]
    [DisplayName("module")]
    public int IdModule { get; set; }
    
    [ForeignKey("IdModule")]
    public virtual ModuleSolar? Module { get; set; }
}