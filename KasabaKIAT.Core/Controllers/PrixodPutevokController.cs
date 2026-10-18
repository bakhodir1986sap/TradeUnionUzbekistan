using ClosedXML.Excel;
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

    // ── GET /api/prixod-putevok/ku ─────────────────────────────────────────
    // Returns KU invoice headers (tblPost) with optional filters.
    // zbId = profsoyuz/org ID, dateFrom/dateTo filter by Data_Post.
    [HttpGet("ku")]
    public async Task<IActionResult> GetKuList(
        [FromQuery] long? zbId,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TblPosts.AsQueryable();

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DepartmentBase == depBase);
        if (zbId.HasValue)
            query = query.Where(p => p.ID_Pos == zbId);
        if (dateFrom.HasValue)
            query = query.Where(p => p.Data_Post >= dateFrom.Value.Date);
        if (dateTo.HasValue)
            query = query.Where(p => p.Data_Post <= dateTo.Value.Date.AddDays(1).AddSeconds(-1));

        var items = await query
            .OrderByDescending(p => p.Data_Post)
            .Select(p => new
            {
                p.ID,
                numDoc = p.Num_Doc,
                dataDoc = p.Data_Post,
                tipVidachi = p.TipVidachi,
                dogovor = p.Dogovor,
                zaKvartal = p.ID_KV,
                dovNum = p.DovNum,
                dovDate = p.DovDate,
                komVidan = p.Name_Dov,
                kolPutevok = p.Num_Kol,
                summa = p.Symma,
                vid = p.Vid,
                departmentBase = p.DepartmentBase,
                beginNumber = p.BeginNumber,
                endNumber = p.EndNumber,
                orgName = db.TblSpravOrgs
                    .Where(o => o.ID == p.ID_Pos)
                    .Select(o => o.Name_Org)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(items);
    }

    // ── GET /api/prixod-putevok/ku/{id}/details ───────────────────────────
    [HttpGet("ku/{id}/details")]
    public async Task<IActionResult> GetKuDetails(long id)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var header = await db.TblPosts.FindAsync(id);
        if (header == null) return NotFound();
        if (!isSuperAdmin && header.DepartmentBase != depBase) return Forbid();

        var details = await db.TblPosPuts
            .Where(d => d.ID_Post == id)
            .OrderBy(d => d.Num_Put)
            .Select(d => new
            {
                d.ID,
                sanat = db.TblSpravSans
                    .Where(s => s.ID == d.ID_Sanat)
                    .Select(s => s.Name_San)
                    .FirstOrDefault(),
                numPut = d.Num_Put,
                tip = d.Tip,
                dataNach = d.DataNach,
                dataKon = d.DataKon,
                stoimost = d.Price,
                summa = d.Symma_Put,
                status = d.ID_PolVoz == true ? "Реализованная" :
                    db.TblBrons.Any(b => b.Num_Put == d.Num_Put && b.ID_Sanat == d.ID_Sanat && b.BRON_FLG == true)
                        ? "Забронированная" : "Свободная"
            })
            .ToListAsync();

        return Ok(new { header, details });
    }

    // ── POST /api/prixod-putevok/ku/{id}/oprixodovat ──────────────────────
    // Marks the KU invoice as posted (Vid = true), recalculates totals.
    [HttpPost("ku/{id}/oprixodovat")]
    public async Task<IActionResult> Oprixodovat(long id)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var post = await db.TblPosts.FindAsync(id);
        if (post == null) return NotFound();
        if (!isSuperAdmin && post.DepartmentBase != depBase) return Forbid();
        if (post.Vid) return BadRequest(new { message = "Документ уже оприходован." });

        var totals = await db.TblPosPuts
            .Where(d => d.ID_Post == id)
            .GroupBy(_ => 1)
            .Select(g => new { Count = g.Count(), Summa = g.Sum(d => d.Symma_Put ?? 0) })
            .FirstOrDefaultAsync();

        post.Num_Kol = totals?.Count ?? 0;
        post.Symma = totals?.Summa ?? 0;
        post.Vid = true;

        db.Entry(post).State = EntityState.Modified;
        await db.SaveChangesAsync();

        return Ok(new { message = "Оприходовано успешно.", post.ID, post.Num_Kol, post.Symma });
    }

    // ── GET /api/prixod-putevok/ku/export ─────────────────────────────────
    // Returns an xlsx of KU invoices matching the same filters as GetKuList.
    [HttpGet("ku/export")]
    public async Task<IActionResult> ExportKu(
        [FromQuery] long? zbId,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TblPosts.AsQueryable();
        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DepartmentBase == depBase);
        if (zbId.HasValue)
            query = query.Where(p => p.ID_Pos == zbId);
        if (dateFrom.HasValue)
            query = query.Where(p => p.Data_Post >= dateFrom.Value.Date);
        if (dateTo.HasValue)
            query = query.Where(p => p.Data_Post <= dateTo.Value.Date.AddDays(1).AddSeconds(-1));

        var items = await query
            .OrderByDescending(p => p.Data_Post)
            .Select(p => new
            {
                p.ID,
                p.Num_Doc,
                p.Data_Post,
                p.Num_Kol,
                p.Symma,
                p.Name_Dov,
                p.Vid,
                p.SmFil,
                OrgName = db.TblSpravOrgs
                    .Where(o => o.ID == p.ID_Pos)
                    .Select(o => o.Name_Org)
                    .FirstOrDefault()
            })
            .ToListAsync();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("КУ");

        ws.Cell(1, 1).Value = "№";
        ws.Cell(1, 2).Value = "Номер документа";
        ws.Cell(1, 3).Value = "Дата";
        ws.Cell(1, 4).Value = "Год";
        ws.Cell(1, 5).Value = "Организация";
        ws.Cell(1, 6).Value = "Кол-во путёвок";
        ws.Cell(1, 7).Value = "Сумма";
        ws.Cell(1, 8).Value = "Доверенность";
        ws.Cell(1, 9).Value = "Проведено";
        ws.Row(1).Style.Font.Bold = true;

        for (int i = 0; i < items.Count; i++)
        {
            var it = items[i];
            ws.Cell(i + 2, 1).Value = i + 1;
            ws.Cell(i + 2, 2).Value = it.Num_Doc;
            ws.Cell(i + 2, 3).Value = it.Data_Post?.ToString("dd.MM.yyyy");
            ws.Cell(i + 2, 4).Value = it.SmFil;
            ws.Cell(i + 2, 5).Value = it.OrgName;
            ws.Cell(i + 2, 6).Value = it.Num_Kol;
            ws.Cell(i + 2, 7).Value = it.Symma;
            ws.Cell(i + 2, 8).Value = it.Name_Dov;
            ws.Cell(i + 2, 9).Value = it.Vid ? "Да" : "Нет";
        }

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        ms.Position = 0;

        return File(ms.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"ku-{DateTime.Now:yyyyMMdd}.xlsx");
    }
}
