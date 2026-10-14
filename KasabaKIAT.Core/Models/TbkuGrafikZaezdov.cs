using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbku_GrafikZaezdovSanatoriev")]
public class TbkuGrafikZaezdov
{
    [Key] public long ID { get; set; }
    public long? GodZaezdov { get; set; }
    public string? SanatoryKod { get; set; }
    public string? SanatoryNaimenovanie { get; set; }
    public string? SanatoryProfil { get; set; }
    public string? SanatoryOblast { get; set; }
    public long? SanatoryKoekVsego { get; set; }
    public long? SanatoryKoekLetom { get; set; }
    public long? SanatoryKoekZimoy { get; set; }
    public string? SanatoryRazmeshenieKod { get; set; }
    public string? SanatoryRazmeshenieNaimenovanie { get; set; }
    public long? Putevok1kv { get; set; }
    public long? Putevok2kv { get; set; }
    public long? Putevok3kv { get; set; }
    public long? Putevok4kv { get; set; }
}
