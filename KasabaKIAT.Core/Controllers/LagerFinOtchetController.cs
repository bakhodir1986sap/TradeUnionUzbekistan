using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

/// <summary>
/// Camp (lager) financial report.
/// Ported from BusinessLogic/Buxgalteriya/LagerFinOtchet/*
/// </summary>
[ApiController]
[Route("api/lager-fin-otchet")]
[Authorize]
public class LagerFinOtchetController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? year, [FromQuery] int? kv, [FromQuery] int? campId)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TbptLagerOtchets.AsQueryable();

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(r => r.DEPARTMENT_BASE == depBase);
        if (year.HasValue)   query = query.Where(r => r.YEAR == year);
        if (kv.HasValue)     query = query.Where(r => r.KVARTAL == kv);
        if (campId.HasValue) query = query.Where(r => r.CAMP_ID == campId);

        var items = await query
            .Select(r => new
            {
                r.ID,
                r.DEPARTMENT_BASE,
                r.CAMP_ID,
                CampName = db.TCamps
                    .Where(c => c.id == r.CAMP_ID)
                    .Select(c => c.camps_name).FirstOrDefault(),
                r.YEAR,
                r.KVARTAL,
                r.CHILDREN_COUNT,
                r.SUMMA,
                r.IS_CLOSED,
                r.CREATE_DATE
            })
            .OrderByDescending(r => r.YEAR).ThenByDescending(r => r.KVARTAL)
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.TbptLagerOtchets.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TbptLagerOtchet model)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        model.ID = 0;
        model.DEPARTMENT_BASE ??= depBase;
        model.USER_ID = userId;
        model.CREATE_DATE = DateTime.Now;
        model.IS_CLOSED = false;

        db.TbptLagerOtchets.Add(model);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.ID }, model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] TbptLagerOtchet model)
    {
        if (id != model.ID) return BadRequest();
        var existing = await db.TbptLagerOtchets.FindAsync(id);
        if (existing == null) return NotFound();
        if (existing.IS_CLOSED) return BadRequest(new { message = "Отчёт уже закрыт." });

        existing.CAMP_ID = model.CAMP_ID;
        existing.YEAR = model.YEAR;
        existing.KVARTAL = model.KVARTAL;
        existing.CHILDREN_COUNT = model.CHILDREN_COUNT;
        existing.SUMMA = model.SUMMA;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/close")]
    public async Task<IActionResult> Close(long id)
    {
        var item = await db.TbptLagerOtchets.FindAsync(id);
        if (item == null) return NotFound();
        if (item.IS_CLOSED) return BadRequest(new { message = "Уже закрыт." });

        item.IS_CLOSED = true;
        await db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.TbptLagerOtchets.FindAsync(id);
        if (item == null) return NotFound();
        if (item.IS_CLOSED) return BadRequest(new { message = "Закрытый отчёт нельзя удалить." });
        db.TbptLagerOtchets.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
