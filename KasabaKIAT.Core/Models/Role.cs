using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("roles")]
public partial class Role
{
    [Key]
    public long id { get; set; }
    public int version { get; set; }
    public string? name { get; set; }
    public string? s_comment { get; set; }
    public DateTime? s_create { get; set; }
    public string? s_status { get; set; }
    public long? s_user_id { get; set; }
    public long? NATIVE_ID { get; set; }

    public virtual ICollection<User> users { get; set; } = [];
    public virtual ICollection<RolePermission> role_permissions { get; set; } = [];
}
