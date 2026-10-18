using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbku_SpisokRaspredeleniya")]
public class TbkuSpisokRaspredeleniya
{
    [Key] public long ID { get; set; }
    public long? GodZaezdov { get; set; }
    public string? SanatoryKod { get; set; }
    public string? ProfsoyzKod { get; set; }
    public string? SanatoryRazmeshenieKod { get; set; }
    public int? Putevok1kv { get; set; }
    public int? Putevok2kv { get; set; }
    public int? Putevok3kv { get; set; }
    public int? Putevok4kv { get; set; }
}
