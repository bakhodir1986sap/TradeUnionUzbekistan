using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs.Reference;
using KasabaKIAT.Core.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/sp-branch")]
[Authorize]
public class SpBranchController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = "reference")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var items = await db.SpBranches.ProjectToType<SpBranchDto>().ToListAsync(ct);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var item = await db.SpBranches.FindAsync([id], ct);
        if (item == null) return NotFound();
        return Ok(item.Adapt<SpBranchDto>());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SpBranch item, CancellationToken ct)
    {
        db.SpBranches.Add(item);
        await db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = item.id }, item.Adapt<SpBranchDto>());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] SpBranch item, CancellationToken ct)
    {
        if (id != item.id) return BadRequest();
        db.Entry(item).State = EntityState.Modified;
        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var item = await db.SpBranches.FindAsync([id], ct);
        if (item == null) return NotFound();
        db.SpBranches.Remove(item);
        await db.SaveChangesAsync(ct);
        return NoContent();
    }
}
