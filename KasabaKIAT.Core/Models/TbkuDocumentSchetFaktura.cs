using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("tbku_DocumentSchetFaktura")]
public class TbkuDocumentSchetFaktura
{
    [Key] public long ID { get; set; }
    public string? DocNomer { get; set; }
    public DateTime? DocData { get; set; }
    public string? ProfsoyzKod { get; set; }
    public int? KolichestvoPutevok { get; set; }
    public decimal? SummaDoc { get; set; }
    public string? DocTypeRealVozv { get; set; }
    public string? ProfsoyzDoverennost { get; set; }
    public DateTime? ProfsoyzDoverennostData { get; set; }
    public string? ProfsoyzDoverennostMOL { get; set; }
    public string? ProfsoyzDogovor { get; set; }
}
