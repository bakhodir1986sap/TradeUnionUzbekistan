namespace KasabaKIAT.Core.Services;

public interface IDbContextRagService
{
    Task<string> BuildContextBlockAsync(string userQuestion, long userId, CancellationToken ct);
}
