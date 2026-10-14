using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("t_normativdoc")]
public partial class TNormativDoc
{
    [Key]
    public long id { get; set; }
    public string? name_uz { get; set; }
    public string? name_ru { get; set; }
    public DateTime? s_create { get; set; }
    public string? description { get; set; }
}
