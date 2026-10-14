using KasabaKIAT.Core.DTOs.Roles;

namespace KasabaKIAT.Core.Services;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllAsync(CancellationToken ct = default);
    Task<RoleDto?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<RoleDto> CreateAsync(string name, long actorUserId, CancellationToken ct = default);
    Task<RoleDto?> UpdateAsync(long id, string name, long actorUserId, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, long actorUserId, CancellationToken ct = default);
}
