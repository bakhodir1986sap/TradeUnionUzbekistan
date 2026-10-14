using KasabaKIAT.Core.DTOs.Users;

namespace KasabaKIAT.Core.Services;

public interface IUserService
{
    Task<(List<UserDto> Items, int Total)> GetUsersAsync(int page, int pageSize, string? search);
    Task<UserDto?> GetByIdAsync(long id);
    Task<UserDto> CreateAsync(CreateUserRequest request);
    Task<UserDto?> UpdateAsync(long id, UpdateUserRequest request);
    Task<bool> DeactivateAsync(long id);
}
