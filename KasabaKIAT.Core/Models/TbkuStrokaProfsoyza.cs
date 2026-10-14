using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbku_StrokaProfsoyza")]
public class TbkuStrokaProfsoyza
{
    [Key] public long ID { get; set; }
    public string? ProfsoyzKod { get; set; }
    public string? ProfsoyzNaimenovanie { get; set; }
    public string? MATCH_DEPARTMENTBASE { get; set; }
}
