using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblFace_Med")]
public class TblFaceMed
{
    [Key]
    public long ID { get; set; }
    public long? ID_Face { get; set; }
    public string? Med_Doc { get; set; }
    public DateTime? Med_Data { get; set; }
    public int? Med_San { get; set; }
    public string? Opl_Doc { get; set; }
    public DateTime? Opl_Data { get; set; }
    public decimal? Opl_Symma { get; set; }
    public string? Tal_Doc { get; set; }
    public DateTime? Tal_Data { get; set; }
    public long? UserId { get; set; }
    public string? DepartmentBase { get; set; }
}
