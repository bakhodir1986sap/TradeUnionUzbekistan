using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs.Roles;
using KasabaKIAT.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Services;

public class RoleService(KasabaKIATContext db, IAuditService audit, ILogger<RoleService> logger) : IRoleService
{
    public async Task<List<RoleDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await db.Roles
            .Include(r => r.role_permissions)
                .ThenInclude(rp => rp.functionality)
            .Select(r => ToDto(r))
            .ToListAsync(ct);
    }

    public async Task<RoleDto?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var role = await db.Roles
            .Include(r => r.role_permissions)
                .ThenInclude(rp => rp.functionality)
            .FirstOrDefaultAsync(r => r.id == id, ct);
        return role == null ? null : ToDto(role);
    }

    public async Task<RoleDto> CreateAsync(string name, long actorUserId, CancellationToken ct = default)
    {
        var role = new Role { name = name, version = 0, s_status = "1" };
        db.Roles.Add(role);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Role created: Id={RoleId} Name={RoleName}", role.id, name);
        await audit.LogAsync(actorUserId, "ROLE_CREATE", $"Name={name}", ct);
        return ToDto(role);
    }

    public async Task<RoleDto?> UpdateAsync(long id, string name, long actorUserId, CancellationToken ct = default)
    {
        var role = await db.Roles.FindAsync([id], ct);
        if (role == null) return null;
        role.name = name;
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Role updated: Id={RoleId} Name={RoleName}", id, name);
        await audit.LogAsync(actorUserId, "ROLE_UPDATE", $"RoleId={id} Name={name}", ct);
        return ToDto(role);
    }

    public async Task<bool> DeleteAsync(long id, long actorUserId, CancellationToken ct = default)
    {
        var role = await db.Roles.FindAsync([id], ct);
        if (role == null) return false;
        db.Roles.Remove(role);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Role deleted: Id={RoleId}", id);
        await audit.LogAsync(actorUserId, "ROLE_DELETE", $"RoleId={id}", ct);
        return true;
    }

    private static RoleDto ToDto(Role r) => new()
    {
        Id = r.id,
        Name = r.name,
        Permissions = r.role_permissions.Select(rp => new PermissionDto
        {
            Id = rp.id,
            FunctionalityName = rp.functionality?.name,
            Action = rp.action
        }).ToList()
    };
}
