using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("role_permission")]
public partial class RolePermission
{
    [Key]
    public long id { get; set; }
    public long? role_id { get; set; }
    public long? functionality_id { get; set; }
    public string? action { get; set; }

    public virtual Role? role { get; set; }
    public virtual Functionality? functionality { get; set; }
}
