namespace KasabaKIAT.Core.DTOs.Roles;

public class RoleDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public List<PermissionDto> Permissions { get; set; } = [];
}

public class PermissionDto
{
    public long Id { get; set; }
    public string? FunctionalityName { get; set; }
    public string? Action { get; set; }
}
