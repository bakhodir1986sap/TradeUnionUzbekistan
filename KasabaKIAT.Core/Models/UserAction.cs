using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("useractions")]
public partial class UserAction
{
    [Key]
    public long id { get; set; }
    public long? user_id { get; set; }
    public string? action_name { get; set; }
    public DateTime? action_date { get; set; }
    public string? description { get; set; }
}
