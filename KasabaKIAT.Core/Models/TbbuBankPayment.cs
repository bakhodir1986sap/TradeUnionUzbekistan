using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbbu_bank_payments")]
public class TbbuBankPayment
{
    [Key] public long ID { get; set; }
    public string? DEPARTMENT_BASE { get; set; }
    public string? DOC_NUM { get; set; }
    public DateTime? DOC_DATE { get; set; }
    public decimal? SUMMA { get; set; }
    public string? PAYER_NAME { get; set; }
    public string? PAYER_INN { get; set; }
    public string? DESCRIPTION { get; set; }
    public int? CUR_YEAR { get; set; }
    public int? CUR_KV { get; set; }
    public long? USER_ID { get; set; }
}
