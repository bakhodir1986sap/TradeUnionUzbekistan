using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KasabaKIAT.Core.Models;

[Table("chat_document_chunks")]
public class ChatDocumentChunk
{
    [Key]
    public long id { get; set; }
    public string source_name { get; set; } = null!;
    public int chunk_index { get; set; }
    public string chunk_text { get; set; } = null!;
    public string embedding { get; set; } = null!; // JSON float array
    public DateTime created_at { get; set; }
    public long? ingested_by { get; set; }
}
