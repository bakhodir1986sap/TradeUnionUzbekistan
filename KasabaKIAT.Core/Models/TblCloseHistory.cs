using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblCloseHistory")]
public class TblCloseHistory
{
    [Key] public long REC_ID { get; set; }
    public string? DEPARTMENT_CODE { get; set; }
    public DateTime? CLOSED_DATE { get; set; }
    public string? REMARK { get; set; }
    public long? USER_ID { get; set; }
}
