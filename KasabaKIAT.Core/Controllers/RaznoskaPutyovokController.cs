using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

/// <summary>
/// Raznoska po putyovkam — voucher distribution by person/med.
/// Ported from BusinessLogic/Putyovki/RaznoskaPoPutyovki/*
/// </summary>
[ApiController]
[Route("api/raznoska-putyovok")]
[Authorize]
public class RaznoskaPutyovokController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? year, [FromQuery] int? sanId, [FromQuery] bool? isMed)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TbptRaznoskaPutyovkis.AsQueryable();

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(r => r.DEPARTMENT_BASE == depBase);
        if (sanId.HasValue) query = query.Where(r => r.ID_SANAT == sanId);
        if (isMed.HasValue) query = query.Where(r => r.IS_MED == isMed);
        if (year.HasValue)
            query = query.Where(r => r.TIMESTAMP_T != null && r.TIMESTAMP_T.Value.Year == year);

        var items = await query
            .Select(r => new
            {
                r.ID,
                r.DEPARTMENT_BASE,
                r.FACE_ID,
                FaceName = db.TblSpravFaces
                    .Where(f => f.ID == r.FACE_ID)
                    .Select(f => f.Name_Face).FirstOrDefault(),
                r.PUT_ID,
                r.ID_SANAT,
                SanatName = db.TblSpravSans
                    .Where(s => s.ID == r.ID_SANAT)
                    .Select(s => s.Name_San).FirstOrDefault(),
                r.NUM_PUT,
                r.PRICE,
                r.DATE_ZAEZD,
                r.IS_MED,
                r.TIMESTAMP_T
            })
            .OrderBy(r => r.NUM_PUT)
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TbptRaznoskaPutyovki model)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        model.ID = 0;
        model.DEPARTMENT_BASE ??= depBase;
        model.USER_ID = userId;
        model.TIMESTAMP_T = DateTime.Now;

        db.TbptRaznoskaPutyovkis.Add(model);
        await db.SaveChangesAsync();
        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.TbptRaznoskaPutyovkis.FindAsync(id);
        if (item == null) return NotFound();
        db.TbptRaznoskaPutyovkis.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("general-stats")]
    public async Task<IActionResult> GetGeneralStats([FromQuery] int year, [FromQuery] int sanId)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TbptRaznoskaPutyovkis
            .Where(r => r.ID_SANAT == sanId
                     && r.TIMESTAMP_T != null
                     && r.TIMESTAMP_T.Value.Year == year);

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(r => r.DEPARTMENT_BASE == depBase);

        var total = await query.CountAsync();
        var medCount = await query.CountAsync(r => r.IS_MED);
        var totalSumma = await query.SumAsync(r => r.PRICE ?? 0);

        return Ok(new { year, sanId, total, medCount, nonMedCount = total - medCount, totalSumma });
    }
}
