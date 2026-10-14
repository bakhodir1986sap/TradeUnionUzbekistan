namespace KasabaKIAT.Core.DTOs.Auth;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserInfo User { get; set; } = new();
}

public class UserInfo
{
    public long Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
    public long BkutId { get; set; }
    public bool IsSuperAdmin { get; set; }
}
