using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("functionality")]
public partial class Functionality
{
    [Key]
    public long id { get; set; }
    public string? name { get; set; }
    public string? description { get; set; }

    public virtual ICollection<RolePermission> role_permissions { get; set; } = [];
}
