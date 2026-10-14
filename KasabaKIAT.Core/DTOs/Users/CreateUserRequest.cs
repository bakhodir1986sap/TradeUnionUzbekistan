using System.ComponentModel.DataAnnotations;

namespace KasabaKIAT.Core.DTOs.Users;

public class CreateUserRequest
{
    [Required]
    public string Login { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; }
    public long? RoleId { get; set; }
    public long BkutId { get; set; }
    public long? DepId { get; set; }
    public string? RegionId { get; set; }
    public long BranchId { get; set; }
    public string? Language { get; set; }
    public string? UserType { get; set; }
}
