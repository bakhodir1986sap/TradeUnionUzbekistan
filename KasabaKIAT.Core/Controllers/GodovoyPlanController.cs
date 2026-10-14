using KasabaKIAT.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/godovoy-plan")]
[Authorize]
public class GodovoyPlanController(KasabaKIATContext db) : ControllerBase
{
    // GET /api/godovoy-plan/sanatoriums — list sanatoriums from tblSprav_San
    [HttpGet("sanatoriums")]
    public async Task<IActionResult> GetSanatoriums()
    {
        var items = await db.TblSpravSans
            .Select(s => new { id = s.ID, name = s.Name_San })
            .ToListAsync();
        return Ok(items);
    }

    // GET /api/godovoy-plan/camps — list camps from t_camp
    [HttpGet("camps")]
    public async Task<IActionResult> GetCamps()
    {
        var items = await db.TCamps
            .Select(c => new { id = c.id, name = c.camps_name })
            .ToListAsync();
        return Ok(items);
    }

    // GET /api/godovoy-plan/branches — list branches/otrasl from sp_branch
    [HttpGet("branches")]
    public async Task<IActionResult> GetBranches()
    {
        var items = await db.SpBranches
            .Select(b => new { id = b.id, name = b.name_uz })
            .ToListAsync();
        return Ok(items);
    }

    // GET /api/godovoy-plan — list putyovki with filters
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] long? sanatoriumId = null,
        [FromQuery] long? campId = null,
        [FromQuery] int? year = null,
        [FromQuery] string? kvartal = null)
    {
        var query = db.TbptPutyovkis.Where(p => p.STATUS == "1");
        if (sanatoriumId.HasValue) query = query.Where(p => p.SANATORIUM_ID == sanatoriumId);
        if (campId.HasValue) query = query.Where(p => p.CAMP_ID == campId);
        if (year.HasValue) query = query.Where(p => p.SMFIL == year);

        var items = await query
            .Select(p => new
            {
                id = p.REC_ID,
                sanatoriumId = p.SANATORIUM_ID,
                campId = p.CAMP_ID,
                year = p.SMFIL,
                serialBegin = p.SERIAL_BEGIN,
                serialEnd = p.SERIAL_END,
                restBeginDate = p.REST_BEGIN_DATE,
                restEndDate = p.REST_END_DATE,
                price = p.PRICE,
                quantityIn = p.QUANTITY_IN,
                quantityOut = p.QUANTITY_OUT,
                quantityRemain = p.QUANTITY_REMAIN,
                remainBal = p.REMAIN_BAL,
                status = p.STATUS,
                modifyDate = p.MODIFY_DATE
            })
            .ToListAsync();
        return Ok(items);
    }

    // POST /api/godovoy-plan — create a yearly plan entry
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GodovoyPlanRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(userIdClaim, out var userId)) return Unauthorized();

        var entry = new KasabaKIAT.Core.Models.TbptPutyovki
        {
            SANATORIUM_ID = (int?)request.SanatoriumId,
            SERIAL_END = request.SerialEnd,
            REST_BEGIN_DATE = request.RestBeginDate,
            REST_END_DATE = request.RestEndDate,
            PRICE = request.Price,
            STATUS = "1",
            MODIFY_DATE = DateTime.Now,
            QUANTITY_IN = request.QuantityIn,
            QUANTITY_OUT = 0,
            QUANTITY_REMAIN = request.QuantityIn,
            REMAIN_BAL = request.QuantityIn * (request.Price ?? 0),
            SMFIL = request.Year ?? DateTime.Now.Year
        };

        db.TbptPutyovkis.Add(entry);
        await db.SaveChangesAsync();
        return Ok(new { id = entry.REC_ID });
    }

    // PUT /api/godovoy-plan/{id}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] GodovoyPlanRequest request)
    {
        var entry = await db.TbptPutyovkis.FindAsync(id);
        if (entry == null) return NotFound();

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        long.TryParse(userIdClaim, out var userId);

        entry.SANATORIUM_ID = (int?)request.SanatoriumId;
        entry.CAMP_ID = request.CampId;
        entry.USER_ID = userId;
        entry.SERIAL_BEGIN = request.SerialBegin;
        entry.SERIAL_END = request.SerialEnd;
        entry.REST_BEGIN_DATE = request.RestBeginDate;
        entry.REST_END_DATE = request.RestEndDate;
        entry.PRICE = request.Price;
        entry.MODIFY_DATE = DateTime.Now;
        entry.QUANTITY_IN = request.QuantityIn;
        entry.QUANTITY_REMAIN = request.QuantityIn - (entry.QUANTITY_OUT ?? 0);
        entry.REMAIN_BAL = entry.QUANTITY_REMAIN * (request.Price ?? 0);
        entry.SMFIL = request.Year ?? entry.SMFIL;

        await db.SaveChangesAsync();
        return Ok(new { id = entry.REC_ID });
    }

    // DELETE /api/godovoy-plan/{id} — soft delete (STATUS="0")
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var entry = await db.TbptPutyovkis.FindAsync(id);
        if (entry == null) return NotFound();
        entry.STATUS = "0";
        await db.SaveChangesAsync();
        return NoContent();
    }
}

public record GodovoyPlanRequest(
    long? SanatoriumId,
    long? CampId,
    string? SerialBegin,
    string? SerialEnd,
    DateTime? RestBeginDate,
    DateTime? RestEndDate,
    decimal? Price,
    int? QuantityIn,
    int? Year
);
