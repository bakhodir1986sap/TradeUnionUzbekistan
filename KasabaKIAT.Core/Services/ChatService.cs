using DocumentFormat.OpenXml.Packaging;
using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs.Chat;
using KasabaKIAT.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using UglyToad.PdfPig;

namespace KasabaKIAT.Core.Services;

#pragma warning disable SKEXP0070  // Ollama connector is experimental

public class ChatService(
    KasabaKIATContext db,
    IOptions<OllamaOptions> options,
    IDbContextRagService dbRag,
    IHttpClientFactory httpClientFactory,
    ILogger<ChatService> logger) : IChatService
{
    private const int ChunkSize = 2048;
    private const int ChunkOverlap = 200;
    private const float SimilarityThreshold = 0.60f;
    private const int TopKChunks = 5;

    public async IAsyncEnumerable<string> StreamAnswerAsync(
        ChatMessageRequest request,
        long userId,
        [EnumeratorCancellation] CancellationToken ct)
    {
        var opts = options.Value;
        var http = httpClientFactory.CreateClient("ollama");

        // Use OllamaApiClient (non-deprecated) — separate instances for different models
        var embeddingClient = new OllamaApiClient(http, opts.EmbeddingModel);
        var embeddingService = embeddingClient.AsTextEmbeddingGenerationService();
        var chatClient = new OllamaApiClient(http, opts.ChatModel);
        var chatService = chatClient.AsChatCompletionService();

        // 1. Build DB context block
        var dbContext = await dbRag.BuildContextBlockAsync(request.Message, userId, ct);

        // 2. Retrieve relevant document chunks via cosine similarity
        var docContext = await RetrieveDocumentContextAsync(request.Message, embeddingService, ct);

        // 3. Build system prompt
        var systemPrompt = BuildSystemPrompt(dbContext, docContext);

        // 4. Build chat history
        var history = new ChatHistory(systemPrompt);
        if (request.History != null)
        {
            foreach (var turn in request.History)
            {
                if (turn.Role == "user")
                    history.AddUserMessage(turn.Content);
                else
                    history.AddAssistantMessage(turn.Content);
            }
        }
        history.AddUserMessage(request.Message);

        // 5. Stream tokens
        await foreach (var content in chatService.GetStreamingChatMessageContentsAsync(
            history, cancellationToken: ct))
        {
            if (!string.IsNullOrEmpty(content.Content))
                yield return content.Content;
        }
    }

    public async Task<IngestDocumentResponse> IngestDocumentAsync(
        string sourceName,
        Stream documentStream,
        string contentType,
        long userId,
        CancellationToken ct)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var opts = options.Value;
        var http = httpClientFactory.CreateClient("ollama");
        var ollamaClient = new OllamaApiClient(http, opts.EmbeddingModel);
        var embeddingService = ollamaClient.AsTextEmbeddingGenerationService();

        // 1. Extract text
        var text = ExtractText(documentStream, sourceName, contentType);

        // 2. Chunk text
        var chunks = ChunkText(text, ChunkSize, ChunkOverlap);

        // 3. Delete existing chunks for this source
        var existing = db.ChatDocumentChunks.Where(c => c.source_name == sourceName);
        db.ChatDocumentChunks.RemoveRange(existing);
        await db.SaveChangesAsync(ct);

        // 4. Embed and save each chunk
        var newChunks = new List<ChatDocumentChunk>(chunks.Count);
        for (int i = 0; i < chunks.Count; i++)
        {
            var embeddings = await embeddingService.GenerateEmbeddingsAsync(
                [chunks[i]], cancellationToken: ct);
            var embeddingJson = JsonSerializer.Serialize(embeddings[0].ToArray());
            newChunks.Add(new ChatDocumentChunk
            {
                source_name = sourceName,
                chunk_index = i,
                chunk_text = chunks[i],
                embedding = embeddingJson,
                created_at = DateTime.UtcNow,
                ingested_by = userId
            });
        }

        db.ChatDocumentChunks.AddRange(newChunks);
        await db.SaveChangesAsync(ct);

        sw.Stop();
        logger.LogInformation("Ingested {Source}: {Count} chunks in {Ms}ms",
            sourceName, newChunks.Count, sw.ElapsedMilliseconds);

        return new IngestDocumentResponse(sourceName, newChunks.Count, sw.Elapsed);
    }

    private async Task<string> RetrieveDocumentContextAsync(
        string query,
        ITextEmbeddingGenerationService embeddingService,
        CancellationToken ct)
    {
        var totalChunks = await db.ChatDocumentChunks.CountAsync(ct);
        if (totalChunks == 0)
            return string.Empty;

        var queryEmbeddings = await embeddingService.GenerateEmbeddingsAsync(
            [query], cancellationToken: ct);
        var queryVec = queryEmbeddings[0].ToArray();

        var allChunks = await db.ChatDocumentChunks
            .Select(c => new { c.chunk_text, c.embedding })
            .ToListAsync(ct);

        var scored = allChunks
            .Select(c => new
            {
                c.chunk_text,
                Score = CosineSimilarity(queryVec, JsonSerializer.Deserialize<float[]>(c.embedding)!)
            })
            .Where(c => c.Score >= SimilarityThreshold)
            .OrderByDescending(c => c.Score)
            .Take(TopKChunks)
            .ToList();

        if (scored.Count == 0)
            return string.Empty;

        return string.Join("\n\n", scored.Select(c => c.chunk_text));
    }

    private static string BuildSystemPrompt(string dbContext, string docContext)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Вы — помощник для системы управления профсоюзом KasabaKIAT.");
        sb.AppendLine("Отвечайте на русском или узбекском языке в зависимости от языка вопроса.");
        sb.AppendLine("Будьте точны и кратки. Если не знаете ответа — скажите об этом.");

        if (!string.IsNullOrWhiteSpace(dbContext))
        {
            sb.AppendLine("\n=== ДАННЫЕ СИСТЕМЫ ===");
            sb.AppendLine(dbContext);
        }

        if (!string.IsNullOrWhiteSpace(docContext))
        {
            sb.AppendLine("\n=== ДОКУМЕНТЫ ===");
            sb.AppendLine(docContext);
        }

        return sb.ToString();
    }

    private static string ExtractText(Stream stream, string fileName, string contentType)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();

        if (ext == ".pdf" || contentType == "application/pdf")
        {
            using var pdf = PdfDocument.Open(stream);
            var sb = new StringBuilder();
            foreach (var page in pdf.GetPages())
                sb.AppendLine(page.Text);
            return sb.ToString();
        }

        if (ext == ".docx" ||
            contentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
        {
            using var doc = WordprocessingDocument.Open(stream, false);
            var body = doc.MainDocumentPart?.Document?.Body;
            if (body == null) return string.Empty;
            return string.Join("\n", body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
                .Select(p => p.InnerText));
        }

        // Plain text fallback
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    private static List<string> ChunkText(string text, int chunkSize, int overlap)
    {
        var chunks = new List<string>();
        var start = 0;
        while (start < text.Length)
        {
            var end = Math.Min(start + chunkSize, text.Length);
            chunks.Add(text[start..end]);
            start += chunkSize - overlap;
            if (start >= text.Length) break;
        }
        return chunks;
    }

    private static float CosineSimilarity(float[] a, float[] b)
    {
        if (a.Length != b.Length) return 0f;
        float dot = 0, magA = 0, magB = 0;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }
        var denom = MathF.Sqrt(magA) * MathF.Sqrt(magB);
        return denom == 0 ? 0f : dot / denom;
    }
}

#pragma warning restore SKEXP0070
