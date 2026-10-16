using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

/// <summary>
/// Putyovki Zayavki (voucher applications) — ported from BusinessLogic/Putyovki/Zayavki/*
/// </summary>
[ApiController]
[Route("api/zayavki")]
[Authorize]
public class ZayavkiController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? year, [FromQuery] int? kv, [FromQuery] string? status)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TbptZayavkas.AsQueryable();

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(z => z.DEPARTMENT_BASE == depBase);
        if (year.HasValue) query = query.Where(z => z.YEAR == year);
        if (kv.HasValue)   query = query.Where(z => z.KVARTAL == kv);
        if (!string.IsNullOrEmpty(status)) query = query.Where(z => z.STATUS == status);

        var items = await query
            .OrderByDescending(z => z.CREATE_DATE)
            .Select(z => new
            {
                z.ID,
                z.DEPARTMENT_BASE,
                z.ORG_ID,
                OrgName = db.TblSpravOrgs
                    .Where(o => o.ID == z.ORG_ID)
                    .Select(o => o.Name_Org).FirstOrDefault(),
                z.YEAR,
                z.KVARTAL,
                z.QUANTITY,
                z.STATUS,
                z.CREATE_DATE
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.TbptZayavkas.FindAsync(id);
        if (item == null) return NotFound();

        var details = await db.TbptZayavkaDetails
            .Where(d => d.ZAYAVKA_ID == id)
            .Select(d => new
            {
                d.ID,
                d.ID_SANAT,
                SanatName = db.TblSpravSans
                    .Where(s => s.ID == d.ID_SANAT)
                    .Select(s => s.Name_San).FirstOrDefault(),
                d.QUANTITY,
                d.PRICE
            })
            .ToListAsync();

        return Ok(new { header = item, details });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateZayavkaRequest req)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        var zayavka = new TbptZayavka
        {
            DEPARTMENT_BASE = depBase,
            ORG_ID = req.OrgId,
            YEAR = req.Year,
            KVARTAL = req.Kvartal,
            QUANTITY = req.Details.Sum(d => d.Quantity),
            STATUS = "NEW",
            CREATE_DATE = DateTime.Now,
            USER_ID = userId
        };

        db.TbptZayavkas.Add(zayavka);
        await db.SaveChangesAsync();

        foreach (var detail in req.Details)
        {
            db.TbptZayavkaDetails.Add(new TbptZayavkaDetail
            {
                ZAYAVKA_ID = zayavka.ID,
                ID_SANAT = detail.SanatId,
                QUANTITY = detail.Quantity,
                PRICE = detail.Price
            });
        }

        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = zayavka.ID }, zayavka);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] ZayavkaStatusRequest req)
    {
        var item = await db.TbptZayavkas.FindAsync(id);
        if (item == null) return NotFound();
        item.STATUS = req.Status;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.TbptZayavkas.FindAsync(id);
        if (item == null) return NotFound();
        if (item.STATUS != "NEW") return BadRequest(new { message = "Только новые заявки можно удалять." });

        var details = db.TbptZayavkaDetails.Where(d => d.ZAYAVKA_ID == id);
        db.TbptZayavkaDetails.RemoveRange(details);
        db.TbptZayavkas.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}

public class CreateZayavkaRequest
{
    public long OrgId { get; set; }
    public int Year { get; set; }
    public int Kvartal { get; set; }
    public List<ZayavkaDetailItem> Details { get; set; } = [];
}

public class ZayavkaDetailItem
{
    public int SanatId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class ZayavkaStatusRequest
{
    public string Status { get; set; } = string.Empty;
}
