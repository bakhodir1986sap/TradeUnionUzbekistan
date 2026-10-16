using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblCloseKvartalHistory")]
public class TblCloseKvartalHistory
{
    [Key] public long REC_ID { get; set; }
    public string? DEPARTMENT_CODE { get; set; }
    public int? CLOSED_KVYEAR { get; set; }
    public int? CLOSED_KV { get; set; }
    public string? REMARK { get; set; }
}
