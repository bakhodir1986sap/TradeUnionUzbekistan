using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("api_refresh_tokens")]
public partial class ApiRefreshToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long id { get; set; }
    public long user_id { get; set; }
    public string token { get; set; } = string.Empty;
    public DateTime expires_at { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? revoked_at { get; set; }
}
