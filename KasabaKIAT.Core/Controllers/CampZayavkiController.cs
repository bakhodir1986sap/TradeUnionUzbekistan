using KasabaKIAT.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/camp-zayavki")]
[Authorize]
public class CampZayavkiController(KasabaKIATContext db) : ControllerBase
{
    // GET /api/camp-zayavki?status=1
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] string? status = null)
    {
        var query = db.TbptPutyovkis
            .Where(p => p.CAMP_ID != null);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.STATUS == status);

        var items = await query
            .Join(db.TCamps, p => p.CAMP_ID, c => c.id, (p, c) => new
            {
                id = p.REC_ID,
                campId = p.CAMP_ID,
                campName = c.camps_name,
                status = p.STATUS,
                serialBegin = p.SERIAL_BEGIN,
                serialEnd = p.SERIAL_END,
                restBeginDate = p.REST_BEGIN_DATE,
                restEndDate = p.REST_END_DATE,
                price = p.PRICE,
                quantityIn = p.QUANTITY_IN,
                quantityRemain = p.QUANTITY_REMAIN,
                modifyDate = p.MODIFY_DATE
            })
            .ToListAsync();
        return Ok(items);
    }

    // GET /api/camp-zayavki/{id}
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.TbptPutyovkis
            .Where(p => p.REC_ID == id && p.CAMP_ID != null)
            .Join(db.TCamps, p => p.CAMP_ID, c => c.id, (p, c) => new
            {
                id = p.REC_ID,
                campId = p.CAMP_ID,
                campName = c.camps_name,
                status = p.STATUS,
                serialBegin = p.SERIAL_BEGIN,
                serialEnd = p.SERIAL_END,
                restBeginDate = p.REST_BEGIN_DATE,
                restEndDate = p.REST_END_DATE,
                price = p.PRICE,
                quantityIn = p.QUANTITY_IN,
                quantityRemain = p.QUANTITY_REMAIN,
                modifyDate = p.MODIFY_DATE
            })
            .FirstOrDefaultAsync();
        return item == null ? NotFound() : Ok(item);
    }

    // POST /api/camp-zayavki
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CampZayavkaRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        long.TryParse(userIdClaim, out var userId);

        var deptCode = User.FindFirstValue("bkut_id") ?? "0";

        var entry = new KasabaKIAT.Core.Models.TbptPutyovki
        {
            CAMP_ID = request.CampId,
            USER_ID = userId,
            DEPARTMENT_CODE = deptCode,
            SERIAL_BEGIN = request.SerialBegin,
            SERIAL_END = request.SerialEnd,
            REST_BEGIN_DATE = request.RestBeginDate,
            REST_END_DATE = request.RestEndDate,
            PRICE = request.Price,
            STATUS = "1",
            MODIFY_DATE = DateTime.Now,
            QUANTITY_IN = request.Quantity ?? 0,
            QUANTITY_OUT = 0,
            QUANTITY_REMAIN = request.Quantity ?? 0,
            REMAIN_BAL = (request.Quantity ?? 0) * (request.Price ?? 0)
        };
        db.TbptPutyovkis.Add(entry);
        await db.SaveChangesAsync();
        return Ok(new { id = entry.REC_ID });
    }

    // PUT /api/camp-zayavki/{id}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] CampZayavkaRequest request)
    {
        var entry = await db.TbptPutyovkis.FindAsync(id);
        if (entry == null) return NotFound();

        entry.CAMP_ID = request.CampId;
        entry.SERIAL_BEGIN = request.SerialBegin;
        entry.SERIAL_END = request.SerialEnd;
        entry.REST_BEGIN_DATE = request.RestBeginDate;
        entry.REST_END_DATE = request.RestEndDate;
        entry.PRICE = request.Price;
        entry.MODIFY_DATE = DateTime.Now;
        entry.QUANTITY_IN = request.Quantity ?? entry.QUANTITY_IN;
        entry.QUANTITY_REMAIN = (request.Quantity ?? entry.QUANTITY_IN) - (entry.QUANTITY_OUT ?? 0);
        entry.REMAIN_BAL = entry.QUANTITY_REMAIN * (request.Price ?? entry.PRICE ?? 0);

        await db.SaveChangesAsync();
        return Ok(new { id = entry.REC_ID });
    }

    // PATCH /api/camp-zayavki/{id}/status — change status
    [HttpPatch("{id:long}/status")]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateStatusRequest request)
    {
        var entry = await db.TbptPutyovkis.FindAsync(id);
        if (entry == null) return NotFound();
        entry.STATUS = request.Status;
        await db.SaveChangesAsync();
        return Ok(new { id = entry.REC_ID, status = entry.STATUS });
    }

    // DELETE /api/camp-zayavki/{id}
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

public record CampZayavkaRequest(
    long? CampId,
    string? SerialBegin,
    string? SerialEnd,
    DateTime? RestBeginDate,
    DateTime? RestEndDate,
    decimal? Price,
    int? Quantity
);

public record UpdateStatusRequest(string Status);
