using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbpt_yearlyplanotrasl")]
public class TbptYearlyPlanOtrasl
{
    [Key] public long REC_ID { get; set; }
    public string? YEAR { get; set; }
    public string? KVARTAL { get; set; }
    public long? SANATORIUM_ID { get; set; }
    public long? CAMP_ID { get; set; }
    public decimal? PRICE { get; set; }
    public long? QUANTITY { get; set; }
    public string? STATUS { get; set; }
    public long? USER_ID { get; set; }
    public long? OTRASL_ID { get; set; }
    public DateTime? MODIFY_DATE { get; set; }
    public string? DEPARTMANT_BASE { get; set; }
}
