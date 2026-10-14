using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("t_departmentbase")]
public partial class TDepartmentBase
{
    [Key]
    public long id { get; set; }
    public string? DEPARTMENT_CODE { get; set; }
    public string? name_ru { get; set; }
    public string? name_uz { get; set; }
    public string? name_uzl { get; set; }
    public string? s_status { get; set; }
    public long? s_user_id { get; set; }
    public DateTime? s_create { get; set; }
    public long? PARENT_ID { get; set; }
    public string? MATCH_DEPARTMENTBASE { get; set; }
    public long? bkut_id { get; set; }
}
