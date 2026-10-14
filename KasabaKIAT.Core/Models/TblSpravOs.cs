using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblSprav_Os")]
public partial class TblSpravOs
{
    [Key]
    public long ID { get; set; }
    public string? Name_Os { get; set; }
    public long? UserId { get; set; }
}
