using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblCloseDays")]
public class TblCloseDay
{
    [Key] public long REC_ID { get; set; }
    public string? DEPARTMENT_CODE { get; set; }
    public DateTime? PREVDATE { get; set; }
    public DateTime? CURDAY { get; set; }
    public DateTime? NEXTDAY { get; set; }
}
