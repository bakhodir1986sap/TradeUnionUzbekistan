namespace KasabaKIAT.Core.DTOs.Chat;

public class OllamaOptions
{
    public string BaseUrl { get; set; } = "http://localhost:11434";
    public string ChatModel { get; set; } = "llama3.2";
    public string EmbeddingModel { get; set; } = "nomic-embed-text";
    public int MaxContextTokens { get; set; } = 4096;
}
