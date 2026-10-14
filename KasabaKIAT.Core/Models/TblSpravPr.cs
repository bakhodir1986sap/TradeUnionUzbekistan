using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblSprav_Pr")]
public partial class TblSpravPr
{
    [Key]
    public long ID { get; set; }
    public string? Name_Pr { get; set; }
    public decimal? Value { get; set; }
    public long? UserId { get; set; }
}
