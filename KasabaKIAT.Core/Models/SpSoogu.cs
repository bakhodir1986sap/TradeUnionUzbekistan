using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("sp_soogu")]
public class SpSoogu
{
    [Key] public long id { get; set; }
    public long? version { get; set; }
    public string? code { get; set; }
    public string? name_ru { get; set; }
    public string? name_uz { get; set; }
    public string? name_uzl { get; set; }
    public DateTime? s_create { get; set; }
    public string? s_status { get; set; }
    public long? s_user_id { get; set; }
    public long? NATIVE_ID { get; set; }
}
