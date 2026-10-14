using System.ComponentModel.DataAnnotations;

namespace KasabaKIAT.Core.DTOs.Auth;

public class RefreshRequest
{
    [Required, MaxLength(500)]
    public string RefreshToken { get; set; } = string.Empty;
}
