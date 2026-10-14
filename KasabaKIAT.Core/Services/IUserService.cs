using KasabaKIAT.Core.DTOs;
using KasabaKIAT.Core.DTOs.Users;

namespace KasabaKIAT.Core.Services;

public interface IUserService
{
    Task<PagedResult<UserDto>> GetUsersAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    Task<UserDto?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
    Task<UserDto?> UpdateAsync(long id, UpdateUserRequest request, CancellationToken ct = default);
    Task<bool> DeactivateAsync(long id, CancellationToken ct = default);
}
