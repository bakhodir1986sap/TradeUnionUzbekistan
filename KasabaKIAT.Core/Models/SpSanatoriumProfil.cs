using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("sp_sanatorium_profil")]
public partial class SpSanatoriumProfil
{
    [Key]
    public long id { get; set; }
    public string? code { get; set; }
    public string? name_ru { get; set; }
    public string? name_uz { get; set; }
    public string? name_uzl { get; set; }
    public DateTime? s_create { get; set; }
}
