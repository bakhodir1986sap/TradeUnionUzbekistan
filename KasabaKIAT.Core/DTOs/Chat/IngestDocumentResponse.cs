namespace KasabaKIAT.Core.DTOs.Chat;

public record IngestDocumentResponse(string SourceName, int ChunkCount, TimeSpan Duration);
