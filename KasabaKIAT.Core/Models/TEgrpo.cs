using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("t_egrpo")]
public class TEgrpo
{
    [Key] public long id { get; set; }
    public string? inn { get; set; }
    public string? name_full { get; set; }
    public string? name_short { get; set; }
    public string? address { get; set; }
    public string? soato_id { get; set; }
    public long? okonx_id { get; set; }
    public long? opf_id { get; set; }
    public string? director_name { get; set; }
    public string? director_inn { get; set; }
}
