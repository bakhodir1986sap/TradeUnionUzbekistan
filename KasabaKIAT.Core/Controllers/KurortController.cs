using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using KasabaKIAT.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/kurort")]
[Authorize]
public class KurortController(IKurortService kurortService, KasabaKIATContext db) : ControllerBase
{
    private long UserId => long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

    // POST api/kurort/load-profsoyuzy
    [HttpPost("load-profsoyuzy")]
    public async Task<IActionResult> LoadProfsoyuzy()
    {
        await kurortService.LoadSpiskaProfsoyzovAsync(UserId);
        return Ok(new { message = "Список профсоюзов загружен" });
    }

    // GET api/kurort/profsoyuzy
    [HttpGet("profsoyuzy")]
    public async Task<IActionResult> GetProfsoyuzy()
    {
        var list = await db.TbkuStrokaProfsoyazs
            .Select(p => new { p.ID, p.ProfsoyzKod, p.ProfsoyzNaimenovanie })
            .ToListAsync();
        return Ok(list);
    }

    // POST api/kurort/load-grafik-zaezdov
    [HttpPost("load-grafik-zaezdov")]
    public async Task<IActionResult> LoadGrafikZaezdov([FromQuery] int year, [FromQuery] string? sanatoryKod = "")
    {
        if (year < 2000 || year > 2100) return BadRequest("Invalid year");
        await kurortService.LoadZaezdovSanatorievAsync(year, sanatoryKod ?? "", UserId);
        return Ok(new { message = $"График заездов за {year} год загружен" });
    }

    // GET api/kurort/grafik-zaezdov?year=2024
    [HttpGet("grafik-zaezdov")]
    public async Task<IActionResult> GetGrafikZaezdov([FromQuery] int? year = null, [FromQuery] string? sanatoryKod = null)
    {
        var q = db.TbkuGrafikZaezdovs.AsQueryable();
        if (year.HasValue) q = q.Where(g => g.GodZaezdov == year);
        if (!string.IsNullOrEmpty(sanatoryKod)) q = q.Where(g => g.SanatoryKod == sanatoryKod);
        return Ok(await q.ToListAsync());
    }

    // POST api/kurort/send-raspredeleniya?year=2024
    [HttpPost("send-raspredeleniya")]
    public async Task<IActionResult> SendRaspredeleniya([FromQuery] int year)
    {
        if (year < 2000 || year > 2100) return BadRequest("Invalid year");
        await kurortService.SendRaspredeleniyaAsync(year, UserId);
        return Ok(new { message = $"Распределение за {year} год отправлено" });
    }

    // GET api/kurort/raspredeleniya?year=2024
    [HttpGet("raspredeleniya")]
    public async Task<IActionResult> GetRaspredeleniya([FromQuery] int? year = null)
    {
        var q = db.TbkuSpisokRaspredeleniyas.AsQueryable();
        if (year.HasValue) q = q.Where(r => r.GodZaezdov == year);
        return Ok(await q.ToListAsync());
    }

    // POST api/kurort/raspredeleniya
    [HttpPost("raspredeleniya")]
    public async Task<IActionResult> AddRaspredeleniya([FromBody] TbkuSpisokRaspredeleniya row)
    {
        row.ID = 0;
        db.TbkuSpisokRaspredeleniyas.Add(row);
        await db.SaveChangesAsync();
        return Ok(new { row.ID });
    }

    // DELETE api/kurort/raspredeleniya/{id}
    [HttpDelete("raspredeleniya/{id:long}")]
    public async Task<IActionResult> DeleteRaspredeleniya(long id)
    {
        var row = await db.TbkuSpisokRaspredeleniyas.FindAsync(id);
        if (row == null) return NotFound();
        db.TbkuSpisokRaspredeleniyas.Remove(row);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // POST api/kurort/load-schet-faktura
    [HttpPost("load-schet-faktura")]
    public async Task<IActionResult> LoadSchetFaktura(
        [FromQuery] DateTime dateFrom,
        [FromQuery] DateTime dateTo,
        [FromQuery] string? profsoyzKod = "")
    {
        await kurortService.LoadSchetFakturaAsync(dateFrom, dateTo, profsoyzKod ?? "", UserId);
        return Ok(new { message = "Счета-фактуры загружены" });
    }

    // GET api/kurort/schet-faktura?profsoyzKod=xxx
    [HttpGet("schet-faktura")]
    public async Task<IActionResult> GetSchetFaktura([FromQuery] string? profsoyzKod = null, [FromQuery] DateTime? dateFrom = null, [FromQuery] DateTime? dateTo = null)
    {
        var q = db.TbkuDocumentSchetFakturas.AsQueryable();
        if (!string.IsNullOrEmpty(profsoyzKod)) q = q.Where(d => d.ProfsoyzKod == profsoyzKod);
        if (dateFrom.HasValue) q = q.Where(d => d.DocData >= dateFrom);
        if (dateTo.HasValue) q = q.Where(d => d.DocData <= dateTo);
        return Ok(await q.ToListAsync());
    }

    // GET api/kurort/schet-faktura/{id}/details
    [HttpGet("schet-faktura/{id:long}/details")]
    public async Task<IActionResult> GetSchetFakturaDetails(long id)
    {
        var details = await db.TbkuStrokaSchetFakturis
            .Where(s => s.SchetFactura_ID == id)
            .ToListAsync();
        return Ok(details);
    }

    // POST api/kurort/load-zaezdy-clientov
    [HttpPost("load-zaezdy-clientov")]
    public async Task<IActionResult> LoadZaezdyClientov(
        [FromQuery] string sanatoryKod,
        [FromQuery] DateTime dateFrom,
        [FromQuery] DateTime dateTo)
    {
        if (string.IsNullOrEmpty(sanatoryKod)) return BadRequest("sanatoryKod is required");
        await kurortService.LoadZaezdyClientovAsync(sanatoryKod, dateFrom, dateTo, UserId);
        return Ok(new { message = "Заезды клиентов загружены" });
    }

    // GET api/kurort/zaezdy-clientov?sanatoryKod=xxx
    [HttpGet("zaezdy-clientov")]
    public async Task<IActionResult> GetZaezdyClientov([FromQuery] string? sanatoryKod = null)
    {
        var q = db.TbkuZaezdyClientovs.AsQueryable();
        if (!string.IsNullOrEmpty(sanatoryKod)) q = q.Where(z => z.SanatoryKod == sanatoryKod);
        return Ok(await q.ToListAsync());
    }

    // GET api/kurort/journal
    [HttpGet("journal")]
    public async Task<IActionResult> GetJournal([FromQuery] int? serviceId = null)
    {
        var q = db.TbkuJournals.AsQueryable();
        if (serviceId.HasValue) q = q.Where(j => j.ServiceID == serviceId);
        return Ok(await q.OrderByDescending(j => j.CreateDT).Take(200).ToListAsync());
    }
}
