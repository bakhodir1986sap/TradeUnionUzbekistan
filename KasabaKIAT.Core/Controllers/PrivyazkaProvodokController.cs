using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/privyazka-provodok")]
[Authorize]
public class PrivyazkaProvodokController(KasabaKIATContext db) : ControllerBase
{
    // GET /api/privyazka-provodok?orderId=1&departmentBase=17
    // Returns all provodki with a flag indicating whether a binding exists for the given order+department
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int orderId, [FromQuery] string departmentBase)
    {
        var provodki = await db.Provodoks.ToListAsync();
        var bindings = await db.PProvodoks
            .Where(p => p.order == orderId && p.DEPARTMENT_BASE == departmentBase)
            .ToListAsync();

        var result = provodki.Select(p => new
        {
            p.id,
            p.description,
            scoreBegin = p.score_begin,
            scoreEnd = p.score_end,
            privExists = bindings.Any(b => b.score_begin == p.score_begin && b.score_end == p.score_end)
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await db.PProvodoks.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PProvodok item)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        long.TryParse(userIdClaim, out var userId);
        item.USER_ID = userId;

        // REGION_ID derivation matches old project logic
        item.DEPARTMENT_BASE = item.REGION_ID;
        if (item.REGION_ID != null && item.REGION_ID != "17" && item.REGION_ID.Length > 2)
            item.REGION_ID = item.REGION_ID.Remove(2, 1);

        db.PProvodoks.Add(item);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = item.id }, item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.PProvodoks.FindAsync(id);
        if (item == null) return NotFound();
        db.PProvodoks.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
