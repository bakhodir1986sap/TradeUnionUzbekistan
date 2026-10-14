using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

/// <summary>
/// Voucher receipts (prixod putevok) — manages incoming vouchers tblPos_Put
/// and links to tbpt_putyovki distribution.
/// </summary>
[ApiController]
[Route("api/prixod-putevok")]
[Authorize]
public class PrixodPutevokController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] long? sanId, [FromQuery] string? status,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TblPosPuts.AsQueryable();
        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DEPARTMENT_BASE == depBase);
        if (sanId.HasValue)
            query = query.Where(p => p.ID_San == sanId);
        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(p => p.MODIFY_DATE)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new
            {
                p.ID,
                p.ID_San,
                p.Num_Put,
                p.DataNach,
                p.DataKon,
                p.Price,
                p.Mes,
                p.Tip,
                p.Status,
                p.UserId,
                p.DEPARTMENT_BASE,
                p.MODIFY_DATE
            })
            .ToListAsync();

        return Ok(new { total, page, pageSize, items });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.TblPosPuts.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpGet("by-sanatorium/{sanId}")]
    public async Task<IActionResult> GetBySanatorium(long sanId, [FromQuery] string? status)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var query = db.TblPosPuts.Where(p => p.ID_San == sanId);
        if (!string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DEPARTMENT_BASE == depBase);
        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);

        var items = await query
            .OrderBy(p => p.DataNach)
            .ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TblPosPut posPut)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        posPut.UserId = userId;
        posPut.DEPARTMENT_BASE ??= depBase;
        posPut.MODIFY_DATE = DateTime.Now;
        posPut.Status ??= "NEW";

        db.TblPosPuts.Add(posPut);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = posPut.ID }, posPut);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] TblPosPut posPut)
    {
        if (id != posPut.ID) return BadRequest();
        posPut.MODIFY_DATE = DateTime.Now;
        db.Entry(posPut).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.TblPosPuts.FindAsync(id);
        if (item == null) return NotFound();
        db.TblPosPuts.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Get list of putyovki (vouchers) available for distribution — from tbpt_putyovki
    /// </summary>
    [HttpGet("available-putyovki")]
    public async Task<IActionResult> GetAvailablePutyovki([FromQuery] long? sanId, [FromQuery] string? year)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TbptPutyovkis.AsQueryable();
        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DEPARTMENT_CODE == depBase);
        if (sanId.HasValue)
            query = query.Where(p => p.SANATORIUM_ID == sanId);
        if (!string.IsNullOrEmpty(year) && int.TryParse(year, out int yearInt))
            query = query.Where(p => p.REST_BEGIN_DATE != null && p.REST_BEGIN_DATE.Value.Year == yearInt);

        var items = await query
            .Select(p => new
            {
                p.REC_ID,
                p.DEPARTMENT_CODE,
                p.SANATORIUM_ID,
                p.SERIAL_BEGIN,
                p.SERIAL_END,
                p.REST_BEGIN_DATE,
                p.REST_END_DATE,
                p.PRICE,
                p.STATUS,
                p.QUANTITY_IN,
                p.REMAIN_BAL,
                p.QUANTITY_REMAIN
            })
            .ToListAsync();

        return Ok(items);
    }
}
