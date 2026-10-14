using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/sp-department-type")]
[Authorize]
public class SpDepartmentTypeController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await db.SpDepartmentTypes.ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.SpDepartmentTypes.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SpDepartmentType item)
    {
        db.SpDepartmentTypes.Add(item);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = item.id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] SpDepartmentType item)
    {
        if (id != item.id) return BadRequest();
        db.Entry(item).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.SpDepartmentTypes.FindAsync(id);
        if (item == null) return NotFound();
        db.SpDepartmentTypes.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
