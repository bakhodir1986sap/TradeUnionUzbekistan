using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("t_sanatorium")]
public partial class TSanatorium
{
    [Key]
    public long id { get; set; }
    public string? name_uz { get; set; }
    public string? name_ru { get; set; }
    public string? code { get; set; }
    public DateTime? s_create { get; set; }
    public long? sanatorium_type_id { get; set; }
}
