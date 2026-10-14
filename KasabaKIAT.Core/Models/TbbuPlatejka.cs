using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbbu_platejka")]
public class TbbuPlatejka
{
    [Key] public long REC_ID { get; set; }
    public DateTime? PLAT_DATE { get; set; }
    public string? PLAT_NUM { get; set; }
    public string? PLATELNAME { get; set; }
    public string? PLATELINN { get; set; }
    public string? PLATELRASCHET { get; set; }
    public string? PLATELBANKNAME { get; set; }
    public string? PLATELBANKMFO { get; set; }
    public string? POLUCHNAME { get; set; }
    public string? POLUCHINN { get; set; }
    public string? POLUCHRASCHET { get; set; }
    public string? POLUCHBANKNAME { get; set; }
    public string? POLUCHBANKMFO { get; set; }
    public decimal? SUMMA { get; set; }
    public string? COMMENT { get; set; }
    public string? RUKOVODITEL { get; set; }
    public string? GLAVBUX { get; set; }
    public string? DEPARTMENT_CODE { get; set; }
    public long? USR_ID { get; set; }
    public DateTime? MODIFY_DATE { get; set; }
}
