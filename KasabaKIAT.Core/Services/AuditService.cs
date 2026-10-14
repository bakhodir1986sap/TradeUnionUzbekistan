using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;

namespace KasabaKIAT.Core.Services;

public class AuditService(KasabaKIATContext db, ILogger<AuditService> logger) : IAuditService
{
    public async Task LogAsync(long userId, string action, string? description = null)
    {
        try
        {
            db.UserActions.Add(new UserAction
            {
                user_id = userId,
                action_name = action,
                action_date = DateTime.UtcNow,
                description = description
            });
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Audit failure must never crash the main request
            logger.LogWarning(ex, "Failed to write audit entry for user {UserId} action {Action}", userId, action);
        }
    }
}
