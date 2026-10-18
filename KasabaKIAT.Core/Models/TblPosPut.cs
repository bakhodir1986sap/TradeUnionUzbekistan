using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblPos_Put")]
public class TblPosPut
{
    [Key] public long ID { get; set; }
    public int? ID_San { get; set; }
    public string? Num_Put { get; set; }
    public DateTime? DataNach { get; set; }
    public DateTime? DataKon { get; set; }
    public decimal? Price { get; set; }
    public string? Mes { get; set; }
    public string? Tip { get; set; }
    public string? Status { get; set; }
    public long? UserId { get; set; }
    public string? DEPARTMENT_BASE { get; set; }
    public string? SMFIL { get; set; }
    public DateTime? MODIFY_DATE { get; set; }
    public long? ID_Post { get; set; }
    public int? ID_Sanat { get; set; }
    public DateTime? DataSan { get; set; }
    public DateTime? DataSanEnd { get; set; }
    public decimal? Symma_Put { get; set; }
    public bool? ID_PolVoz { get; set; }
    public DateTime? Data_Post { get; set; }
    public string? Departmentbase { get; set; }
    public DateTime? TIMESTAMP_T { get; set; }
}
