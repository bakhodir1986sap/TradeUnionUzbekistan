namespace KasabaKIAT.Core.Services;

public interface IAuditService
{
    Task LogAsync(long userId, string action, string? description = null, CancellationToken ct = default);
}
