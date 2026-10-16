using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbpt_raznoska_putyovki")]
public class TbptRaznoskaPutyovki
{
    [Key] public long ID { get; set; }
    public string? DEPARTMENT_BASE { get; set; }
    public long? FACE_ID { get; set; }
    public long? PUT_ID { get; set; }
    public int? ID_SANAT { get; set; }
    public string? NUM_PUT { get; set; }
    public decimal? PRICE { get; set; }
    public DateTime? DATE_ZAEZD { get; set; }
    public bool IS_MED { get; set; }
    public long? USER_ID { get; set; }
    public DateTime? TIMESTAMP_T { get; set; }
}
