using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblBrons")]
public class TblBron
{
    [Key] public long ID { get; set; }
    public long? ID_Sanat { get; set; }
    public string? Num_Put { get; set; }
    public bool? BRON_FLG { get; set; }
    public DateTime? BRON_DATE { get; set; }
    public DateTime? EXPIRE_DATE { get; set; }
    public long? USER_ID { get; set; }
    public string? DEPARTMENT_BASE { get; set; }
    public string? PRIM { get; set; }
}
