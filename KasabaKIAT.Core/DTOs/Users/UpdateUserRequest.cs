namespace KasabaKIAT.Core.DTOs.Users;

public class UpdateUserRequest
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public long? RoleId { get; set; }
    public long? DepId { get; set; }
    public string? RegionId { get; set; }
    public long? BranchId { get; set; }
    public bool? IsSuperAdmin { get; set; }
    public bool? AccountLocked { get; set; }
    public bool? AccountExpired { get; set; }
    public string? State { get; set; }
    public string? Language { get; set; }
    public string? UserType { get; set; }
}
