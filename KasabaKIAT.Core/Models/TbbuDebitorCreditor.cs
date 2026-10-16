using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbbu_debitor_creditor")]
public class TbbuDebitorCreditor
{
    [Key] public long ID { get; set; }
    public string? DEPARTMENT_BASE { get; set; }
    public string? ACCOUNT_CODE { get; set; }
    public int? CUR_YEAR { get; set; }
    public int? CUR_KV { get; set; }
    public decimal? DEBIT_BAL { get; set; }
    public decimal? CREDIT_BAL { get; set; }
    public long? ORG_ID { get; set; }
}
