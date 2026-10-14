using KasabaKIAT.Core.DTOs.Auth;

namespace KasabaKIAT.Core.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<LoginResponse?> RefreshAsync(string refreshToken);
    Task<bool> LogoutAsync(string refreshToken);
}
