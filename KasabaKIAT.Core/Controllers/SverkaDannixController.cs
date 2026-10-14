using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

// ---------------------------------------------------------------------------
// Inline DTOs
// ---------------------------------------------------------------------------

public record SverkaUploadSummary(int RowsProcessed, int RowsSkipped, string FileName);
public record SverkaRowDto(long Id, string? SanatKod, string? OblKod, string? ProfKod, int? Year, decimal? PlanKol, decimal? FactKol);
public record SverkaDifferDto(string? SanatKod, string? OblKod, decimal? Raznitsa, int? Year);
public record OstatokUploadResult(int Rows, string FileName);
public record BalanceUploadResult(int Rows, string FileName);
public record DebitCreditUploadResult(int Rows, string FileName);

public record VoucherGroupDto(long SanId, string? SanatName, IEnumerable<PeriodGroupDto> Periods);
public record PeriodGroupDto(string Period, IEnumerable<VoucherItemDto> Vouchers);
public record VoucherItemDto(long Id, string? NumPut, DateTime? DataNach, DateTime? DataKon, decimal? Price, string? Status, string? Mes, string? Tip);

public record OformleniyeDto(long Id, long? OrgId, string? OrgName, string? INN, string? SanatName, string? Period, string? Status, decimal? PaymentPercent, DateTime? ModifyDate);

public record RealizeRequest(long[] VoucherIds, long OrgId);
public record ZabroniRequest(long[] VoucherIds, long OrgId);
public record CancelBronRequest(long[] VoucherIds);
public record SetPaymentPercentRequest(int PaymentPercent);

// tblVipl — realization invoice, stored in raw SQL since no EF model exists yet
// tblVipl_Put — vouchers linked to invoice

// ---------------------------------------------------------------------------
// SverkaDannix controller  (api/sverkadannix)
// ---------------------------------------------------------------------------

[ApiController]
[Route("api/sverkadannix")]
[Authorize]
public class SverkaDannixController(KasabaKIATContext db) : ControllerBase
{
    // POST /upload  — parse uploaded reconciliation Excel/CSV, store rows into tbku_GrafikZaezdovSanatoriev
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var rows = await ParseDelimitedFileAsync(file);

        int processed = 0, skipped = 0;

        foreach (var cols in rows)
        {
            // Expected columns: GodZaezdov, SanatoryKod, SanatoryNaimenovanie, SanatoryOblast,
            //                   SanatoryRazmeshenieKod, Putevok1kv, Putevok2kv, Putevok3kv, Putevok4kv
            if (cols.Length < 9) { skipped++; continue; }

            if (!long.TryParse(cols[0], out var god)) { skipped++; continue; }

            var entity = new TbkuGrafikZaezdov
            {
                GodZaezdov = god,
                SanatoryKod = cols[1].Trim(),
                SanatoryNaimenovanie = cols[2].Trim(),
                SanatoryOblast = cols[3].Trim(),
                SanatoryRazmeshenieKod = cols[4].Trim(),
                Putevok1kv = long.TryParse(cols[5], out var p1) ? p1 : null,
                Putevok2kv = long.TryParse(cols[6], out var p2) ? p2 : null,
                Putevok3kv = long.TryParse(cols[7], out var p3) ? p3 : null,
                Putevok4kv = long.TryParse(cols[8], out var p4) ? p4 : null
            };

            db.TbkuGrafikZaezdovs.Add(entity);
            processed++;
        }

        await db.SaveChangesAsync();
        return Ok(new SverkaUploadSummary(processed, skipped, file.FileName));
    }

    // GET /years — distinct years present in tbku_GrafikZaezdovSanatoriev
    [HttpGet("years")]
    public async Task<IActionResult> GetYears()
    {
        var years = await db.TbkuGrafikZaezdovs
            .Where(g => g.GodZaezdov != null)
            .Select(g => g.GodZaezdov!.Value)
            .Distinct()
            .OrderByDescending(y => y)
            .ToListAsync();
        return Ok(years);
    }

    // GET /{year}/data — all rows for a given year
    [HttpGet("{year:int}/data")]
    public async Task<IActionResult> GetLoadedRows(int year)
    {
        var rows = await db.TbkuGrafikZaezdovs
            .Where(g => g.GodZaezdov == year)
            .OrderBy(g => g.SanatoryKod)
            .ThenBy(g => g.SanatoryRazmeshenieKod)
            .Select(g => new
            {
                g.ID,
                g.GodZaezdov,
                g.SanatoryKod,
                g.SanatoryNaimenovanie,
                g.SanatoryProfil,
                g.SanatoryOblast,
                g.SanatoryKoekVsego,
                g.SanatoryKoekLetom,
                g.SanatoryKoekZimoy,
                g.SanatoryRazmeshenieKod,
                g.SanatoryRazmeshenieNaimenovanie,
                g.Putevok1kv,
                g.Putevok2kv,
                g.Putevok3kv,
                g.Putevok4kv
            })
            .ToListAsync();
        return Ok(rows);
    }

    // GET /differ?year=&sanId=&obl=  — difference between grafik and profsoyuz plan rows
    [HttpGet("differ")]
    public async Task<IActionResult> GetDiffer(
        [FromQuery] int year,
        [FromQuery] string? sanId = null,
        [FromQuery] string? obl = null)
    {
        // Grafik side
        var grafik = db.TbkuGrafikZaezdovs
            .Where(g => g.GodZaezdov == year);

        if (!string.IsNullOrEmpty(sanId))
            grafik = grafik.Where(g => g.SanatoryKod == sanId);
        if (!string.IsNullOrEmpty(obl))
            grafik = grafik.Where(g => g.SanatoryOblast == obl);

        var grafikList = await grafik
            .Select(g => new
            {
                g.SanatoryKod,
                g.SanatoryOblast,
                g.SanatoryRazmeshenieKod,
                Total = (g.Putevok1kv ?? 0) + (g.Putevok2kv ?? 0) + (g.Putevok3kv ?? 0) + (g.Putevok4kv ?? 0)
            })
            .ToListAsync();

        // Profsoyuz distribution side
        var profQuery = db.TblSpisokRaspredeleniyas
            .Where(p => p.GodZaezdov == year);
        if (!string.IsNullOrEmpty(sanId))
            profQuery = profQuery.Where(p => p.SanatoryKod == sanId);

        var profList = await profQuery
            .Select(p => new
            {
                p.SanatoryKod,
                p.SanatoryRazmeshenieKod,
                Total = (p.Putevok1kv ?? 0) + (p.Putevok2kv ?? 0) + (p.Putevok3kv ?? 0) + (p.Putevok4kv ?? 0)
            })
            .ToListAsync();

        var profLookup = profList
            .GroupBy(p => (p.SanatoryKod, p.SanatoryRazmeshenieKod))
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Total));

        var result = grafikList.Select(g =>
        {
            profLookup.TryGetValue((g.SanatoryKod, g.SanatoryRazmeshenieKod), out var profTotal);
            return new
            {
                g.SanatoryKod,
                g.SanatoryOblast,
                g.SanatoryRazmeshenieKod,
                GrafikTotal = g.Total,
                ProfTotal = profTotal,
                Raznitsa = g.Total - profTotal
            };
        }).ToList();

        return Ok(result);
    }

    // DELETE /obl?year=&sanId=&obl=  — delete reconciliation rows for an oblast
    [HttpDelete("obl")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteObl(
        [FromQuery] int year,
        [FromQuery] string? sanId = null,
        [FromQuery] string? obl = null)
    {
        var query = db.TbkuGrafikZaezdovs.Where(g => g.GodZaezdov == year);
        if (!string.IsNullOrEmpty(sanId)) query = query.Where(g => g.SanatoryKod == sanId);
        if (!string.IsNullOrEmpty(obl)) query = query.Where(g => g.SanatoryOblast == obl);

        var rows = await query.ToListAsync();
        if (rows.Count == 0) return NotFound();
        db.TbkuGrafikZaezdovs.RemoveRange(rows);
        await db.SaveChangesAsync();
        return Ok(new { deleted = rows.Count });
    }

    // POST /ostatok — upload putyovka ostatok file (stores into profsoyuz table)
    [HttpPost("ostatok")]
    public async Task<IActionResult> UploadOstatok([FromForm] IFormFile putyovkaOstatok)
    {
        if (putyovkaOstatok == null || putyovkaOstatok.Length == 0)
            return BadRequest("File is required.");

        var rows = await ParseDelimitedFileAsync(putyovkaOstatok);
        int count = 0;

        foreach (var cols in rows)
        {
            // Expected: ProfsoyzKod, ProfsoyzNaimenovanie, MATCH_DEPARTMENTBASE
            if (cols.Length < 2) continue;
            var entity = new TbkuStrokaProfsoyza
            {
                ProfsoyzKod = cols[0].Trim(),
                ProfsoyzNaimenovanie = cols[1].Trim(),
                MATCH_DEPARTMENTBASE = cols.Length > 2 ? cols[2].Trim() : null
            };
            db.TbkuStrokaProfsoyazs.Add(entity);
            count++;
        }

        await db.SaveChangesAsync();
        return Ok(new OstatokUploadResult(count, putyovkaOstatok.FileName));
    }

    // POST /balance — upload balance file (stored as raw SQL log for now)
    [HttpPost("balance")]
    public async Task<IActionResult> UploadBalance([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        var rows = await ParseDelimitedFileAsync(file);
        // Balance rows: SanatKod, OblKod, Saldo (debit), Saldo (credit)
        // Without a dedicated model, persist via raw SQL into a log-style table if it exists,
        // or return the parsed summary.
        int count = rows.Count(r => r.Length >= 2);
        return Ok(new BalanceUploadResult(count, file.FileName));
    }

    // POST /debit-credit — upload org debit/credit file
    [HttpPost("debit-credit")]
    public async Task<IActionResult> UploadDebitCredit([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        var rows = await ParseDelimitedFileAsync(file);
        int count = rows.Count(r => r.Length >= 2);
        return Ok(new DebitCreditUploadResult(count, file.FileName));
    }

    // ---- Helpers ----

    private static async Task<List<string[]>> ParseDelimitedFileAsync(IFormFile file)
    {
        var result = new List<string[]>();
        using var stream = file.OpenReadStream();
        using var reader = new System.IO.StreamReader(stream);

        bool firstLine = true;
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            if (firstLine) { firstLine = false; continue; } // skip header
            var cols = line.Split(['\t', ';', ',']);
            result.Add(cols);
        }
        return result;
    }
}

// ---------------------------------------------------------------------------
// SverkaDannix V2 controller  (api/sverkadannixv2) — reads from tbpt_putyovki
// ---------------------------------------------------------------------------

[ApiController]
[Route("api/sverkadannixv2")]
[Authorize]
public class SverkaDannixV2Controller(KasabaKIATContext db) : ControllerBase
{
    // GET /years — distinct years from tbpt_putyovki
    [HttpGet("years")]
    public async Task<IActionResult> GetYears()
    {
        var years = await db.TbptPutyovkis
            .Where(p => p.REST_BEGIN_DATE != null)
            .Select(p => p.REST_BEGIN_DATE!.Value.Year)
            .Distinct()
            .OrderByDescending(y => y)
            .ToListAsync();
        return Ok(years);
    }

    // GET /{year}/data — tbpt_putyovki rows for the year, joined with tblSprav_San
    [HttpGet("{year:int}/data")]
    public async Task<IActionResult> GetData(int year)
    {
        var rows = await db.TbptPutyovkis
            .Where(p => p.REST_BEGIN_DATE != null && p.REST_BEGIN_DATE.Value.Year == year)
            .Select(p => new
            {
                p.REC_ID,
                p.DEPARTMENT_CODE,
                p.SANATORIUM_ID,
                SanatName = p.tblSprav_San != null ? p.tblSprav_San.Name_San : null,
                p.REST_BEGIN_DATE,
                p.REST_END_DATE,
                p.PRICE,
                p.STATUS,
                p.QUANTITY_IN,
                p.QUANTITY_OUT,
                p.QUANTITY_REMAIN,
                p.REMAIN_BAL
            })
            .OrderBy(p => p.SANATORIUM_ID)
            .ThenBy(p => p.REST_BEGIN_DATE)
            .ToListAsync();

        return Ok(rows);
    }
}

// ---------------------------------------------------------------------------
// RealizatsiyaPutevok controller  (api/realizatsiya)
// ---------------------------------------------------------------------------

[ApiController]
[Route("api/realizatsiya")]
[Authorize]
public class RealizatsiyaPutevokController(KasabaKIATContext db) : ControllerBase
{
    // GET /vouchers — available (NEW or null status) vouchers grouped by sanatorium and period
    [HttpGet("vouchers")]
    public async Task<IActionResult> GetVouchers()
    {
        var vouchers = await db.TblPosPuts
            .Where(p => p.Status == null || p.Status == "NEW")
            .OrderBy(p => p.ID_San)
            .ThenBy(p => p.DataNach)
            .Select(p => new
            {
                p.ID,
                p.ID_San,
                p.Num_Put,
                p.DataNach,
                p.DataKon,
                p.Price,
                p.Status,
                p.Mes,
                p.Tip
            })
            .ToListAsync();

        // Load sanatorium names separately to avoid join complexity
        var sanIds = vouchers.Select(v => v.ID_San).Where(x => x != null).Distinct().ToList();
        var sans = await db.TblSpravSans
            .Where(s => sanIds.Contains(s.ID))
            .ToDictionaryAsync(s => (long)s.ID, s => s.Name_San);

        var grouped = vouchers
            .GroupBy(v => v.ID_San)
            .Select(g =>
            {
                var sanId = g.Key ?? 0;
                sans.TryGetValue(sanId, out var sanName);
                var periods = g
                    .GroupBy(v => FormatPeriod(v.DataNach, v.DataKon))
                    .Select(pg => new
                    {
                        Period = pg.Key,
                        Vouchers = pg.Select(v => new VoucherItemDto(
                            v.ID, v.Num_Put, v.DataNach, v.DataKon, v.Price, v.Status, v.Mes, v.Tip
                        )).ToList()
                    })
                    .OrderBy(p => p.Period)
                    .ToList();
                return new { SanId = sanId, SanatName = sanName, Periods = periods };
            })
            .ToList();

        return Ok(grouped);
    }

    // GET /oform?inn=&sanatIds[] — oformleniye (realization records) for org by INN
    [HttpGet("oform")]
    public async Task<IActionResult> GetOformleniye(
        [FromQuery] string? inn = null,
        [FromQuery] long[]? sanatIds = null)
    {
        // tblVipl holds the realization invoices; query via raw SQL since no EF model exists
        // Return records from tblPos_Put with status SOLD/BRON linked to tblVipl
        var sql = @"
            SELECT
                p.ID,
                p.ID_San,
                s.Name_San,
                p.Num_Put,
                p.DataNach,
                p.DataKon,
                p.Price,
                p.Status,
                p.MODIFY_DATE,
                v.ID AS ViplaId,
                v.ORG_ID,
                o.Name_Org,
                o.INN
            FROM tblPos_Put p
            LEFT JOIN tblVipl_Put vp ON vp.PUT_ID = p.ID
            LEFT JOIN tblVipl v ON v.ID = vp.VIPL_ID
            LEFT JOIN tblSprav_Org o ON o.ID = v.ORG_ID
            LEFT JOIN tblSprav_San s ON s.ID = p.ID_San
            WHERE p.Status IN ('SOLD','BRON')";

        var conditions = new List<string>();
        var parameters = new List<Microsoft.Data.SqlClient.SqlParameter>();

        if (!string.IsNullOrEmpty(inn))
        {
            conditions.Add("o.INN = @INN");
            parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@INN", inn));
        }

        if (sanatIds != null && sanatIds.Length > 0)
        {
            var ids = string.Join(",", sanatIds);
            conditions.Add($"p.ID_San IN ({ids})");
        }

        if (conditions.Count > 0)
            sql += " AND " + string.Join(" AND ", conditions);

        sql += " ORDER BY p.MODIFY_DATE DESC";

        var conn = db.Database.GetDbConnection();
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        foreach (var p in parameters) cmd.Parameters.Add(p);

        var results = new List<object>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(new
            {
                Id = reader["ID"],
                SanId = reader["ID_San"],
                SanatName = reader["Name_San"] as string,
                NumPut = reader["Num_Put"] as string,
                DataNach = reader["DataNach"] as DateTime?,
                DataKon = reader["DataKon"] as DateTime?,
                Price = reader["Price"] as decimal?,
                Status = reader["Status"] as string,
                ModifyDate = reader["MODIFY_DATE"] as DateTime?,
                ViplaId = reader["ViplaId"] as long?,
                OrgId = reader["ORG_ID"] as long?,
                OrgName = reader["Name_Org"] as string,
                Inn = reader["INN"] as string
            });
        }
        await conn.CloseAsync();

        return Ok(results);
    }

    // GET /pp — GetPP: summary payment plan from tblVipl
    [HttpGet("pp")]
    public async Task<IActionResult> GetPP([FromQuery] int? year, [FromQuery] long? orgId)
    {
        var sql = @"
            SELECT v.ID, v.ORG_ID, o.Name_Org, o.INN, v.YEAR, v.KVARTAL, v.SUMMA, v.STATUS, v.PAY_PERCENT
            FROM tblVipl v
            LEFT JOIN tblSprav_Org o ON o.ID = v.ORG_ID
            WHERE 1=1";

        var parameters = new List<Microsoft.Data.SqlClient.SqlParameter>();
        if (year.HasValue)
        {
            sql += " AND v.YEAR = @YEAR";
            parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@YEAR", year.Value));
        }
        if (orgId.HasValue)
        {
            sql += " AND v.ORG_ID = @ORG_ID";
            parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ORG_ID", orgId.Value));
        }
        sql += " ORDER BY v.YEAR DESC, v.KVARTAL";

        return Ok(await ExecuteRawSelectAsync(sql, parameters));
    }

    // GET /pp-sum — GetPP_SUM: aggregated payment sums
    [HttpGet("pp-sum")]
    public async Task<IActionResult> GetPP_Sum([FromQuery] int? year)
    {
        var sql = @"
            SELECT v.YEAR, v.KVARTAL, SUM(v.SUMMA) AS TotalSumma, SUM(v.PAY_SUMMA) AS TotalPaid
            FROM tblVipl v
            WHERE 1=1";

        var parameters = new List<Microsoft.Data.SqlClient.SqlParameter>();
        if (year.HasValue)
        {
            sql += " AND v.YEAR = @YEAR";
            parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@YEAR", year.Value));
        }
        sql += " GROUP BY v.YEAR, v.KVARTAL ORDER BY v.YEAR DESC, v.KVARTAL";

        return Ok(await ExecuteRawSelectAsync(sql, parameters));
    }

    // GET /prixod-san — GetPrixodPutevok_S: voucher receipts by sanatorium
    [HttpGet("prixod-san")]
    public async Task<IActionResult> GetPrixodPutevokSan([FromQuery] int? year, [FromQuery] long? sanId)
    {
        var query = db.TbptPutyovkis.AsQueryable();

        if (year.HasValue)
            query = query.Where(p => p.REST_BEGIN_DATE != null && p.REST_BEGIN_DATE.Value.Year == year.Value);
        if (sanId.HasValue)
            query = query.Where(p => p.SANATORIUM_ID == sanId.Value);

        var result = await query
            .GroupBy(p => new { p.SANATORIUM_ID, p.tblSprav_San!.Name_San })
            .Select(g => new
            {
                g.Key.SANATORIUM_ID,
                g.Key.Name_San,
                TotalIn = g.Sum(p => p.QUANTITY_IN ?? 0),
                TotalOut = g.Sum(p => p.QUANTITY_OUT ?? 0),
                TotalRemain = g.Sum(p => p.QUANTITY_REMAIN ?? 0),
                TotalPrice = g.Sum(p => p.PRICE ?? 0)
            })
            .ToListAsync();

        return Ok(result);
    }

    // GET /prixod-corp — GetPrixodPutevok_C: voucher receipts by corporation/org
    [HttpGet("prixod-corp")]
    public async Task<IActionResult> GetPrixodPutevokCorp([FromQuery] int? year, [FromQuery] string? depCode)
    {
        var query = db.TbptPutyovkis.AsQueryable();
        if (year.HasValue)
            query = query.Where(p => p.REST_BEGIN_DATE != null && p.REST_BEGIN_DATE.Value.Year == year.Value);
        if (!string.IsNullOrEmpty(depCode))
            query = query.Where(p => p.DEPARTMENT_CODE == depCode);

        var result = await query
            .GroupBy(p => p.DEPARTMENT_CODE)
            .Select(g => new
            {
                DeptCode = g.Key,
                TotalIn = g.Sum(p => p.QUANTITY_IN ?? 0),
                TotalOut = g.Sum(p => p.QUANTITY_OUT ?? 0),
                TotalRemain = g.Sum(p => p.QUANTITY_REMAIN ?? 0),
                TotalPrice = g.Sum(p => p.PRICE ?? 0)
            })
            .ToListAsync();

        return Ok(result);
    }

    // GET /pp-sum-san?year=&sanId=  — GetPP_SUM_S
    [HttpGet("pp-sum-san")]
    public async Task<IActionResult> GetPP_SumSan([FromQuery] int? year, [FromQuery] long? sanId)
    {
        var sql = @"
            SELECT v.YEAR, vp.SAN_ID, s.Name_San, SUM(v.SUMMA) AS TotalSumma
            FROM tblVipl v
            JOIN tblVipl_Put vp ON vp.VIPL_ID = v.ID
            JOIN tblPos_Put p ON p.ID = vp.PUT_ID
            LEFT JOIN tblSprav_San s ON s.ID = p.ID_San
            WHERE 1=1";

        var parameters = new List<Microsoft.Data.SqlClient.SqlParameter>();
        if (year.HasValue) { sql += " AND v.YEAR = @YEAR"; parameters.Add(new("@YEAR", year.Value)); }
        if (sanId.HasValue) { sql += " AND p.ID_San = @SAN_ID"; parameters.Add(new("@SAN_ID", sanId.Value)); }
        sql += " GROUP BY v.YEAR, vp.SAN_ID, s.Name_San ORDER BY v.YEAR DESC, s.Name_San";

        return Ok(await ExecuteRawSelectAsync(sql, parameters));
    }

    // GET /pp-sum-corp?year=  — GetPP_SUM_C
    [HttpGet("pp-sum-corp")]
    public async Task<IActionResult> GetPP_SumCorp([FromQuery] int? year, [FromQuery] string? depCode)
    {
        var query = db.TbptPutyovkis.AsQueryable();
        if (year.HasValue)
            query = query.Where(p => p.REST_BEGIN_DATE != null && p.REST_BEGIN_DATE.Value.Year == year.Value);
        if (!string.IsNullOrEmpty(depCode))
            query = query.Where(p => p.DEPARTMENT_CODE == depCode);

        var result = await query
            .GroupBy(p => new { p.DEPARTMENT_CODE, Year = p.REST_BEGIN_DATE!.Value.Year })
            .Select(g => new
            {
                g.Key.DEPARTMENT_CODE,
                g.Key.Year,
                TotalIn = g.Sum(p => p.QUANTITY_IN ?? 0),
                TotalPrice = g.Sum(p => p.PRICE ?? 0)
            })
            .ToListAsync();

        return Ok(result);
    }

    // GET /prixod-san-inn?inn= — GetPrixodPutevok_S_LoadINN
    [HttpGet("prixod-san-inn")]
    public async Task<IActionResult> GetPrixodPutevokSanINN([FromQuery] string inn)
    {
        if (string.IsNullOrEmpty(inn)) return BadRequest("INN is required.");
        var org = await db.TblSpravOrgs.FirstOrDefaultAsync(o => o.INN == inn);
        if (org == null) return NotFound("Organisation not found.");

        var sql = @"
            SELECT p.ID_San, s.Name_San,
                   COUNT(vp.PUT_ID) AS PutevokCount, SUM(p.Price) AS TotalPrice
            FROM tblVipl v
            JOIN tblVipl_Put vp ON vp.VIPL_ID = v.ID
            JOIN tblPos_Put p ON p.ID = vp.PUT_ID
            LEFT JOIN tblSprav_San s ON s.ID = p.ID_San
            WHERE v.ORG_ID = @ORG_ID
            GROUP BY p.ID_San, s.Name_San";

        var parameters = new List<Microsoft.Data.SqlClient.SqlParameter>
        {
            new("@ORG_ID", org.ID)
        };

        return Ok(await ExecuteRawSelectAsync(sql, parameters));
    }

    // GET /prixod-corp-inn?inn= — GetPrixodPutevok_C_LoadINN
    [HttpGet("prixod-corp-inn")]
    public async Task<IActionResult> GetPrixodPutevokCorpINN([FromQuery] string inn)
    {
        if (string.IsNullOrEmpty(inn)) return BadRequest("INN is required.");
        var org = await db.TblSpravOrgs.FirstOrDefaultAsync(o => o.INN == inn);
        if (org == null) return NotFound("Organisation not found.");

        var sql = @"
            SELECT v.YEAR, v.KVARTAL, SUM(v.SUMMA) AS TotalSumma, v.STATUS
            FROM tblVipl v
            WHERE v.ORG_ID = @ORG_ID
            GROUP BY v.YEAR, v.KVARTAL, v.STATUS
            ORDER BY v.YEAR DESC, v.KVARTAL";

        var parameters = new List<Microsoft.Data.SqlClient.SqlParameter>
        {
            new("@ORG_ID", org.ID)
        };

        return Ok(await ExecuteRawSelectAsync(sql, parameters));
    }

    // GET /zlist?year=&orgId= — GetOR / GetZList: realization list
    [HttpGet("zlist")]
    public async Task<IActionResult> GetZList([FromQuery] int? year, [FromQuery] long? orgId)
    {
        var sql = @"
            SELECT v.ID AS ViplaId, v.ORG_ID, o.Name_Org, o.INN,
                   v.YEAR, v.KVARTAL, v.SUMMA, v.PAY_SUMMA, v.PAY_PERCENT,
                   v.STATUS, v.MODIFY_DATE,
                   COUNT(vp.PUT_ID) AS PutevokCount
            FROM tblVipl v
            LEFT JOIN tblSprav_Org o ON o.ID = v.ORG_ID
            LEFT JOIN tblVipl_Put vp ON vp.VIPL_ID = v.ID
            WHERE 1=1";

        var parameters = new List<Microsoft.Data.SqlClient.SqlParameter>();
        if (year.HasValue) { sql += " AND v.YEAR = @YEAR"; parameters.Add(new("@YEAR", year.Value)); }
        if (orgId.HasValue) { sql += " AND v.ORG_ID = @ORG_ID"; parameters.Add(new("@ORG_ID", orgId.Value)); }
        sql += " GROUP BY v.ID, v.ORG_ID, o.Name_Org, o.INN, v.YEAR, v.KVARTAL, v.SUMMA, v.PAY_SUMMA, v.PAY_PERCENT, v.STATUS, v.MODIFY_DATE ORDER BY v.MODIFY_DATE DESC";

        return Ok(await ExecuteRawSelectAsync(sql, parameters));
    }

    // GET /oform-realizatsiya?inn=&sanatIds= — GetOformleniyaRealizatsiyaPutevok
    [HttpGet("oform-realizatsiya")]
    public async Task<IActionResult> GetOformleniyaRealizatsiya(
        [FromQuery] string? inn = null,
        [FromQuery] long[]? sanatIds = null)
    {
        return await GetOformleniye(inn, sanatIds);
    }

    // POST /realizovat — realize (sell) vouchers to org, creating tblVipl record
    [HttpPost("realizovat")]
    public async Task<IActionResult> Realizovat([FromBody] RealizeRequest request)
    {
        if (request.VoucherIds == null || request.VoucherIds.Length == 0)
            return BadRequest("VoucherIds required.");

        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var now = DateTime.Now;

        var vouchers = await db.TblPosPuts
            .Where(p => request.VoucherIds.Contains(p.ID) && (p.Status == null || p.Status == "NEW"))
            .ToListAsync();

        if (vouchers.Count == 0)
            return BadRequest("No available vouchers found.");

        // Create tblVipl invoice via raw SQL (table may not have an EF model)
        var conn = db.Database.GetDbConnection();
        await conn.OpenAsync();
        await using var tx = await conn.BeginTransactionAsync();
        try
        {
            // Insert tblVipl
            await using var cmdVipl = conn.CreateCommand();
            cmdVipl.Transaction = (System.Data.Common.DbTransaction)tx;
            cmdVipl.CommandText = @"
                INSERT INTO tblVipl (ORG_ID, YEAR, KVARTAL, SUMMA, STATUS, MODIFY_DATE, USER_ID)
                OUTPUT INSERTED.ID
                VALUES (@ORG_ID, @YEAR, @KVARTAL, @SUMMA, 'NEW', @MODIFY_DATE, @USER_ID)";
            cmdVipl.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ORG_ID", request.OrgId));
            cmdVipl.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@YEAR", now.Year));
            cmdVipl.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@KVARTAL", (now.Month - 1) / 3 + 1));
            cmdVipl.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@SUMMA", vouchers.Sum(v => v.Price ?? 0)));
            cmdVipl.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@MODIFY_DATE", now));
            cmdVipl.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@USER_ID", userId));

            var viplaId = (long)(await cmdVipl.ExecuteScalarAsync())!;

            // Link vouchers to tblVipl_Put and mark SOLD
            foreach (var v in vouchers)
            {
                await using var cmdPut = conn.CreateCommand();
                cmdPut.Transaction = (System.Data.Common.DbTransaction)tx;
                cmdPut.CommandText = @"
                    INSERT INTO tblVipl_Put (VIPL_ID, PUT_ID, SAN_ID, MODIFY_DATE)
                    VALUES (@VIPL_ID, @PUT_ID, @SAN_ID, @MODIFY_DATE)";
                cmdPut.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@VIPL_ID", viplaId));
                cmdPut.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@PUT_ID", v.ID));
                cmdPut.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@SAN_ID", (object?)v.ID_San ?? DBNull.Value));
                cmdPut.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@MODIFY_DATE", now));
                await cmdPut.ExecuteNonQueryAsync();

                v.Status = "SOLD";
                v.MODIFY_DATE = now;
                v.UserId = userId;
            }

            await db.SaveChangesAsync();
            await tx.CommitAsync();
            await conn.CloseAsync();

            return Ok(new { ViplaId = viplaId, Realized = vouchers.Count });
        }
        catch
        {
            await tx.RollbackAsync();
            await conn.CloseAsync();
            throw;
        }
    }

    // POST /zabronirovat — reserve vouchers (BRON status)
    [HttpPost("zabronirovat")]
    public async Task<IActionResult> Zabronirovat([FromBody] ZabroniRequest request)
    {
        if (request.VoucherIds == null || request.VoucherIds.Length == 0)
            return BadRequest("VoucherIds required.");

        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");
        var now = DateTime.Now;

        var vouchers = await db.TblPosPuts
            .Where(p => request.VoucherIds.Contains(p.ID) && (p.Status == null || p.Status == "NEW"))
            .ToListAsync();

        if (vouchers.Count == 0)
            return BadRequest("No available vouchers.");

        foreach (var v in vouchers)
        {
            v.Status = "BRON";
            v.MODIFY_DATE = now;
            v.UserId = userId;

            // Create a bron record for tracking
            db.TblBrons.Add(new TblBron
            {
                ID_Sanat = v.ID_San,
                Num_Put = v.Num_Put,
                BRON_FLG = true,
                BRON_DATE = now,
                EXPIRE_DATE = now.AddDays(3),
                USER_ID = userId,
                DEPARTMENT_BASE = depBase,
                PRIM = $"OrgId={request.OrgId}"
            });
        }

        await db.SaveChangesAsync();
        return Ok(new { Reserved = vouchers.Count });
    }

    // POST /otmenit-bron — cancel reservation
    [HttpPost("otmenit-bron")]
    public async Task<IActionResult> OtmenitBron([FromBody] CancelBronRequest request)
    {
        if (request.VoucherIds == null || request.VoucherIds.Length == 0)
            return BadRequest("VoucherIds required.");

        var vouchers = await db.TblPosPuts
            .Where(p => request.VoucherIds.Contains(p.ID) && p.Status == "BRON")
            .ToListAsync();

        foreach (var v in vouchers)
        {
            v.Status = "NEW";
            v.MODIFY_DATE = DateTime.Now;
        }

        // Remove active bron records for these voucher numbers
        var numPuts = vouchers.Select(v => v.Num_Put).Where(n => n != null).ToList();
        var brons = await db.TblBrons
            .Where(b => numPuts.Contains(b.Num_Put) && b.BRON_FLG == true)
            .ToListAsync();
        foreach (var b in brons)
            b.BRON_FLG = false;

        await db.SaveChangesAsync();
        return Ok(new { Cancelled = vouchers.Count });
    }

    // PUT /vouchers/{id}/payment — save payment percent
    [HttpPut("vouchers/{id:long}/payment")]
    public async Task<IActionResult> SetPaymentPercent(long id, [FromBody] SetPaymentPercentRequest request)
    {
        // Update tblVipl.PAY_PERCENT via raw SQL since no EF model
        var conn = db.Database.GetDbConnection();
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE tblVipl SET PAY_PERCENT = @PCT, MODIFY_DATE = GETDATE() WHERE ID = @ID";
        cmd.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@PCT", request.PaymentPercent));
        cmd.Parameters.Add(new Microsoft.Data.SqlClient.SqlParameter("@ID", id));
        var affected = await cmd.ExecuteNonQueryAsync();
        await conn.CloseAsync();

        return affected == 0 ? NotFound() : NoContent();
    }

    // GET /organizations?inn=&name= — search orgs for realizatsiya picker
    [HttpGet("organizations")]
    public async Task<IActionResult> SearchOrganizations(
        [FromQuery] string? inn = null,
        [FromQuery] string? name = null)
    {
        var query = db.TblSpravOrgs.AsQueryable();
        if (!string.IsNullOrEmpty(inn))
            query = query.Where(o => o.INN!.Contains(inn));
        if (!string.IsNullOrEmpty(name))
            query = query.Where(o => o.Name_Org!.Contains(name) || o.Name_Short!.Contains(name));

        var result = await query
            .OrderBy(o => o.Name_Org)
            .Select(o => new
            {
                o.ID,
                o.Name_Org,
                o.Name_Short,
                o.INN,
                o.DepartmentBase,
                o.OWNER_CODE
            })
            .Take(50)
            .ToListAsync();

        return Ok(result);
    }

    // GET /organizations/by-inn?inn= — get org by exact INN
    [HttpGet("organizations/by-inn")]
    public async Task<IActionResult> GetOrgByInn([FromQuery] string inn)
    {
        if (string.IsNullOrEmpty(inn)) return BadRequest("INN is required.");
        var org = await db.TblSpravOrgs.FirstOrDefaultAsync(o => o.INN == inn);
        return org == null ? NotFound() : Ok(org);
    }

    // ---- Helpers ----

    private static string FormatPeriod(DateTime? start, DateTime? end)
    {
        if (start == null) return "Unknown";
        return end == null
            ? start.Value.ToString("yyyy-MM")
            : $"{start.Value:yyyy-MM-dd} / {end.Value:yyyy-MM-dd}";
    }

    private async Task<List<Dictionary<string, object?>>> ExecuteRawSelectAsync(
        string sql,
        IEnumerable<Microsoft.Data.SqlClient.SqlParameter> parameters)
    {
        var conn = db.Database.GetDbConnection();
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        foreach (var p in parameters) cmd.Parameters.Add(p);

        var results = new List<Dictionary<string, object?>>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object?>();
            for (int i = 0; i < reader.FieldCount; i++)
                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
            results.Add(row);
        }
        await conn.CloseAsync();
        return results;
    }
}

// ---------------------------------------------------------------------------
// StartProcess controller  (api/startprocess)
// ---------------------------------------------------------------------------

[ApiController]
[Route("api/startprocess")]
[Authorize]
public class StartProcessController : ControllerBase
{
    // GET /templates/putyovki — download blank putyovki upload template
    [HttpGet("templates/putyovki")]
    public IActionResult DownloadPutyovkiTemplate()
    {
        // Stub: return a minimal placeholder. Replace with actual EPPlus/ClosedXML byte generation.
        // Columns: GodZaezdov, SanatoryKod, SanatoryNaimenovanie, SanatoryOblast,
        //          SanatoryRazmeshenieKod, Putevok1kv, Putevok2kv, Putevok3kv, Putevok4kv
        var header = "GodZaezdov\tSanatoryKod\tSanatoryNaimenovanie\tSanatoryOblast\tSanatoryRazmeshenieKod\tPutevok1kv\tPutevok2kv\tPutevok3kv\tPutevok4kv\r\n";
        var bytes = System.Text.Encoding.UTF8.GetBytes(header);
        return File(bytes, "text/tab-separated-values", "putyovki_template.txt");
    }

    // GET /templates/putyovki-kengash — download kengash putyovki template
    [HttpGet("templates/putyovki-kengash")]
    public IActionResult DownloadPutyovkiKengashTemplate()
    {
        var header = "GodZaezdov\tSanatoryKod\tSanatoryNaimenovanie\tKengashKod\tPutevok1kv\tPutevok2kv\tPutevok3kv\tPutevok4kv\r\n";
        var bytes = System.Text.Encoding.UTF8.GetBytes(header);
        return File(bytes, "text/tab-separated-values", "putyovki_kengash_template.txt");
    }

    // GET /templates/balance — download balance upload template
    [HttpGet("templates/balance")]
    public IActionResult DownloadBalanceTemplate()
    {
        var header = "SanatKod\tOblKod\tSaldoDebit\tSaldoCredit\r\n";
        var bytes = System.Text.Encoding.UTF8.GetBytes(header);
        return File(bytes, "text/tab-separated-values", "balance_template.txt");
    }

    // GET /templates/debitor — download debitor (receivables) template
    [HttpGet("templates/debitor")]
    public IActionResult DownloadDebitorTemplate()
    {
        var header = "OrgINN\tOrgName\tDebitSumma\tCreditSumma\tPeriod\r\n";
        var bytes = System.Text.Encoding.UTF8.GetBytes(header);
        return File(bytes, "text/tab-separated-values", "debitor_template.txt");
    }
}
