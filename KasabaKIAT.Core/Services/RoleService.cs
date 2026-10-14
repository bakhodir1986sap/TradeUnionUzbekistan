using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs.Roles;
using KasabaKIAT.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Services;

public class RoleService(KasabaKIATContext db) : IRoleService
{
    public async Task<List<RoleDto>> GetAllAsync()
    {
        return await db.Roles
            .Include(r => r.role_permissions)
                .ThenInclude(rp => rp.functionality)
            .Select(r => ToDto(r))
            .ToListAsync();
    }

    public async Task<RoleDto?> GetByIdAsync(long id)
    {
        var role = await db.Roles
            .Include(r => r.role_permissions)
                .ThenInclude(rp => rp.functionality)
            .FirstOrDefaultAsync(r => r.id == id);
        return role == null ? null : ToDto(role);
    }

    public async Task<RoleDto> CreateAsync(string name)
    {
        var role = new Role { name = name, version = 0, s_status = "1" };
        db.Roles.Add(role);
        await db.SaveChangesAsync();
        return ToDto(role);
    }

    public async Task<RoleDto?> UpdateAsync(long id, string name)
    {
        var role = await db.Roles.FindAsync(id);
        if (role == null) return null;
        role.name = name;
        await db.SaveChangesAsync();
        return ToDto(role);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var role = await db.Roles.FindAsync(id);
        if (role == null) return false;
        db.Roles.Remove(role);
        await db.SaveChangesAsync();
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
