using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblPost")]
public class TblPost
{
    [Key] public long ID { get; set; }
    public int? ID_KV { get; set; }
    public DateTime? Data_Post { get; set; }
    public string? Num_Doc { get; set; }
    public long? ID_Pos { get; set; }
    public int? Num_Kol { get; set; }
    public decimal? Symma { get; set; }
    public string? Name_Dov { get; set; }
    public bool Vid { get; set; }
    public int? SmFil { get; set; }
    public long? UserId { get; set; }
    public string? DepartmentBase { get; set; }
    public int? BeginNumber { get; set; }
    public int? EndNumber { get; set; }
}
