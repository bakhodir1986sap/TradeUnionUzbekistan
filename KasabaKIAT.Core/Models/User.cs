using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("users")]
public partial class User
{
    [Key]
    public long id { get; set; }
    public int version { get; set; }
    public bool account_expired { get; set; }
    public bool account_locked { get; set; }
    public DateTime? changed { get; set; }
    public DateTime? created { get; set; }
    public string? email { get; set; }
    public DateTime? last_logged { get; set; }
    public string? last_logged_id { get; set; }
    public string? state { get; set; }
    public string? password { get; set; }
    public string? salt { get; set; }
    public string? language { get; set; }
    public bool? is_super_admin { get; set; }
    public string? user_name { get; set; }
    public string? s_comment { get; set; }
    public DateTime? s_create { get; set; }
    public string? s_status { get; set; }
    public long? s_user_id { get; set; }
    public string? usertype { get; set; }
    public long bkut_id { get; set; }
    public long? dep_id { get; set; }
    public long? role_id { get; set; }
    public string? REGION_ID { get; set; }
    public long BRANCH_ID { get; set; }
    public long? NATIVE_ID { get; set; }

    public virtual Role? role { get; set; }
}
