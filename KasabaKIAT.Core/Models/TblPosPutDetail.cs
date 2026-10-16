using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

/// <summary>
/// KU invoice detail line — individual voucher in a receipt (tblPos_Put rows linked to tblPost)
/// </summary>
[Table("tblPos_Put")]
public class TblPosPutDetail
{
    [Key] public long ID { get; set; }
    public long? ID_Post { get; set; }
    public int? ID_Sanat { get; set; }
    public string? Num_Put { get; set; }
    public DateTime? DataSan { get; set; }
    public DateTime? DataSanEnd { get; set; }
    public decimal? Symma_Put { get; set; }
    public bool? ID_PolVoz { get; set; }
    public DateTime? Data_Post { get; set; }
    public long? UserId { get; set; }
    public string? Departmentbase { get; set; }
    public DateTime? TIMESTAMP_T { get; set; }
}
