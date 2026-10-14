using System.ComponentModel.DataAnnotations;

namespace KasabaKIAT.Core.DTOs.Users;

public class CreateUserRequest
{
    [Required, MaxLength(100)]
    public string Login { get; set; } = string.Empty;
    [Required, MinLength(8), MaxLength(100)]
    public string Password { get; set; } = string.Empty;
    [MaxLength(200), EmailAddress]
    public string? Email { get; set; }
    public long? RoleId { get; set; }
    public long BkutId { get; set; }
    public long? DepId { get; set; }
    [MaxLength(50)]
    public string? RegionId { get; set; }
    public long BranchId { get; set; }
    [MaxLength(10)]
    public string? Language { get; set; }
    [MaxLength(50)]
    public string? UserType { get; set; }
}
