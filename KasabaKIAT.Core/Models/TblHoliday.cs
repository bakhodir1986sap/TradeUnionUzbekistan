using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblHolidays")]
public class TblHoliday
{
    [Key] public long REC_ID { get; set; }
    public DateTime? SET_DATE { get; set; }
    public short DATE_TYPE { get; set; }
    public short STATUS { get; set; }
}
