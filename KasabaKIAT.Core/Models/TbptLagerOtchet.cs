using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbpt_lager_otchet")]
public class TbptLagerOtchet
{
    [Key] public long ID { get; set; }
    public string? DEPARTMENT_BASE { get; set; }
    public int? CAMP_ID { get; set; }
    public int? YEAR { get; set; }
    public int? KVARTAL { get; set; }
    public int? CHILDREN_COUNT { get; set; }
    public decimal? SUMMA { get; set; }
    public bool IS_CLOSED { get; set; }
    public long? USER_ID { get; set; }
    public DateTime? CREATE_DATE { get; set; }
}
