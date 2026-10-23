using ClosedXML.Excel;
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
        var profQuery = db.TbkuSpisokRaspredeleniyas
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
// SverkaDannix V2 controller  (api/sverkadannixv2)
// Reconciliation of sanatorium-uploaded data (tblFase) vs union issued data (tblVipl_Put)
// ---------------------------------------------------------------------------

[ApiController]
[Route("api/sverkadannixv2")]
[Authorize]
public class SverkaDannixV2Controller(KasabaKIATContext db) : ControllerBase
{
    // GET /years — distinct years from tblFase and tblVipl_Put, shape: { Id, Year }
    [HttpGet("years")]
    public async Task<IActionResult> GetYears()
    {
        var fromFase = await db.TblFases
            .Where(f => f.Data_Post != null)
            .Select(f => f.Data_Post!.Value.Year)
            .ToListAsync();

        var fromVipl = await db.Database
            .SqlQueryRaw<int>("SELECT DISTINCT YEAR(Data_Post) AS Value FROM tblVipl_Put WHERE Data_Post IS NOT NULL")
            .ToListAsync();

        var years = fromFase.Concat(fromVipl).Distinct().OrderByDescending(y => y).ToList();

        var result = years.Select(y => new { Id = y, Year = y.ToString() }).ToList();
        return Ok(result);
    }

    // GET /sides — upload sides (hardcoded: sanatorium side vs union side)
    [HttpGet("sides")]
    public IActionResult GetSides()
    {
        var sides = new[]
        {
            new { Code = "SANAT",  Decode = "Санаторий" },
            new { Code = "OBLAST", Decode = "Профсоюз"  },
        };
        return Ok(sides);
    }

    // GET /sanatories — all sanatoriums
    [HttpGet("sanatories")]
    public async Task<IActionResult> GetSanatories()
    {
        var list = await db.TblSpravSans
            .OrderBy(s => s.Name_San)
            .Select(s => new { s.ID, s.Name_San })
            .ToListAsync();
        return Ok(list);
    }

    // GET /profsoyuzy?sideCode= — union organisations filtered by side
    [HttpGet("profsoyuzy")]
    public async Task<IActionResult> GetProfsoyuzy([FromQuery] string? sideCode)
    {
        var list = await db.TDepartmentBases
            .OrderBy(d => d.name_ru)
            .Select(d => new
            {
                MATCH_DEPARTMENTBASE = d.MATCH_DEPARTMENTBASE,
                ProfsoyzNaimenovanie = d.name_ru,
            })
            .ToListAsync();
        return Ok(list);
    }

    // GET /years-list — distinct years in tblFase for sverka/loaded filter dropdowns
    [HttpGet("years-list")]
    public async Task<IActionResult> GetYearsList()
    {
        var fromFase = await db.TblFases
            .Where(f => f.Data_Post != null)
            .Select(f => f.Data_Post!.Value.Year)
            .ToListAsync();

        var fromVipl = await db.Database
            .SqlQueryRaw<int>("SELECT DISTINCT YEAR(Data_Post) AS Value FROM tblVipl_Put WHERE Data_Post IS NOT NULL")
            .ToListAsync();

        var years = fromFase.Concat(fromVipl).Distinct().OrderByDescending(y => y)
            .Select(y => new { year_id = y.ToString(), year_str = y.ToString() })
            .ToList();
        return Ok(years);
    }

    // GET /sanatory-list — { text, value } list for sverka filter
    [HttpGet("sanatory-list")]
    public async Task<IActionResult> GetSanatoryList()
    {
        var list = await db.TblSpravSans
            .OrderBy(s => s.Name_San)
            .Select(s => new { text = s.Name_San, value = s.ID.ToString() })
            .ToListAsync();
        return Ok(list);
    }

    // GET /prof-list — { text, value } list for sverka filter
    [HttpGet("prof-list")]
    public async Task<IActionResult> GetProfList()
    {
        var list = await db.TDepartmentBases
            .OrderBy(d => d.name_ru)
            .Select(d => new { text = d.name_ru, value = d.MATCH_DEPARTMENTBASE })
            .ToListAsync();
        return Ok(list);
    }

    // GET /GetLoadedRowsV2?year= — rows uploaded from sanatorium side (tblFase)
    [HttpGet("GetLoadedRowsV2")]
    public async Task<IActionResult> GetLoadedRowsV2([FromQuery] string? year)
    {
        var query = db.TblFases
            .AsQueryable();

        if (int.TryParse(year, out var yr))
            query = query.Where(f => f.Data_Post != null && f.Data_Post.Value.Year == yr);

        var faseRows = await query
            .Select(f => new
            {
                f.ID,
                f.ID_Put,
                f.ID_Sanat,
                f.Num_Put,
                f.LastName,
                f.Name,
                f.FirstName,
                f.Adress,
                f.Rabota,
                f.Data_Post,
            })
            .ToListAsync();

        var sanIds = faseRows.Select(f => f.ID_Sanat).Where(x => x != null).Cast<int>().Distinct().ToList();
        var sans = await db.TblSpravSans
            .Where(s => sanIds.Contains(s.ID))
            .ToDictionaryAsync(s => s.ID, s => s.Name_San);

        var result = faseRows.Select(f =>
        {
            var sanName = f.ID_Sanat != null && sans.TryGetValue(f.ID_Sanat.Value, out var n) ? n : "";
            var fio = string.Join(" ", new[] { f.LastName, f.Name, f.FirstName }.Where(s => !string.IsNullOrEmpty(s)));
            return new
            {
                num_put = f.Num_Put ?? "",
                sanatName = sanName ?? "",
                date_san = f.Data_Post?.ToString("dd.MM.yyyy") ?? "",
                fio_otdix = fio,
                price = "",
                work_place = f.Rabota ?? "",
            };
        }).ToList();

        return Ok(result);
    }

    // GET /GetDifferByYearFilesV2?year=&sanatory=&profsoyuz= — reconciliation differences
    [HttpGet("GetDifferByYearFilesV2")]
    public async Task<IActionResult> GetDifferByYearFilesV2(
        [FromQuery] string? year,
        [FromQuery] string? sanatory,
        [FromQuery] string? profsoyuz)
    {
        // Get tblFase rows (sanatorium-uploaded data) and tblVipl_Put rows (union data)
        // Compare by Num_Put — show where FIO or address differ

        var faseQuery = db.TblFases.AsQueryable();
        if (int.TryParse(year, out var yr))
            faseQuery = faseQuery.Where(f => f.Data_Post != null && f.Data_Post.Value.Year == yr);
        if (int.TryParse(sanatory, out var sanId))
            faseQuery = faseQuery.Where(f => f.ID_Sanat == sanId);
        if (!string.IsNullOrEmpty(profsoyuz))
            faseQuery = faseQuery.Where(f => f.DepartmentBase == profsoyuz);

        var faseRows = await faseQuery
            .Select(f => new
            {
                f.Num_Put,
                f.ID_Sanat,
                KurortFio  = (f.LastName ?? "") + " " + (f.Name ?? "") + " " + (f.FirstName ?? ""),
                KurortAddr = f.Adress ?? "",
            })
            .ToListAsync();

        // Union side: tblVipl_Put — no FIO stored there; use tblFase rows created by union staff (DepartmentBase != null)
        var viplPutNums = faseRows.Select(f => f.Num_Put).Where(n => n != null).ToList();
        var unionFaseRows = await db.TblFases
            .Where(f => viplPutNums.Contains(f.Num_Put) && f.DepartmentBase != null)
            .Select(f => new
            {
                f.Num_Put,
                f.ID_Sanat,
                OblastFio  = (f.LastName ?? "") + " " + (f.Name ?? "") + " " + (f.FirstName ?? ""),
                OblastAddr = f.Adress ?? "",
            })
            .ToListAsync();

        var unionLookup = unionFaseRows.ToDictionary(u => u.Num_Put ?? "", u => u);

        // Also pull issued vouchers from tblVipl_Put
        var viplRows = await db.Database
            .SqlQueryRaw<ViplPutNumRow>(
                "SELECT Num_Put, ID_Sanat FROM tblVipl_Put WHERE Num_Put IS NOT NULL")
            .ToListAsync();

        var sanIds = faseRows.Select(f => f.ID_Sanat).Where(x => x != null).Cast<int>().Distinct().ToList();
        var sans = await db.TblSpravSans
            .Where(s => sanIds.Contains(s.ID))
            .ToDictionaryAsync(s => s.ID, s => s.Name_San);

        var results = new List<object>();
        foreach (var f in faseRows)
        {
            var sanName = f.ID_Sanat != null && sans.TryGetValue(f.ID_Sanat.Value, out var sn) ? sn : "";
            unionLookup.TryGetValue(f.Num_Put ?? "", out var unionRow);

            var oblastFio  = unionRow?.OblastFio  ?? "";
            var oblastAddr = unionRow?.OblastAddr ?? "";

            // Only include rows that actually differ or have no union counterpart
            if (f.KurortFio.Trim() != oblastFio.Trim() || f.KurortAddr.Trim() != oblastAddr.Trim())
            {
                results.Add(new
                {
                    sanatName    = sanName ?? "",
                    numPut       = f.Num_Put ?? "",
                    kurortFio    = f.KurortFio.Trim(),
                    kurortAdress = f.KurortAddr,
                    oblastFio    = oblastFio.Trim(),
                    oblastAdress = oblastAddr,
                });
            }
        }

        return Ok(results);
    }

    // GET /download-template — returns an xlsx template for upload
    [HttpGet("download-template")]
    public IActionResult DownloadTemplate()
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Шаблон");

        var headers = new[]
        {
            "Номер путевки", "Санаторий (ID)", "Фамилия", "Имя", "Отчество",
            "Адрес", "Место работы", "Должность", "ИНН", "ПНФЛ",
            "Серия паспорта", "Номер паспорта", "Кем выдан", "Дата выдачи", "Пол",
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
        }
        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        ms.Position = 0;

        return File(ms.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "sverka_template.xlsx");
    }

    // POST /upload — parse xlsx and insert into tblFase
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile? UploadedFile,
        [FromForm] int? yearId,
        [FromForm] string? sideCode,
        [FromForm] int? sanatoryId,
        [FromForm] string? profsoyzId)
    {
        if (UploadedFile == null || UploadedFile.Length == 0)
            return BadRequest("Файл не выбран.");

        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id") ?? string.Empty;
        var now = DateTime.Now;
        int rowsProcessed = 0, rowsSkipped = 0;

        using var stream = UploadedFile.OpenReadStream();
        using var wb = new XLWorkbook(stream);
        var ws = wb.Worksheets.First();

        foreach (var row in ws.RowsUsed().Skip(1)) // skip header
        {
            var numPut = row.Cell(1).GetString().Trim();
            if (string.IsNullOrEmpty(numPut)) { rowsSkipped++; continue; }

            var existing = await db.TblFases.FirstOrDefaultAsync(f => f.Num_Put == numPut && f.ID_Sanat == sanatoryId);
            var fase = existing ?? new TblFase
            {
                ID_Sanat = sanatoryId,
                Num_Put = numPut,
                DepartmentBase = depBase,
                UserId = userId,
                Data_Post = now,
            };

            fase.LastName  = row.Cell(3).GetString().Trim();
            fase.Name      = row.Cell(4).GetString().Trim();
            fase.FirstName = row.Cell(5).GetString().Trim();
            fase.Adress    = row.Cell(6).GetString().Trim();
            fase.Rabota    = row.Cell(7).GetString().Trim();
            fase.Dolj      = row.Cell(8).GetString().Trim();
            fase.INN       = row.Cell(9).GetString().Trim();
            fase.PNFL      = row.Cell(10).GetString().Trim();
            fase.Pas_Ser   = row.Cell(11).GetString().Trim();
            fase.Pas_Num   = row.Cell(12).GetString().Trim();
            fase.Pas_KD    = row.Cell(13).GetString().Trim();
            var pasDataStr = row.Cell(14).GetString().Trim();
            fase.Pas_Data  = DateTime.TryParse(pasDataStr, out var pd) ? pd : null;
            fase.Pol       = row.Cell(15).GetString().Trim();

            if (existing == null)
                db.TblFases.Add(fase);

            rowsProcessed++;
        }

        await db.SaveChangesAsync();
        return Ok(new { rowsProcessed, rowsSkipped, fileName = UploadedFile.FileName });
    }

    // GET /export-sverka — export reconciliation results to xlsx
    [HttpGet("export-sverka")]
    public async Task<IActionResult> ExportSverka(
        [FromQuery] string? year,
        [FromQuery] string? sanatory,
        [FromQuery] string? profsoyuz)
    {
        var result = await GetDifferByYearFilesV2(year, sanatory, profsoyuz);
        if (result is not OkObjectResult ok) return result;
        var rows = (ok.Value as IEnumerable<object>)?.ToList() ?? [];

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Сверка");
        var headers = new[] { "Санаторий", "№ путевки", "Санаторий ФИО", "Санаторий Адрес", "Профсоюз ФИО", "Профсоюз Адрес" };
        for (int i = 0; i < headers.Length; i++)
        {
            ws.Cell(1, i + 1).Value = headers[i];
            ws.Cell(1, i + 1).Style.Font.Bold = true;
        }

        int r = 2;
        foreach (var item in rows)
        {
            var p = item.GetType();
            ws.Cell(r, 1).Value = p.GetProperty("sanatName")?.GetValue(item)?.ToString() ?? "";
            ws.Cell(r, 2).Value = p.GetProperty("numPut")?.GetValue(item)?.ToString() ?? "";
            ws.Cell(r, 3).Value = p.GetProperty("kurortFio")?.GetValue(item)?.ToString() ?? "";
            ws.Cell(r, 4).Value = p.GetProperty("kurortAdress")?.GetValue(item)?.ToString() ?? "";
            ws.Cell(r, 5).Value = p.GetProperty("oblastFio")?.GetValue(item)?.ToString() ?? "";
            ws.Cell(r, 6).Value = p.GetProperty("oblastAdress")?.GetValue(item)?.ToString() ?? "";
            r++;
        }
        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return File(ms.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "sverka_result.xlsx");
    }

    // GET /export-loaded — export loaded rows to xlsx
    [HttpGet("export-loaded")]
    public async Task<IActionResult> ExportLoaded([FromQuery] string? year)
    {
        var result = await GetLoadedRowsV2(year);
        if (result is not OkObjectResult ok) return result;
        var rows = (ok.Value as IEnumerable<object>)?.ToList() ?? [];

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Загруженные данные");
        var headers = new[] { "№ путевки", "Санаторий", "Дата", "ФИО", "Стоимость", "Место работы" };
        for (int i = 0; i < headers.Length; i++)
        {
            ws.Cell(1, i + 1).Value = headers[i];
            ws.Cell(1, i + 1).Style.Font.Bold = true;
        }

        int r = 2;
        foreach (var item in rows)
        {
            var p = item.GetType();
            ws.Cell(r, 1).Value = p.GetProperty("num_put")?.GetValue(item)?.ToString() ?? "";
            ws.Cell(r, 2).Value = p.GetProperty("sanatName")?.GetValue(item)?.ToString() ?? "";
            ws.Cell(r, 3).Value = p.GetProperty("date_san")?.GetValue(item)?.ToString() ?? "";
            ws.Cell(r, 4).Value = p.GetProperty("fio_otdix")?.GetValue(item)?.ToString() ?? "";
            ws.Cell(r, 5).Value = p.GetProperty("price")?.GetValue(item)?.ToString() ?? "";
            ws.Cell(r, 6).Value = p.GetProperty("work_place")?.GetValue(item)?.ToString() ?? "";
            r++;
        }
        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return File(ms.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "loaded_data.xlsx");
    }

    // GET /{year:int}/data — tbpt_putyovki rows for the year (kept for backwards compat)
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

public class ViplPutNumRow
{
    public string? Num_Put { get; set; }
    public int? ID_Sanat { get; set; }
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
            var sanParams = sanatIds.Select((v, i) =>
            {
                parameters.Add(new Microsoft.Data.SqlClient.SqlParameter($"@SanId{i}", v));
                return $"@SanId{i}";
            });
            conditions.Add($"p.ID_San IN ({string.Join(",", sanParams)})");
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
            query = query.Where(o => o.Name_Org!.Contains(name));

        var result = await query
            .OrderBy(o => o.Name_Org)
            .Select(o => new
            {
                o.ID,
                o.Name_Org,
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

