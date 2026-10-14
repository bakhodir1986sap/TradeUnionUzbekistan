using KasabaKIAT.Core.DTOs.Roles;

namespace KasabaKIAT.Core.Services;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllAsync();
    Task<RoleDto?> GetByIdAsync(long id);
    Task<RoleDto> CreateAsync(string name);
    Task<RoleDto?> UpdateAsync(long id, string name);
    Task<bool> DeleteAsync(long id);
}
