using KasabaKIAT.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = "admin")]
public class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await roleService.GetAllAsync();
        return Ok(roles);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var role = await roleService.GetByIdAsync(id);
        return role == null ? NotFound() : Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
    {
        var actorId = GetActorId();
        var role = await roleService.CreateAsync(request.Name, actorId);
        return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] CreateRoleRequest request)
    {
        var actorId = GetActorId();
        var role = await roleService.UpdateAsync(id, request.Name, actorId);
        return role == null ? NotFound() : Ok(role);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var actorId = GetActorId();
        var result = await roleService.DeleteAsync(id, actorId);
        return result ? NoContent() : NotFound();
    }

    private long GetActorId() =>
        long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;
}

public record CreateRoleRequest(string Name);
