using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbcs_dictionarytype")]
public partial class TbcsDictionaryType
{
    [Key]
    public long REC_ID { get; set; }
    public string? CODE { get; set; }
    public string? NAME_RU { get; set; }
    public string? NAME_UZ { get; set; }
}
