using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/fin-otchet-setting")]
[Authorize(Roles = "admin")]
public class FinOtchetSettingController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? depBase)
    {
        var query = db.TbbuFinOtchetSettings.AsQueryable();
        if (!string.IsNullOrEmpty(depBase))
            query = query.Where(s => s.DEPARTMENT_BASE == depBase);

        var items = await query
            .OrderBy(s => s.OTCHET_PART).ThenBy(s => s.CODE)
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.TbbuFinOtchetSettings.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TbbuFinOtchetSetting setting)
    {
        var depBase = User.FindFirstValue("bkut_id");
        setting.DEPARTMENT_BASE ??= depBase;

        db.TbbuFinOtchetSettings.Add(setting);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = setting.REC_ID }, setting);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] TbbuFinOtchetSetting setting)
    {
        if (id != setting.REC_ID) return BadRequest();
        db.Entry(setting).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.TbbuFinOtchetSettings.FindAsync(id);
        if (item == null) return NotFound();
        db.TbbuFinOtchetSettings.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
