using KasabaKIAT.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/user-actions")]
[Authorize]
public class UserActionsController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var query = db.UserActions
            .OrderByDescending(u => u.id);

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new
            {
                u.id,
                u.user_id,
                u.action_name,
                u.action_date,
                u.description
            })
            .ToListAsync();

        return Ok(new { total, page, pageSize, items });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.UserActions.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserActionRequest req)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var action = new Models.UserAction
        {
            user_id = userId,
            action_name = req.Action,
            description = req.Description,
            action_date = DateTime.UtcNow
        };

        db.UserActions.Add(action);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = action.id }, action);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.UserActions.FindAsync(id);
        if (item == null) return NotFound();
        db.UserActions.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}

public class CreateUserActionRequest
{
    public string? Action { get; set; }
    public string? Description { get; set; }
}
