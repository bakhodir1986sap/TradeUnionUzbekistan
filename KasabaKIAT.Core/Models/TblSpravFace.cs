using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblSprav_Face")]
public partial class TblSpravFace
{
    [Key]
    public long ID { get; set; }
    public string? Name_Face { get; set; }
    public long? UserId { get; set; }
}
