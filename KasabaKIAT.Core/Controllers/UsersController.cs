using KasabaKIAT.Core.DTOs.Users;
using KasabaKIAT.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
    {
        var (items, total) = await userService.GetUsersAsync(page, pageSize, search);
        return Ok(new { items, total, page, pageSize });
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(idClaim, out var userId))
            return Unauthorized();
        var user = await userService.GetByIdAsync(userId);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var user = await userService.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = await userService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateUserRequest request)
    {
        var user = await userService.UpdateAsync(id, request);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Deactivate(long id)
    {
        var result = await userService.DeactivateAsync(id);
        return result ? NoContent() : NotFound();
    }
}
