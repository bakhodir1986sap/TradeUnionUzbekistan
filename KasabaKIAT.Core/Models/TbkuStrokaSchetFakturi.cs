using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbku_StrokaSchetFakturi")]
public class TbkuStrokaSchetFakturi
{
    [Key] public long ID { get; set; }
    public long? SchetFactura_ID { get; set; }
    public string? SanatoryKod { get; set; }
    public string? SanatoryNaimenovanie { get; set; }
    public string? Blank1 { get; set; }
    public string? Blank2 { get; set; }
    public DateTime? DataZaezda { get; set; }
    public decimal? Cena { get; set; }
}
