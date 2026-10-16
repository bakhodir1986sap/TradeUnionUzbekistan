using KasabaKIAT.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Policy = "AdminOrSuperAdmin")]
public class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var roles = await roleService.GetAllAsync(ct);
        return Ok(roles);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var role = await roleService.GetByIdAsync(id, ct);
        return role == null ? NotFound() : Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken ct)
    {
        var actorId = GetActorId();
        var role = await roleService.CreateAsync(request.Name, actorId, ct);
        return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] CreateRoleRequest request, CancellationToken ct)
    {
        var actorId = GetActorId();
        var role = await roleService.UpdateAsync(id, request.Name, actorId, ct);
        return role == null ? NotFound() : Ok(role);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var actorId = GetActorId();
        var result = await roleService.DeleteAsync(id, actorId, ct);
        return result ? NoContent() : NotFound();
    }

    private long GetActorId() =>
        long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;
}

public record CreateRoleRequest(string Name);
