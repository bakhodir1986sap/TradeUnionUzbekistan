using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbbu_finOtchetSetting")]
public class TbbuFinOtchetSetting
{
    [Key] public long REC_ID { get; set; }
    public int? OTCHET_PART { get; set; }
    public string? CODE { get; set; }
    public string? DEBIT_SIDE { get; set; }
    public string? CREDIT_SIDE { get; set; }
    public string? ACCOUNT_CODE { get; set; }
    public int? DATA_TYPE { get; set; }
    public string? DATA_DESC { get; set; }
    public string? DEPARTMENT_BASE { get; set; }
    public long? USR_ID { get; set; }
}
