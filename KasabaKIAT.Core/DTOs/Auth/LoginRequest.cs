using System.ComponentModel.DataAnnotations;

namespace KasabaKIAT.Core.DTOs.Auth;

public class LoginRequest
{
    [Required, MaxLength(100)]
    public string Login { get; set; } = string.Empty;
    [Required, MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}
