using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

/// <summary>
/// Oprixodovaniye from KU invoices (tbku_DocumentSchetFaktura → tblPost + tblPos_Put).
/// Ported from BusinessLogic/Putyovki/PrixodPutyovok/Oprixodovaniye.cs
/// </summary>
[ApiController]
[Route("api/oprixodovaniye-ku")]
[Authorize]
public class OprixodovaniyeKuController(KasabaKIATContext db) : ControllerBase
{
    // ── GET /api/oprixodovaniye-ku/pending ───────────────────────────────
    // Lists KU invoices not yet posted (no matching tblPost record)
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending(
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo,
        [FromQuery] string? profsoyuzKod)
    {
        var query = db.TbkuDocumentSchetFakturas.AsQueryable();

        if (dateFrom.HasValue)
            query = query.Where(d => d.DocData >= dateFrom.Value.Date.AddSeconds(1));
        if (dateTo.HasValue)
            query = query.Where(d => d.DocData <= dateTo.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
        if (!string.IsNullOrEmpty(profsoyuzKod))
            query = query.Where(d => d.ProfsoyzKod == profsoyuzKod);

        // Exclude documents already posted
        query = query.Where(d => d.DocTypeRealVozv != "В");

        var items = await query
            .OrderBy(d => d.DocData)
            .Select(d => new
            {
                d.ID,
                d.DocNomer,
                d.DocData,
                d.ProfsoyzKod,
                d.KolichestvoPutevok,
                d.SummaDoc,
                alreadyPosted = db.TblPosts
                    .Any(p => p.Num_Doc == d.DocNomer && p.Data_Post == d.DocData!.Value.Date)
            })
            .ToListAsync();

        return Ok(items);
    }

    // ── POST /api/oprixodovaniye-ku/process ──────────────────────────────
    // Processes a date range — creates tblPost + tblPos_Put from KU invoices
    [HttpPost("process")]
    public async Task<IActionResult> Process([FromBody] OprixodovaniyeKuRequest req)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        var periodNach = req.DateFrom.Date.AddSeconds(1);
        var periodOkon = req.DateTo.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        var docs = await db.TbkuDocumentSchetFakturas
            .Where(d => d.DocData >= periodNach
                     && d.DocData <= periodOkon
                     && d.ProfsoyzKod == req.ProfsoyuzKod
                     && d.DocTypeRealVozv != "В")
            .ToListAsync();

        int created = 0, skipped = 0;

        foreach (var doc in docs)
        {
            var docDate = doc.DocData?.Date;

            // Skip already posted
            var alreadyPosted = await db.TblPosts
                .AnyAsync(p => p.Num_Doc == doc.DocNomer && p.Data_Post == docDate);

            if (alreadyPosted) { skipped++; continue; }

            var kv = docDate.HasValue ? (docDate.Value.Month + 2) / 3 : 1;

            // Resolve department from profsoyuz mapping
            var strokaProfsoyza = await db.TbkuStrokaProfsoyazs
                .Where(s => s.ProfsoyzKod == req.ProfsoyuzKod)
                .FirstOrDefaultAsync();

            var deptBase = strokaProfsoyza?.MATCH_DEPARTMENTBASE ?? depBase;

            var dovTotal = string.Join(" ",
                doc.ProfsoyzDoverennost,
                doc.ProfsoyzDoverennostData?.ToString("dd.MM.yyyy"),
                doc.ProfsoyzDoverennostMOL,
                doc.ProfsoyzDogovor).Trim();

            var post = new TblPost
            {
                ID_KV = kv,
                Data_Post = docDate,
                Num_Doc = doc.DocNomer,
                ID_Pos = 2, // KU
                Num_Kol = doc.KolichestvoPutevok,
                Symma = doc.SummaDoc,
                Name_Dov = dovTotal.Length > 50 ? dovTotal[..49] : dovTotal,
                Vid = false,
                SmFil = docDate?.Year,
                UserId = userId,
                DepartmentBase = deptBase
            };

            db.TblPosts.Add(post);
            await db.SaveChangesAsync(); // need ID for details

            // Process detail lines
            var lines = await db.TbkuStrokaSchetFakturis
                .Where(l => l.SchetFactura_ID == doc.ID)
                .ToListAsync();

            foreach (var line in lines)
            {
                if (!uint.TryParse(line.Blank1, out uint begin) ||
                    !uint.TryParse(line.Blank2, out uint end))
                    continue;

                if (begin > end)
                    return BadRequest(new { message = $"Blank1 ({line.Blank1}) превышает Blank2 ({line.Blank2}) в строке {line.SanatoryKod}." });

                int sanatId = 0;
                if (int.TryParse(line.SanatoryKod, out int parsedSanatId))
                {
                    var san = await db.TblSpravSans
                        .Where(s => s.KurortSanID == parsedSanatId)
                        .FirstOrDefaultAsync();

                    if (san != null)
                    {
                        sanatId = (int)san.ID;
                    }
                    else
                    {
                        // Auto-create sanatorium entry
                        var newSan = new TblSpravSan
                        {
                            Name_San = line.SanatoryNaimenovanie ?? "Unknown",
                            Num_Kod = line.SanatoryKod,
                            KurortSanID = parsedSanatId
                        };
                        db.TblSpravSans.Add(newSan);
                        await db.SaveChangesAsync();
                        sanatId = (int)newSan.ID;
                    }
                }

                for (uint j = begin; j <= end; j++)
                {
                    var detail = new TblPosPut
                    {
                        ID_Post = post.ID,
                        ID_Sanat = sanatId,
                        Num_Put = j.ToString().PadLeft(6, '0'),
                        DataSan = (line.DataZaezda == DateTime.MinValue || line.DataZaezda == DateTime.MaxValue)
                            ? null : line.DataZaezda,
                        Symma_Put = line.Cena,
                        ID_PolVoz = false,
                        Data_Post = DateTime.Today,
                        UserId = userId,
                        Departmentbase = deptBase,
                        TIMESTAMP_T = DateTime.Now
                    };
                    db.TblPosPuts.Add(detail);
                }
            }

            await db.SaveChangesAsync();
            created++;
        }

        return Ok(new { created, skipped, total = docs.Count });
    }
}

public class OprixodovaniyeKuRequest
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string ProfsoyuzKod { get; set; } = string.Empty;
}
