using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/brons")]
[Authorize]
public class BronsController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] long? sanId, [FromQuery] string? depBase)
    {
        var query = db.TblBrons.AsQueryable();
        if (sanId.HasValue) query = query.Where(b => b.ID_Sanat == sanId);
        if (!string.IsNullOrEmpty(depBase)) query = query.Where(b => b.DEPARTMENT_BASE == depBase);

        var items = await query
            .OrderByDescending(b => b.BRON_DATE)
            .Select(b => new
            {
                b.ID,
                b.ID_Sanat,
                b.Num_Put,
                b.BRON_FLG,
                b.BRON_DATE,
                b.EXPIRE_DATE,
                b.USER_ID,
                b.DEPARTMENT_BASE,
                b.PRIM
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.TblBrons.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TblBron bron)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        bron.USER_ID = userId;
        bron.DEPARTMENT_BASE = depBase;
        bron.BRON_DATE = DateTime.Now;

        db.TblBrons.Add(bron);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = bron.ID }, bron);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] TblBron bron)
    {
        if (id != bron.ID) return BadRequest();
        db.Entry(bron).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.TblBrons.FindAsync(id);
        if (item == null) return NotFound();
        db.TblBrons.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
