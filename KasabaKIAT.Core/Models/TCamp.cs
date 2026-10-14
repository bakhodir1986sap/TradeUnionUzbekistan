using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("t_camp")]
public partial class TCamp
{
    [Key]
    public long id { get; set; }
    public string? camps_name { get; set; }
    public DateTime? s_create { get; set; }
    public string? s_status { get; set; }
}
