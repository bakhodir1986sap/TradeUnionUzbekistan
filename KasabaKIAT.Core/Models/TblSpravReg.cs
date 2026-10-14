using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblSprav_Reg")]
public partial class TblSpravReg
{
    [Key]
    public long ID { get; set; }
    public string? name_ru { get; set; }
    public string? name_uz { get; set; }
    public string? name_uzl { get; set; }
    public string? code { get; set; }
    public long? UserId { get; set; }
    public DateTime? s_create { get; set; }
}
