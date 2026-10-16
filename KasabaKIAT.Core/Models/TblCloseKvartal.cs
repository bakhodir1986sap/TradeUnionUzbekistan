using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblCloseKvartal")]
public class TblCloseKvartal
{
    [Key] public long REC_ID { get; set; }
    public string? DEPARTMENT_CODE { get; set; }
    public int? PREVKV_YEAR { get; set; }
    public int? PREVKV { get; set; }
    public int? CURKV_YEAR { get; set; }
    public int? CURKV { get; set; }
    public int? NEXTKV_YEAR { get; set; }
    public int? NEXTKV { get; set; }
}
