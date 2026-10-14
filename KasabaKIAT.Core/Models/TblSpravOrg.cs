using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tblSprav_Org")]
public class TblSpravOrg
{
    [Key] public long ID { get; set; }
    public string? Name_Org { get; set; }
    public string? INN { get; set; }
    public string? OWNER_CODE { get; set; }
    public long? ID_Ot { get; set; }
    public string? DepartmentBase { get; set; }
    public long? UserId { get; set; }
    public string? Address { get; set; }
    public string? Name_Short { get; set; }
    public string? Name_Full { get; set; }
    public string? SOATO_ID { get; set; }
}
