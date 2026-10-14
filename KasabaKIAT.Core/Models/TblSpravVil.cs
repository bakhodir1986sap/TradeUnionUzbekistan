using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblSprav_Vil")]
public partial class TblSpravVil
{
    [Key]
    public long ID { get; set; }
    public string? Name_Vil { get; set; }
    public string? CodeVil { get; set; }
    public long? RegID { get; set; }
}
