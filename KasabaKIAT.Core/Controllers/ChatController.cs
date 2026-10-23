using KasabaKIAT.Core.DTOs.Chat;
using KasabaKIAT.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController(IChatService chatService, ILogger<ChatController> logger) : ControllerBase
{
    [HttpPost("message")]
    public async Task StreamMessage([FromBody] ChatMessageRequest request, CancellationToken ct)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        Response.Headers.Append("Cache-Control", "no-cache");
        Response.Headers.Append("X-Accel-Buffering", "no");

        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            await foreach (var delta in chatService.StreamAnswerAsync(request, userId, ct))
            {
                var payload = JsonSerializer.Serialize(new { delta });
                await Response.WriteAsync($"data: {payload}\n\n", ct);
                await Response.Body.FlushAsync(ct);
            }
        }
        catch (OperationCanceledException)
        {
            // Client disconnected — normal for streaming
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error streaming chat response for user {UserId}", userId);
            var errorPayload = JsonSerializer.Serialize(new { error = "Ошибка при получении ответа" });
            await Response.WriteAsync($"data: {errorPayload}\n\n", ct);
            await Response.Body.FlushAsync(ct);
        }

        await Response.WriteAsync("data: [DONE]\n\n", ct);
        await Response.Body.FlushAsync(ct);
    }

    [HttpPost("ingest")]
    [Authorize(Policy = "AdminOrSuperAdmin")]
    [RequestSizeLimit(52_428_800)]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> IngestDocument(IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не выбран или пустой.");

        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await using var stream = file.OpenReadStream();

        var result = await chatService.IngestDocumentAsync(
            file.FileName, stream, file.ContentType, userId, ct);

        return Ok(result);
    }
}
