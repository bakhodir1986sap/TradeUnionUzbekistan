using KasabaKIAT.Core.DTOs.Chat;

namespace KasabaKIAT.Core.Services;

public interface IChatService
{
    IAsyncEnumerable<string> StreamAnswerAsync(
        ChatMessageRequest request,
        long userId,
        CancellationToken ct);

    Task<IngestDocumentResponse> IngestDocumentAsync(
        string sourceName,
        Stream documentStream,
        string contentType,
        long userId,
        CancellationToken ct);
}
