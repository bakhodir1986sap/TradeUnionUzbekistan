using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbcs_dictionary")]
public partial class TbcsDictionary
{
    [Key]
    public long REC_ID { get; set; }
    public string? NAME_RU { get; set; }
    public string? NAME_UZ { get; set; }
    public string? NAME_LT { get; set; }
    public string? CODE { get; set; }
    public bool? STATUS { get; set; }
    public int? USER_ID { get; set; }
    public long? DICTIONARY_TYPE_ID { get; set; }
}
