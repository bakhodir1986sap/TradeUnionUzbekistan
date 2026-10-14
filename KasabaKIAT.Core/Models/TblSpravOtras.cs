using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblSprav_Otras")]
public partial class TblSpravOtras
{
    [Key]
    public long ID { get; set; }
    public string? Name_Ot { get; set; }
    public string? DepartmentBase { get; set; }
    public long? UserId { get; set; }
}
