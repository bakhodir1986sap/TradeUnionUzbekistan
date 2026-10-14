using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("sp_position")]
public partial class SpPosition
{
    [Key]
    public long id { get; set; }
    public int? version { get; set; }
    public string? name_ru { get; set; }
    public string? name_uz { get; set; }
    public string? name_uzl { get; set; }
    public string? s_status { get; set; }
    public long? s_user_id { get; set; }
    public DateTime? s_create { get; set; }
    public long? NATIVE_ID { get; set; }
}
