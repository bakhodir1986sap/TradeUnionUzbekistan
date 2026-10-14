using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbpt_putyovki")]
public partial class TbptPutyovki
{
    [Key]
    public long REC_ID { get; set; }
    public string? DEPARTMENT_CODE { get; set; }
    public int? SANATORIUM_ID { get; set; }
    public long? CAMP_ID { get; set; }
    public long? USER_ID { get; set; }
    public string? SERIAL_BEGIN { get; set; }
    public string? SERIAL_END { get; set; }
    public DateTime? REST_BEGIN_DATE { get; set; }
    public DateTime? REST_END_DATE { get; set; }
    public decimal? PRICE { get; set; }
    public string? STATUS { get; set; }
    public DateTime? MODIFY_DATE { get; set; }
    public int? QUANTITY_IN { get; set; }
    public decimal? REMAIN_BAL { get; set; }
    public int? QUANTITY_OUT { get; set; }
    public int? QUANTITY_REMAIN { get; set; }
    public int? SMFIL { get; set; }

    public virtual TblSpravSan? tblSprav_San { get; set; }
    public virtual User? user { get; set; }
}
