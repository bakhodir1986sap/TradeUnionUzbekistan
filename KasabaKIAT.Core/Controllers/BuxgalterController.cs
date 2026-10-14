using KasabaKIAT.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

/// <summary>
/// Accounting module: payment orders, provodki journal, glavnaya kniga, financial reports.
/// Complex business logic from old BuxgalterController is simplified to data CRUD + SP calls.
/// </summary>
[ApiController]
[Route("api/buxgalter")]
[Authorize]
public class BuxgalterController(KasabaKIATContext db) : ControllerBase
{
    // ---- Payment orders (platejki) ----

    [HttpGet("platejki")]
    public async Task<IActionResult> GetPlatejki()
    {
        var depBase = User.FindFirstValue("bkut_id");
        var items = await db.TbbuPlatejkas
            .Where(p => p.DEPARTMENT_CODE == depBase)
            .OrderByDescending(p => p.PLAT_DATE)
            .ToListAsync();
        return Ok(items);
    }

    [HttpPost("platejki")]
    public async Task<IActionResult> CreatePlatejka([FromBody] Models.TbbuPlatejka platejka)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        platejka.USR_ID = userId;
        platejka.DEPARTMENT_CODE = depBase;
        platejka.MODIFY_DATE = DateTime.Now;

        db.TbbuPlatejkas.Add(platejka);
        await db.SaveChangesAsync();
        return Ok(new { id = platejka.REC_ID });
    }

    [HttpPut("platejki/{id}")]
    public async Task<IActionResult> UpdatePlatejka(long id, [FromBody] Models.TbbuPlatejka platejka)
    {
        if (id != platejka.REC_ID) return BadRequest();
        platejka.MODIFY_DATE = DateTime.Now;
        db.Entry(platejka).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("platejki/{id}")]
    public async Task<IActionResult> DeletePlatejka(long id)
    {
        var item = await db.TbbuPlatejkas.FindAsync(id);
        if (item == null) return NotFound();
        db.TbbuPlatejkas.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ---- Average price settings ----

    [HttpGet("avarage-price")]
    public async Task<IActionResult> GetAvaragePrice()
    {
        var items = await db.TbptAvaragePrices
            .OrderByDescending(p => p.SET_DATE)
            .ToListAsync();
        return Ok(items);
    }

    [HttpPost("avarage-price")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> SetAvaragePrice([FromBody] Models.TbptAvaragePrice price)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        price.USER_ID = userId;
        price.SET_DATE ??= DateTime.Now;
        db.TbptAvaragePrices.Add(price);
        await db.SaveChangesAsync();
        return Ok(new { id = price.REC_ID });
    }

    // ---- Financial report settings ----

    [HttpGet("fin-otchet-settings")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetFinOtchetSettings([FromQuery] string? depBase)
    {
        var query = db.TbbuFinOtchetSettings.AsQueryable();
        if (!string.IsNullOrEmpty(depBase))
            query = query.Where(s => s.DEPARTMENT_BASE == depBase);
        var items = await query.OrderBy(s => s.OTCHET_PART).ThenBy(s => s.CODE).ToListAsync();
        return Ok(items);
    }

    // ---- Yearly plan summaries (read-only aggregates) ----

    [HttpGet("yearly-plan-summary")]
    public async Task<IActionResult> GetYearlyPlanSummary([FromQuery] string? year, [FromQuery] string? depBase)
    {
        var userDepBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var queryOtrasl = db.TbptYearlyPlanOtrasls.AsQueryable();
        if (!isSuperAdmin)
            queryOtrasl = queryOtrasl.Where(p => p.DEPARTMANT_BASE == userDepBase);
        if (!string.IsNullOrEmpty(year))
            queryOtrasl = queryOtrasl.Where(p => p.YEAR == year);

        var data = await queryOtrasl
            .GroupBy(p => new { p.YEAR, p.KVARTAL, p.SANATORIUM_ID })
            .Select(g => new
            {
                year = g.Key.YEAR,
                kvartal = g.Key.KVARTAL,
                sanatoriumId = g.Key.SANATORIUM_ID,
                totalQuantity = g.Sum(p => p.QUANTITY),
                totalPrice = g.Sum(p => p.PRICE)
            })
            .ToListAsync();

        return Ok(data);
    }
}
