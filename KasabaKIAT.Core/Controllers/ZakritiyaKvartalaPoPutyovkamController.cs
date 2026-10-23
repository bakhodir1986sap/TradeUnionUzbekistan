using KasabaKIAT.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/zakritiyakvartalapoputyovkam")]
[Authorize]
public class ZakritiyaKvartalaPoPutyovkamController(KasabaKIATContext db) : ControllerBase
{
    // GET /api/zakritiyakvartalapoputyovkam
    //   ?yearId=2026&kvartalId=1&organizationId=5
    [HttpGet]
    public async Task<IActionResult> GetPutyovkas(
        [FromQuery] int yearId,
        [FromQuery] int kvartalId,
        [FromQuery] long organizationId)
    {
        var rows = await (
            from p in db.TblPosPuts
            join po in db.TblPosts on p.ID_Post equals po.ID
            where po.ID_Pos == organizationId
               && p.DataSan != null
               && p.DataSan.Value.Year == yearId
               && (p.DataSan.Value.Month - 1) / 3 + 1 == kvartalId
               && p.Status != "CLOSED"
            select new
            {
                id = p.ID,
                sanatName = db.TblSpravSans
                    .Where(s => s.ID == p.ID_Sanat)
                    .Select(s => s.Name_San)
                    .FirstOrDefault() ?? "",
                num_put = p.Num_Put ?? "",
                symma = p.Symma_Put ?? 0m,
                data_post = p.Data_Post,
            })
            .OrderBy(p => p.num_put)
            .ToListAsync();

        var zakritiyeRes = new
        {
            PutyovkaCount = rows.Count,
            SummaNakladnix = rows.Sum(r => r.symma),
        };

        return Ok(new { putyovkas = rows, zakritiyeRes });
    }

    // POST /api/zakritiyakvartalapoputyovkam/close
    // Body: { ids: [1, 2, 3] }
    [HttpPost("close")]
    public async Task<IActionResult> ClosePutyovkas([FromBody] CloseRequest req)
    {
        if (req.Ids == null || req.Ids.Length == 0)
            return BadRequest("Список ID пуст.");

        var rows = await db.TblPosPuts
            .Where(p => req.Ids.Contains(p.ID))
            .ToListAsync();

        foreach (var row in rows)
            row.Status = "CLOSED";

        await db.SaveChangesAsync();
        return Ok(new { closed = rows.Count });
    }
}

public class CloseRequest
{
    public long[] Ids { get; set; } = [];
}
