using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/close-period")]
[Authorize]
public class ClosePeriodController(KasabaKIATContext db) : ControllerBase
{
    // ── Holidays ──────────────────────────────────────────────────────────

    [HttpGet("holidays")]
    public async Task<IActionResult> GetHolidays([FromQuery] int? year, [FromQuery] short? dateType)
    {
        var query = db.TblHolidays.AsQueryable();
        if (year.HasValue)
            query = query.Where(h => h.SET_DATE != null && h.SET_DATE.Value.Year == year.Value);
        if (dateType.HasValue)
            query = query.Where(h => h.DATE_TYPE == dateType.Value);

        var items = await query.OrderBy(h => h.SET_DATE).ToListAsync();
        return Ok(items);
    }

    [HttpPost("holidays")]
    public async Task<IActionResult> CreateHoliday([FromBody] TblHoliday model)
    {
        model.REC_ID = 0;
        db.TblHolidays.Add(model);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetHolidays), new { year = model.SET_DATE?.Year }, model);
    }

    [HttpPut("holidays/{id}")]
    public async Task<IActionResult> UpdateHoliday(long id, [FromBody] TblHoliday model)
    {
        var existing = await db.TblHolidays.FindAsync(id);
        if (existing == null) return NotFound();
        existing.SET_DATE = model.SET_DATE;
        existing.DATE_TYPE = model.DATE_TYPE;
        existing.STATUS = model.STATUS;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("holidays/{id}")]
    public async Task<IActionResult> DeleteHoliday(long id)
    {
        var existing = await db.TblHolidays.FindAsync(id);
        if (existing == null) return NotFound();
        db.TblHolidays.Remove(existing);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("holidays/is-workday")]
    public async Task<IActionResult> IsWorkDay([FromQuery] DateTime date)
    {
        var dayOfWeek = date.DayOfWeek;
        var isWeekend = dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;

        var holiday = await db.TblHolidays
            .Where(h => h.SET_DATE == date.Date)
            .FirstOrDefaultAsync();

        bool isWorkDay;
        if (holiday != null)
            isWorkDay = holiday.DATE_TYPE == 2; // ChangedWeekEnd = workday
        else
            isWorkDay = !isWeekend;

        return Ok(new { date = date.Date, isWorkDay });
    }

    // ── Business Day (CloseDay) ───────────────────────────────────────────

    [HttpGet("business-day")]
    public async Task<IActionResult> GetBusinessDay([FromQuery] string? departmentCode)
    {
        var depBase = departmentCode ?? User.FindFirstValue("bkut_id");
        var row = await db.TblCloseDays
            .Where(d => d.DEPARTMENT_CODE == depBase)
            .FirstOrDefaultAsync();
        if (row == null) return NotFound();
        return Ok(row);
    }

    [HttpGet("business-day/all")]
    public async Task<IActionResult> GetAllBusinessDays()
    {
        var items = await db.TblCloseDays.ToListAsync();
        return Ok(items);
    }

    [HttpPost("business-day")]
    public async Task<IActionResult> CreateBusinessDay([FromBody] TblCloseDay model)
    {
        model.REC_ID = 0;
        db.TblCloseDays.Add(model);
        await db.SaveChangesAsync();
        return Ok(model);
    }

    [HttpPost("business-day/advance")]
    public async Task<IActionResult> AdvanceBusinessDay([FromQuery] string? departmentCode)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = departmentCode ?? User.FindFirstValue("bkut_id");

        var row = await db.TblCloseDays
            .Where(d => d.DEPARTMENT_CODE == depBase)
            .FirstOrDefaultAsync();

        if (row == null) return NotFound(new { message = "Business day row not found for department." });

        // Find next working day after row.NEXTDAY
        int counter = 1;
        DateTime nextWorkingDay = DateTime.MinValue;
        var searchFrom = row.NEXTDAY ?? DateTime.Today;

        while (counter <= 365)
        {
            var candidate = searchFrom.AddDays(counter);
            var holiday = await db.TblHolidays.Where(h => h.SET_DATE == candidate.Date).FirstOrDefaultAsync();

            bool isWork;
            if (holiday != null)
                isWork = holiday.DATE_TYPE == 2;
            else
                isWork = candidate.DayOfWeek != DayOfWeek.Saturday && candidate.DayOfWeek != DayOfWeek.Sunday;

            if (isWork) { nextWorkingDay = candidate; break; }
            counter++;
        }

        if (nextWorkingDay == DateTime.MinValue)
            return BadRequest(new { message = "Could not determine next working day." });

        var history = new TblCloseHistory
        {
            DEPARTMENT_CODE = depBase,
            CLOSED_DATE = row.CURDAY,
            REMARK = "CLOSE",
            USER_ID = userId
        };
        db.TblCloseHistories.Add(history);

        row.PREVDATE = row.CURDAY;
        row.CURDAY = row.NEXTDAY;
        row.NEXTDAY = nextWorkingDay;

        await db.SaveChangesAsync();
        return Ok(row);
    }

    [HttpGet("business-day/history")]
    public async Task<IActionResult> GetHistory([FromQuery] string? departmentCode, [FromQuery] int limit = 30)
    {
        var depBase = departmentCode ?? User.FindFirstValue("bkut_id");
        var items = await db.TblCloseHistories
            .Where(h => h.DEPARTMENT_CODE == depBase)
            .OrderByDescending(h => h.CLOSED_DATE)
            .Take(limit)
            .ToListAsync();
        return Ok(items);
    }

    // ── Quarter Close (CloseKvartal) ──────────────────────────────────────

    [HttpGet("kvartal")]
    public async Task<IActionResult> GetKvartalStatus([FromQuery] string? departmentCode)
    {
        var depBase = departmentCode ?? User.FindFirstValue("bkut_id");
        var row = await db.TblCloseKvartals
            .Where(k => k.DEPARTMENT_CODE == depBase)
            .FirstOrDefaultAsync();
        if (row == null) return NotFound();
        return Ok(row);
    }

    [HttpPost("kvartal")]
    public async Task<IActionResult> CreateKvartal([FromBody] TblCloseKvartal model)
    {
        model.REC_ID = 0;
        db.TblCloseKvartals.Add(model);
        await db.SaveChangesAsync();
        return Ok(model);
    }

    [HttpPost("kvartal/close")]
    public async Task<IActionResult> CloseKvartal([FromQuery] string? departmentCode)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = departmentCode ?? User.FindFirstValue("bkut_id");

        var row = await db.TblCloseKvartals
            .Where(k => k.DEPARTMENT_CODE == depBase)
            .FirstOrDefaultAsync();

        if (row == null) return NotFound();

        var (nextYear, nextKv) = GetNextQuarter(row.NEXTKV_YEAR ?? DateTime.Today.Year, row.NEXTKV ?? 1);

        var history = new TblCloseKvartalHistory
        {
            DEPARTMENT_CODE = depBase,
            CLOSED_KVYEAR = row.CURKV_YEAR,
            CLOSED_KV = row.CURKV,
            REMARK = "Close Kvartal"
        };
        db.TblCloseKvartalHistories.Add(history);

        row.PREVKV_YEAR = row.CURKV_YEAR;
        row.PREVKV = row.CURKV;
        row.CURKV_YEAR = row.NEXTKV_YEAR;
        row.CURKV = row.NEXTKV;
        row.NEXTKV_YEAR = nextYear;
        row.NEXTKV = nextKv;

        await db.SaveChangesAsync();
        return Ok(row);
    }

    [HttpPost("kvartal/cancel-close")]
    public async Task<IActionResult> CancelCloseKvartal([FromQuery] string? departmentCode)
    {
        var depBase = departmentCode ?? User.FindFirstValue("bkut_id");

        var row = await db.TblCloseKvartals
            .Where(k => k.DEPARTMENT_CODE == depBase)
            .FirstOrDefaultAsync();

        if (row == null) return NotFound();

        var (prevYear, prevKv) = GetPrevQuarter(row.PREVKV_YEAR ?? DateTime.Today.Year, row.PREVKV ?? 1);

        var history = new TblCloseKvartalHistory
        {
            DEPARTMENT_CODE = depBase,
            CLOSED_KVYEAR = row.NEXTKV_YEAR,
            CLOSED_KV = row.NEXTKV,
            REMARK = "Cancel Close Kvartal"
        };
        db.TblCloseKvartalHistories.Add(history);

        row.NEXTKV_YEAR = row.CURKV_YEAR;
        row.NEXTKV = row.CURKV;
        row.CURKV_YEAR = row.PREVKV_YEAR;
        row.CURKV = row.PREVKV;
        row.PREVKV_YEAR = prevYear;
        row.PREVKV = prevKv;

        await db.SaveChangesAsync();
        return Ok(row);
    }

    [HttpGet("kvartal/history")]
    public async Task<IActionResult> GetKvartalHistory([FromQuery] string? departmentCode)
    {
        var depBase = departmentCode ?? User.FindFirstValue("bkut_id");
        var items = await db.TblCloseKvartalHistories
            .Where(h => h.DEPARTMENT_CODE == depBase)
            .OrderByDescending(h => h.CLOSED_KVYEAR).ThenByDescending(h => h.CLOSED_KV)
            .ToListAsync();
        return Ok(items);
    }

    private static (int year, int kv) GetNextQuarter(int year, int kv)
        => kv == 4 ? (year + 1, 1) : (year, kv + 1);

    private static (int year, int kv) GetPrevQuarter(int year, int kv)
        => kv == 1 ? (year - 1, 4) : (year, kv - 1);
}
