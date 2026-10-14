using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblSprav_MFO")]
public partial class TblSpravMfo
{
    [Key]
    public long ID { get; set; }
    public string? Code_MFO { get; set; }
    public string? Name_MFO { get; set; }
}
