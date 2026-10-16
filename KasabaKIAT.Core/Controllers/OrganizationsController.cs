using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/organizations-v1")]
[Authorize]
public class OrganizationsController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] long? otrasId)
    {
        var query = db.TblSpravOrgs.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(o => o.Name_Org!.Contains(search) || o.INN!.Contains(search));
        if (otrasId.HasValue)
            query = query.Where(o => o.ID_Ot == otrasId);

        var items = await query
            .OrderBy(o => o.Name_Org)
            .Select(o => new
            {
                o.ID,
                o.Name_Org,
                o.INN,
                o.OWNER_CODE,
                o.ID_Ot,
                o.DepartmentBase,
                o.Adress,
                o.RS,
                o.MFO
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.TblSpravOrgs.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpGet("by-inn/{inn}")]
    public async Task<IActionResult> GetByInn(string inn)
    {
        var item = await db.TblSpravOrgs
            .Where(o => o.INN == inn)
            .FirstOrDefaultAsync();
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] TblSpravOrg org)
    {
        db.TblSpravOrgs.Add(org);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = org.ID }, org);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(long id, [FromBody] TblSpravOrg org)
    {
        if (id != org.ID) return BadRequest();
        db.Entry(org).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.TblSpravOrgs.FindAsync(id);
        if (item == null) return NotFound();
        db.TblSpravOrgs.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
