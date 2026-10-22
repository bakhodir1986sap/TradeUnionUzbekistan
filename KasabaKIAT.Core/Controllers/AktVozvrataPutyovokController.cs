using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

// ─────────────────────────────────────────────────────────────────────────────
// Inline DTOs
// ─────────────────────────────────────────────────────────────────────────────

public record CreateVozvratDto(
    int Year,
    int Quarter,
    string DepartmentBase,
    DateTime DataPost,
    string NumDoc,
    long IdOrg,
    long IdSan,
    string NumPut,
    decimal Symma
);

public record ChangeDateDto(DateTime NewDate);

public record CreateZakritiyeDto(
    int Year,
    int Quarter,
    long OrgId,
    string DepartmentBase
);

public record SaveOwnerChangesDto(
    long FaceId,
    string? NameFace,
    string? Dolgnost,
    string? Talon,
    string? NomDov,
    DateTime? DateDov,
    DateTime? SrokIst,
    string? Poluchatel
);

public record UpdateTalonDto(string Talon);

public record UpdateDoverenostDto(
    string? Poluchatel,
    string? NomDov,
    DateTime? DateDov,
    DateTime? SrokIst
);

// ─────────────────────────────────────────────────────────────────────────────
// 1. АКТ ВОЗВРАТА ПУТЕВОК  — /api/akt-vozvrata
// ─────────────────────────────────────────────────────────────────────────────

[ApiController]
[Route("api/akt-vozvrata")]
[Authorize]
public class AktVozvrataController(KasabaKIATContext db) : ControllerBase
{
    private string? DepBase => User.FindFirstValue("bkut_id");
    private bool IsSuperAdmin => User.FindFirstValue("is_super_admin") == "True";
    private long UserId => long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// GetVozvratList / GetVozvratRes — list acts for year+quarter with totals.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] int year, [FromQuery] int quarter)
    {
        var depBase = DepBase;

        var sql = @"
            SELECT v.ID,
                   v.Data_Post,
                   v.Num_Doc,
                   v.IdOrg,
                   o.Name_Org,
                   v.IdSan,
                   s.Name_San,
                   v.Num_Put,
                   v.Symma,
                   v.DepartmentBase
            FROM   tblVozvrat v
            LEFT   JOIN tblSprav_Org  o ON o.ID = v.IdOrg
            LEFT   JOIN tblSprav_San  s ON s.ID = v.IdSan
            WHERE  v.Year    = {0}
              AND  v.Quarter = {1}
              AND  ({2} = 1 OR v.DepartmentBase = {3})
            ORDER  BY v.Data_Post DESC";

        var rows = await db.Database
            .SqlQueryRaw<VozvratRow>(sql,
                year, quarter,
                IsSuperAdmin ? 1 : 0,
                depBase ?? string.Empty)
            .ToListAsync();

        var stats = new
        {
            putyovkaCount = rows.Count,
            summaNakladnix = rows.Sum(r => r.Symma)
        };

        return Ok(new { akts = rows, stats });
    }

    /// <summary>
    /// AddVozvtars / AddVozvtars2 — create a return act.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVozvratDto dto)
    {
        var sql = @"
            INSERT INTO tblVozvrat
                (Year, Quarter, DepartmentBase, Data_Post, Num_Doc,
                 IdOrg, IdSan, Num_Put, Symma, UserId, ModifyDate)
            VALUES
                ({0}, {1}, {2}, {3}, {4},
                 {5}, {6}, {7}, {8}, {9}, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

        var newId = await db.Database
            .SqlQueryRaw<long>(sql,
                dto.Year, dto.Quarter, dto.DepartmentBase,
                dto.DataPost, dto.NumDoc,
                dto.IdOrg, dto.IdSan, dto.NumPut, dto.Symma,
                UserId)
            .FirstOrDefaultAsync();

        // Mark the voucher as returned in tblVipl_Put (raw SQL — table not in EF context)
        await db.Database.ExecuteSqlRawAsync(
            "UPDATE tblVipl_Put SET StatusVozvrat = 1, DateVozvrat = GETDATE() WHERE Num_Put = {0} AND IdOrg = {1}",
            dto.NumPut, dto.IdOrg);

        return Ok(new { id = newId });
    }

    /// <summary>
    /// CancelVozvrat — delete return act.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var affected = await db.Database.ExecuteSqlRawAsync(
            "DELETE FROM tblVozvrat WHERE ID = {0}", id);

        if (affected == 0)
            return NotFound(new { message = "Акт не найден." });

        return NoContent();
    }

    /// <summary>
    /// IzmDataVozvrat — change the date of a return act.
    /// </summary>
    [HttpPut("{id}/date")]
    public async Task<IActionResult> ChangeDate(long id, [FromBody] ChangeDateDto dto)
    {
        var affected = await db.Database.ExecuteSqlRawAsync(
            "UPDATE tblVozvrat SET Data_Post = {0}, ModifyDate = GETDATE() WHERE ID = {1}",
            dto.NewDate, id);

        if (affected == 0)
            return NotFound(new { message = "Акт не найден." });

        return Ok(new { newDate = dto.NewDate });
    }

    /// <summary>
    /// ShowTotalForVozvrat — data for printing the act.
    /// </summary>
    [HttpGet("print/{id}")]
    public async Task<IActionResult> GetPrintData(long id)
    {
        var sql = @"
            SELECT v.ID,
                   v.Data_Post,
                   v.Num_Doc,
                   v.IdOrg,
                   o.Name_Org,
                   o.INN,
                   v.IdSan,
                   s.Name_San,
                   v.Num_Put,
                   v.Symma,
                   v.DepartmentBase,
                   v.Year,
                   v.Quarter
            FROM   tblVozvrat v
            LEFT   JOIN tblSprav_Org o ON o.ID = v.IdOrg
            LEFT   JOIN tblSprav_San s ON s.ID = v.IdSan
            WHERE  v.ID = {0}";

        var row = await db.Database
            .SqlQueryRaw<VozvratDetailRow>(sql, id)
            .FirstOrDefaultAsync();

        if (row == null)
            return NotFound(new { message = "Акт не найден." });

        return Ok(row);
    }

    /// <summary>
    /// GetGivenPutyovkaList — realized vouchers for an org that can be returned.
    /// </summary>
    [HttpGet("available-vouchers")]
    public async Task<IActionResult> GetAvailableVouchers([FromQuery] long orgId)
    {
        var sql = @"
            SELECT vp.ID,
                   vp.Num_Put,
                   vp.IdSan,
                   s.Name_San,
                   vp.DataRealizacii,
                   vp.Symma,
                   vp.IdFace,
                   f.Name_Face
            FROM   tblVipl_Put vp
            LEFT   JOIN tblSprav_San  s ON s.ID = vp.IdSan
            LEFT   JOIN tblSprav_Face f ON f.ID = vp.IdFace
            WHERE  vp.IdOrg = {0}
              AND  (vp.StatusVozvrat IS NULL OR vp.StatusVozvrat = 0)
            ORDER  BY vp.DataRealizacii DESC";

        var rows = await db.Database
            .SqlQueryRaw<AvailableVoucherRow>(sql, orgId)
            .ToListAsync();

        return Ok(rows);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 2. ЗАКРЫТИЕ КВАРТАЛА ПО ПУТЕВКАМ  — /api/zakritiye-kvartala
// ─────────────────────────────────────────────────────────────────────────────

[ApiController]
[Route("api/zakritiye-kvartala")]
[Authorize]
public class ZakritiyeKvartalaController(KasabaKIATContext db) : ControllerBase
{
    private string? DepBase => User.FindFirstValue("bkut_id");
    private bool IsSuperAdmin => User.FindFirstValue("is_super_admin") == "True";
    private long UserId => long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// GetZakritiyaPutyovkas — list quarters available for closing for an org.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] long orgId)
    {
        var depBase = DepBase;

        var sql = @"
            SELECT z.ID,
                   z.Year,
                   z.Quarter,
                   z.DepartmentBase,
                   z.IdOrg,
                   o.Name_Org,
                   z.DateZakr,
                   z.UserId,
                   z.IsClosed
            FROM   tblZakritiye z
            LEFT   JOIN tblSprav_Org o ON o.ID = z.IdOrg
            WHERE  z.IdOrg = {0}
              AND  ({1} = 1 OR z.DepartmentBase = {2})
            ORDER  BY z.Year DESC, z.Quarter DESC";

        var rows = await db.Database
            .SqlQueryRaw<ZakritiyeRow>(sql,
                orgId,
                IsSuperAdmin ? 1 : 0,
                depBase ?? string.Empty)
            .ToListAsync();

        return Ok(rows);
    }

    /// <summary>
    /// ZakritKvartal / ZakritKvartal2 — close a quarter for an org.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CloseQuarter([FromBody] CreateZakritiyeDto dto)
    {
        // Check if already closed
        var existsSql = @"
            SELECT COUNT(1) FROM tblZakritiye
            WHERE IdOrg = {0} AND Year = {1} AND Quarter = {2}";

        var existsRows = await db.Database
            .SqlQueryRaw<int>(existsSql, dto.OrgId, dto.Year, dto.Quarter)
            .ToListAsync();

        if (existsRows.FirstOrDefault() > 0)
            return Conflict(new { message = "Квартал уже закрыт для данной организации." });

        var insertSql = @"
            INSERT INTO tblZakritiye
                (Year, Quarter, DepartmentBase, IdOrg, DateZakr, UserId, IsClosed)
            VALUES
                ({0}, {1}, {2}, {3}, GETDATE(), {4}, 1);
            SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

        var newId = await db.Database
            .SqlQueryRaw<long>(insertSql,
                dto.Year, dto.Quarter, dto.DepartmentBase,
                dto.OrgId, UserId)
            .FirstOrDefaultAsync();

        return Ok(new { id = newId, message = "Квартал закрыт успешно." });
    }

    /// <summary>
    /// GetZakritiyaPutyovkasRes — residuals after quarter closing.
    /// </summary>
    [HttpGet("res")]
    public async Task<IActionResult> GetResiduals([FromQuery] long orgId)
    {
        var sql = @"
            SELECT vp.IdSan,
                   s.Name_San,
                   SUM(CASE WHEN vp.StatusVozvrat = 1 THEN 1 ELSE 0 END) AS VozvratCount,
                   SUM(CASE WHEN vp.StatusVozvrat IS NULL OR vp.StatusVozvrat = 0 THEN 1 ELSE 0 END) AS OstatkiCount,
                   SUM(vp.Symma) AS TotalSymma
            FROM   tblVipl_Put vp
            LEFT   JOIN tblSprav_San s ON s.ID = vp.IdSan
            WHERE  vp.IdOrg = {0}
            GROUP  BY vp.IdSan, s.Name_San";

        var rows = await db.Database
            .SqlQueryRaw<ZakritiyeResRow>(sql, orgId)
            .ToListAsync();

        return Ok(rows);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 3. ДАННЫЕ ВЛАДЕЛЬЦЕВ ПУТЕВОК  — /api/dannye-vladeltsev
// ─────────────────────────────────────────────────────────────────────────────

[ApiController]
[Route("api/dannye-vladeltsev")]
[Authorize]
public class DanniyeVladeltseController(KasabaKIATContext db) : ControllerBase
{
    private string? DepBase => User.FindFirstValue("bkut_id");
    private bool IsSuperAdmin => User.FindFirstValue("is_super_admin") == "True";
    private long UserId => long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// GetDanniyVladelsaPutyovki — owner data for vouchers in a date range.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetOwnerData(
        [FromQuery] DateTime beginDate,
        [FromQuery] DateTime endDate,
        [FromQuery] long orgId)
    {
        var depBase = DepBase;

        var sql = @"
            SELECT vp.ID,
                   vp.Num_Put,
                   vp.IdSan,
                   s.Name_San,
                   vp.IdOrg,
                   o.Name_Org,
                   vp.IdFace,
                   f.Name_Face,
                   vp.DataRealizacii,
                   vp.Symma,
                   vp.Talon,
                   vp.NomDov,
                   vp.DateDov,
                   vp.SrokIst,
                   vp.Poluchatel,
                   vp.Dolgnost,
                   vp.StatusVozvrat
            FROM   tblVipl_Put vp
            LEFT   JOIN tblSprav_San  s ON s.ID = vp.IdSan
            LEFT   JOIN tblSprav_Org  o ON o.ID = vp.IdOrg
            LEFT   JOIN tblSprav_Face f ON f.ID = vp.IdFace
            WHERE  vp.IdOrg = {0}
              AND  vp.DataRealizacii >= {1}
              AND  vp.DataRealizacii <= {2}
              AND  ({3} = 1 OR vp.DepartmentBase = {4})
            ORDER  BY vp.DataRealizacii";

        var rows = await db.Database
            .SqlQueryRaw<ViplPutRow>(sql,
                orgId, beginDate, endDate,
                IsSuperAdmin ? 1 : 0,
                depBase ?? string.Empty)
            .ToListAsync();

        return Ok(rows);
    }

    /// <summary>
    /// GetDanniyVladelsaNakladnoy — invoice-style data grouped by sanatorium.
    /// </summary>
    [HttpGet("nakladnoy")]
    public async Task<IActionResult> GetNakladnoyData(
        [FromQuery] DateTime beginDate,
        [FromQuery] DateTime endDate,
        [FromQuery] long orgId)
    {
        var sql = @"
            SELECT vp.IdSan,
                   s.Name_San,
                   COUNT(*)          AS KolPut,
                   SUM(vp.Symma)     AS TotalSymma,
                   MIN(vp.DataRealizacii) AS DateFirst,
                   MAX(vp.DataRealizacii) AS DateLast
            FROM   tblVipl_Put vp
            LEFT   JOIN tblSprav_San s ON s.ID = vp.IdSan
            WHERE  vp.IdOrg = {0}
              AND  vp.DataRealizacii >= {1}
              AND  vp.DataRealizacii <= {2}
            GROUP  BY vp.IdSan, s.Name_San
            ORDER  BY s.Name_San";

        var rows = await db.Database
            .SqlQueryRaw<NakladnoyRow>(sql, orgId, beginDate, endDate)
            .ToListAsync();

        // GetDanniyTotal — grand total
        var total = new
        {
            kolPut = rows.Sum(r => r.KolPut),
            totalSymma = rows.Sum(r => r.TotalSymma)
        };

        return Ok(new { rows, total });
    }

    /// <summary>
    /// SaveChanges / SaveChanges2 — save owner data changes for a voucher.
    /// </summary>
    [HttpPost("{id}/save")]
    public async Task<IActionResult> SaveChanges(long id, [FromBody] SaveOwnerChangesDto dto)
    {
        var sql = @"
            UPDATE tblVipl_Put
            SET    Poluchatel = {0},
                   Dolgnost   = {1},
                   Talon      = {2},
                   NomDov     = {3},
                   DateDov    = {4},
                   SrokIst    = {5},
                   ModifyDate = GETDATE(),
                   ModifyUser = {6}
            WHERE  ID = {7}";

        var affected = await db.Database.ExecuteSqlRawAsync(sql,
            dto.Poluchatel ?? (object)DBNull.Value,
            dto.Dolgnost ?? (object)DBNull.Value,
            dto.Talon ?? (object)DBNull.Value,
            dto.NomDov ?? (object)DBNull.Value,
            dto.DateDov.HasValue ? (object)dto.DateDov.Value : DBNull.Value,
            dto.SrokIst.HasValue ? (object)dto.SrokIst.Value : DBNull.Value,
            UserId,
            id);

        if (affected == 0)
            return NotFound(new { message = "Запись не найдена." });

        return Ok(new { message = "Сохранено успешно." });
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 4. ПРОСМОТР ОСТАТКОВ  — /api/prosmotr-ostatkov
// ─────────────────────────────────────────────────────────────────────────────

[ApiController]
[Route("api/prosmotr-ostatkov")]
[Authorize]
public class ProsmotrOstatkovController(KasabaKIATContext db) : ControllerBase
{
    private string? DepBase => User.FindFirstValue("bkut_id");
    private bool IsSuperAdmin => User.FindFirstValue("is_super_admin") == "True";

    /// <summary>
    /// RP / RP2 — remaining vouchers from tblPos_Put with optional filters.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetOstatki(
        [FromQuery] long? sanatId,
        [FromQuery] string? numPut,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo,
        [FromQuery] decimal? price)
    {
        var depBase = DepBase;
        var isSuperAdmin = IsSuperAdmin;

        var query = db.TblPosPuts.AsQueryable();

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DEPARTMENT_BASE == depBase);

        if (sanatId.HasValue)
            query = query.Where(p => p.ID_San == sanatId);

        if (!string.IsNullOrEmpty(numPut))
            query = query.Where(p => p.Num_Put != null && p.Num_Put.Contains(numPut));

        if (dateFrom.HasValue)
            query = query.Where(p => p.DataNach >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(p => p.DataKon <= dateTo.Value);

        if (price.HasValue)
            query = query.Where(p => p.Price == price.Value);

        // Join with sanatorium name via raw SQL shortcut on top of the filtered set
        var ids = await query.Select(p => p.ID).ToListAsync();

        if (ids.Count == 0)
            return Ok(new List<object>());

        var idList = string.Join(",", ids);

        var sqlDetail = $@"
            SELECT p.ID,
                   p.ID_San,
                   s.Name_San,
                   p.Num_Put,
                   p.DataNach,
                   p.DataKon,
                   p.Price,
                   p.Mes,
                   p.Tip,
                   p.Status,
                   p.DEPARTMENT_BASE
            FROM   tblPos_Put p
            LEFT   JOIN tblSprav_San s ON s.ID = p.ID_San
            WHERE  p.ID IN ({idList})
            ORDER  BY p.DataNach";

        var rows = await db.Database
            .SqlQueryRaw<OstatokRow>(sqlDetail)
            .ToListAsync();

        return Ok(rows);
    }

    /// <summary>
    /// GetPrixodPutevok_S / GetPrixodPutevok_C / GetPP_SUM_S — export as JSON attachment.
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> Export(
        [FromQuery] long? sanatId,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo)
    {
        var depBase = DepBase;
        var isSuperAdmin = IsSuperAdmin;

        var sql = @"
            SELECT p.ID,
                   p.ID_San,
                   s.Name_San,
                   p.Num_Put,
                   p.DataNach,
                   p.DataKon,
                   p.Price,
                   p.Mes,
                   p.Tip,
                   p.Status,
                   p.DEPARTMENT_BASE
            FROM   tblPos_Put p
            LEFT   JOIN tblSprav_San s ON s.ID = p.ID_San
            WHERE  ({0} = 1 OR p.DEPARTMENT_BASE = {1})
              AND  ({2} IS NULL OR p.ID_San    = {2})
              AND  ({3} IS NULL OR p.DataNach  >= {3})
              AND  ({4} IS NULL OR p.DataKon   <= {4})
            ORDER  BY p.DataNach";

        var rows = await db.Database
            .SqlQueryRaw<OstatokRow>(sql,
                isSuperAdmin ? 1 : 0,
                depBase ?? string.Empty,
                sanatId.HasValue ? (object)sanatId.Value : DBNull.Value,
                dateFrom.HasValue ? (object)dateFrom.Value : DBNull.Value,
                dateTo.HasValue ? (object)dateTo.Value : DBNull.Value)
            .ToListAsync();

        var json = System.Text.Json.JsonSerializer.Serialize(rows);
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);
        return File(bytes, "application/json", $"ostatki_{DateTime.Now:yyyyMMdd_HHmmss}.json");
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 5. ПРОСМОТР РЕАЛИЗОВАННЫХ ПУТЕВОК  — /api/prosmotr-realz-putevok
// ─────────────────────────────────────────────────────────────────────────────

[ApiController]
[Route("api/prosmotr-realz-putevok")]
[Authorize]
public class ProsmotrRealzPutevokController(KasabaKIATContext db) : ControllerBase
{
    private string? DepBase => User.FindFirstValue("bkut_id");
    private bool IsSuperAdmin => User.FindFirstValue("is_super_admin") == "True";
    private long UserId => long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>
    /// PRP — list of realized vouchers from tblVipl_Put.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] long? sanId = null,
        [FromQuery] string? numPut = null,
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null)
    {
        var depBase = DepBase;
        var isSuperAdmin = IsSuperAdmin;

        var listSql = @"
            SELECT vp.ID,
                   vp.Num_Put,
                   vp.IdSan,
                   s.Name_San,
                   vp.IdOrg,
                   o.Name_Org,
                   vp.DataRealizacii,
                   vp.Symma,
                   vp.Talon,
                   vp.StatusVozvrat,
                   vp.DepartmentBase
            FROM   tblVipl_Put vp
            LEFT   JOIN tblSprav_San s ON s.ID = vp.IdSan
            LEFT   JOIN tblSprav_Org o ON o.ID = vp.IdOrg
            WHERE  ({0} = 1 OR vp.DepartmentBase = {1})
              AND  ({2} IS NULL OR vp.IdSan = {2})
              AND  ({3} IS NULL OR vp.Num_Put LIKE {3})
              AND  ({4} IS NULL OR vp.DataRealizacii >= {4})
              AND  ({5} IS NULL OR vp.DataRealizacii <= {5})
            ORDER  BY vp.DataRealizacii DESC";

        var rows = await db.Database
            .SqlQueryRaw<RealzPutListRow>(listSql,
                isSuperAdmin ? 1 : 0,
                depBase ?? string.Empty,
                sanId.HasValue ? (object)sanId.Value : DBNull.Value,
                !string.IsNullOrEmpty(numPut) ? (object)$"%{numPut}%" : DBNull.Value,
                dateFrom.HasValue ? (object)dateFrom.Value : DBNull.Value,
                dateTo.HasValue ? (object)dateTo.Value : DBNull.Value)
            .ToListAsync();

        return Ok(rows);
    }

    /// <summary>
    /// PRP2 — full detail for one realized voucher (3 sections).
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetail(long id)
    {
        // Section 1: voucher info
        var voucherSql = @"
            SELECT vp.ID,
                   vp.Num_Put,
                   vp.IdSan,
                   s.Name_San,
                   vp.DataRealizacii,
                   vp.Symma,
                   vp.Talon,
                   vp.StatusVozvrat,
                   vp.DepartmentBase,
                   vp.IdPosPut
            FROM   tblVipl_Put vp
            LEFT   JOIN tblSprav_San s ON s.ID = vp.IdSan
            WHERE  vp.ID = {0}";

        var voucher = await db.Database
            .SqlQueryRaw<RealzPutDetailRow>(voucherSql, id)
            .FirstOrDefaultAsync();

        if (voucher == null)
            return NotFound(new { message = "Запись не найдена." });

        // Section 2: org and employee (owner)
        var ownerSql = @"
            SELECT vp.IdOrg,
                   o.Name_Org,
                   vp.IdFace,
                   f.Name_Face,
                   vp.Dolgnost,
                   vp.Poluchatel
            FROM   tblVipl_Put vp
            LEFT   JOIN tblSprav_Org  o ON o.ID = vp.IdOrg
            LEFT   JOIN tblSprav_Face f ON f.ID = vp.IdFace
            WHERE  vp.ID = {0}";

        var owner = await db.Database
            .SqlQueryRaw<RealzPutOwnerRow>(ownerSql, id)
            .FirstOrDefaultAsync();

        // Section 3: doverenost
        var dovSql = @"
            SELECT vp.NomDov,
                   vp.DateDov,
                   vp.SrokIst,
                   vp.Poluchatel
            FROM   tblVipl_Put vp
            WHERE  vp.ID = {0}";

        var dov = await db.Database
            .SqlQueryRaw<DoverenostRow>(dovSql, id)
            .FirstOrDefaultAsync();

        return Ok(new { voucher, owner, doverenost = dov });
    }

    /// <summary>
    /// PRP3 — history of operations on a voucher.
    /// GetPutyovkaHistory
    /// </summary>
    [HttpGet("{id}/history")]
    public async Task<IActionResult> GetHistory(long id)
    {
        var sql = @"
            SELECT h.ID,
                   h.ViplPutId,
                   h.OperType,
                   h.OperDate,
                   h.UserId,
                   h.Prim
            FROM   tblVipl_Put_History h
            WHERE  h.ViplPutId = {0}
            ORDER  BY h.OperDate DESC";

        var rows = await db.Database
            .SqlQueryRaw<PutyovkaHistoryRow>(sql, id)
            .ToListAsync();

        return Ok(rows);
    }

    /// <summary>
    /// Update talon field.
    /// </summary>
    [HttpPut("{id}/talon")]
    public async Task<IActionResult> UpdateTalon(long id, [FromBody] UpdateTalonDto dto)
    {
        var affected = await db.Database.ExecuteSqlRawAsync(
            "UPDATE tblVipl_Put SET Talon = {0}, ModifyDate = GETDATE(), ModifyUser = {1} WHERE ID = {2}",
            dto.Talon, UserId, id);

        if (affected == 0)
            return NotFound(new { message = "Запись не найдена." });

        return Ok(new { talon = dto.Talon });
    }

    /// <summary>
    /// Update doverenost fields.
    /// </summary>
    [HttpPut("{id}/doverenost")]
    public async Task<IActionResult> UpdateDoverenost(long id, [FromBody] UpdateDoverenostDto dto)
    {
        var sql = @"
            UPDATE tblVipl_Put
            SET    Poluchatel = {0},
                   NomDov     = {1},
                   DateDov    = {2},
                   SrokIst    = {3},
                   ModifyDate = GETDATE(),
                   ModifyUser = {4}
            WHERE  ID = {5}";

        var affected = await db.Database.ExecuteSqlRawAsync(sql,
            dto.Poluchatel ?? (object)DBNull.Value,
            dto.NomDov ?? (object)DBNull.Value,
            dto.DateDov.HasValue ? (object)dto.DateDov.Value : DBNull.Value,
            dto.SrokIst.HasValue ? (object)dto.SrokIst.Value : DBNull.Value,
            UserId,
            id);

        if (affected == 0)
            return NotFound(new { message = "Запись не найдена." });

        return Ok(new { message = "Доверенность обновлена." });
    }

    /// <summary>
    /// POST /{id}/vozvrat — attach a return file + reason and mark voucher as returned.
    /// </summary>
    [HttpPost("{id}/vozvrat")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateVozvrat(
        long id,
        IFormFile? file,
        [FromForm] string? prichina)
    {
        // Verify the voucher exists
        var checkSql = "SELECT COUNT(1) FROM tblVipl_Put WHERE ID = {0}";
        var countRows = await db.Database.SqlQueryRaw<int>(checkSql, id).ToListAsync();
        if (countRows.FirstOrDefault() == 0)
            return NotFound(new { message = "Путевка не найдена." });

        string? savedFileName = null;
        if (file != null && file.Length > 0)
        {
            // Save attachment to the uploads directory
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "vozvrat");
            Directory.CreateDirectory(uploadsDir);
            var ext = Path.GetExtension(file.FileName);
            savedFileName = $"vozvrat_{id}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
            var fullPath = Path.Combine(uploadsDir, savedFileName);
            await using var stream = System.IO.File.Create(fullPath);
            await file.CopyToAsync(stream);
        }

        var updateSql = @"
            UPDATE tblVipl_Put
            SET    StatusVozvrat  = 1,
                   DateVozvrat    = GETDATE(),
                   PrichinaVozvr  = {0},
                   FileVozvrat    = {1},
                   ModifyDate     = GETDATE(),
                   ModifyUser     = {2}
            WHERE  ID = {3}";

        await db.Database.ExecuteSqlRawAsync(updateSql,
            prichina ?? (object)DBNull.Value,
            savedFileName ?? (object)DBNull.Value,
            UserId,
            id);

        return Ok(new { message = "Возврат оформлен.", fileName = savedFileName });
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Raw-SQL result types (used with SqlQueryRaw — must have parameterless ctor)
// ─────────────────────────────────────────────────────────────────────────────

public class VozvratRow
{
    public long ID { get; set; }
    public DateTime? Data_Post { get; set; }
    public string? Num_Doc { get; set; }
    public long? IdOrg { get; set; }
    public string? Name_Org { get; set; }
    public long? IdSan { get; set; }
    public string? Name_San { get; set; }
    public string? Num_Put { get; set; }
    public decimal? Symma { get; set; }
    public string? DepartmentBase { get; set; }
}

public class VozvratDetailRow : VozvratRow
{
    public string? INN { get; set; }
    public int? Year { get; set; }
    public int? Quarter { get; set; }
}

public class AvailableVoucherRow
{
    public long ID { get; set; }
    public string? Num_Put { get; set; }
    public long? IdSan { get; set; }
    public string? Name_San { get; set; }
    public DateTime? DataRealizacii { get; set; }
    public decimal? Symma { get; set; }
    public long? IdFace { get; set; }
    public string? Name_Face { get; set; }
}

public class ZakritiyeRow
{
    public long ID { get; set; }
    public int? Year { get; set; }
    public int? Quarter { get; set; }
    public string? DepartmentBase { get; set; }
    public long? IdOrg { get; set; }
    public string? Name_Org { get; set; }
    public DateTime? DateZakr { get; set; }
    public long? UserId { get; set; }
    public bool? IsClosed { get; set; }
}

public class ZakritiyeResRow
{
    public long? IdSan { get; set; }
    public string? Name_San { get; set; }
    public int VozvratCount { get; set; }
    public int OstatkiCount { get; set; }
    public decimal? TotalSymma { get; set; }
}

public class ViplPutRow
{
    public long ID { get; set; }
    public string? Num_Put { get; set; }
    public long? IdSan { get; set; }
    public string? Name_San { get; set; }
    public long? IdOrg { get; set; }
    public string? Name_Org { get; set; }
    public long? IdFace { get; set; }
    public string? Name_Face { get; set; }
    public DateTime? DataRealizacii { get; set; }
    public decimal? Symma { get; set; }
    public string? Talon { get; set; }
    public string? NomDov { get; set; }
    public DateTime? DateDov { get; set; }
    public DateTime? SrokIst { get; set; }
    public string? Poluchatel { get; set; }
    public string? Dolgnost { get; set; }
    public int? StatusVozvrat { get; set; }
}

public class NakladnoyRow
{
    public long? IdSan { get; set; }
    public string? Name_San { get; set; }
    public int KolPut { get; set; }
    public decimal? TotalSymma { get; set; }
    public DateTime? DateFirst { get; set; }
    public DateTime? DateLast { get; set; }
}

public class OstatokRow
{
    public long ID { get; set; }
    public int? ID_San { get; set; }
    public string? Name_San { get; set; }
    public string? Num_Put { get; set; }
    public DateTime? DataNach { get; set; }
    public DateTime? DataKon { get; set; }
    public decimal? Price { get; set; }
    public string? Mes { get; set; }
    public string? Tip { get; set; }
    public string? Status { get; set; }
    public string? DEPARTMENT_BASE { get; set; }
}

public class RealzPutListRow
{
    public long ID { get; set; }
    public string? Num_Put { get; set; }
    public long? IdSan { get; set; }
    public string? Name_San { get; set; }
    public long? IdOrg { get; set; }
    public string? Name_Org { get; set; }
    public DateTime? DataRealizacii { get; set; }
    public decimal? Symma { get; set; }
    public string? Talon { get; set; }
    public int? StatusVozvrat { get; set; }
    public string? DepartmentBase { get; set; }
}

public class RealzPutDetailRow
{
    public long ID { get; set; }
    public string? Num_Put { get; set; }
    public long? IdSan { get; set; }
    public string? Name_San { get; set; }
    public DateTime? DataRealizacii { get; set; }
    public decimal? Symma { get; set; }
    public string? Talon { get; set; }
    public int? StatusVozvrat { get; set; }
    public string? DepartmentBase { get; set; }
    public long? IdPosPut { get; set; }
}

public class RealzPutOwnerRow
{
    public long? IdOrg { get; set; }
    public string? Name_Org { get; set; }
    public long? IdFace { get; set; }
    public string? Name_Face { get; set; }
    public string? Dolgnost { get; set; }
    public string? Poluchatel { get; set; }
}

public class DoverenostRow
{
    public string? NomDov { get; set; }
    public DateTime? DateDov { get; set; }
    public DateTime? SrokIst { get; set; }
    public string? Poluchatel { get; set; }
}

public class PutyovkaHistoryRow
{
    public long ID { get; set; }
    public long? ViplPutId { get; set; }
    public string? OperType { get; set; }
    public DateTime? OperDate { get; set; }
    public long? UserId { get; set; }
    public string? Prim { get; set; }
}
