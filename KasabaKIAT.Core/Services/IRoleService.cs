using KasabaKIAT.Core.DTOs.Roles;

namespace KasabaKIAT.Core.Services;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllAsync();
    Task<RoleDto?> GetByIdAsync(long id);
    Task<RoleDto> CreateAsync(string name, long actorUserId);
    Task<RoleDto?> UpdateAsync(long id, string name, long actorUserId);
    Task<bool> DeleteAsync(long id, long actorUserId);
}
