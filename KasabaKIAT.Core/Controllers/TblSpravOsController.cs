using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/tbl-sprav-os")]
[Authorize]
public class TblSpravOsController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await db.TblSpravOses.ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.TblSpravOses.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TblSpravOs item)
    {
        db.TblSpravOses.Add(item);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = item.ID }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] TblSpravOs item)
    {
        if (id != item.ID) return BadRequest();
        db.Entry(item).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.TblSpravOses.FindAsync(id);
        if (item == null) return NotFound();
        db.TblSpravOses.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
