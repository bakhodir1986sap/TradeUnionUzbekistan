using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

/// <summary>
/// Bank payments (prixod chastichka / bank oplati).
/// Ported from BusinessLogic/BankOplati/* and BusinessLogic/Buxgalteriya/PrixodChastichka/*
/// </summary>
[ApiController]
[Route("api/bank-oplati")]
[Authorize]
public class BankOplatiController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? year, [FromQuery] int? kv,
        [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TbbuBankPayments.AsQueryable();

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DEPARTMENT_BASE == depBase);
        if (year.HasValue) query = query.Where(p => p.CUR_YEAR == year);
        if (kv.HasValue)   query = query.Where(p => p.CUR_KV == kv);
        if (dateFrom.HasValue) query = query.Where(p => p.DOC_DATE >= dateFrom.Value.Date);
        if (dateTo.HasValue)   query = query.Where(p => p.DOC_DATE <= dateTo.Value.Date.AddDays(1).AddSeconds(-1));

        var items = await query.OrderByDescending(p => p.DOC_DATE).ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.TbbuBankPayments.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TbbuBankPayment model)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        model.ID = 0;
        model.DEPARTMENT_BASE ??= depBase;
        model.USER_ID = userId;

        db.TbbuBankPayments.Add(model);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = model.ID }, model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] TbbuBankPayment model)
    {
        if (id != model.ID) return BadRequest();
        db.Entry(model).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.TbbuBankPayments.FindAsync(id);
        if (item == null) return NotFound();
        db.TbbuBankPayments.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("totals")]
    public async Task<IActionResult> GetTotals([FromQuery] int year, [FromQuery] int kv)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TbbuBankPayments
            .Where(p => p.CUR_YEAR == year && p.CUR_KV == kv);

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DEPARTMENT_BASE == depBase);

        var count = await query.CountAsync();
        var summa = await query.SumAsync(p => p.SUMMA ?? 0);

        return Ok(new { year, kv, count, summa });
    }
}
