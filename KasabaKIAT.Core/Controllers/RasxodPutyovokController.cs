using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

// ──────────────────────────────────────────────────────────────────────────────
// DTOs
// ──────────────────────────────────────────────────────────────────────────────

public record ViplMainRow(
    long Id,
    int? Year,
    long? Quarter,
    string? DepartmentBase,
    long? UserId,
    string? NumDoc,
    DateTime? DataPost,
    long? IdOrg,
    string? OrgName,
    int? NumKol,
    decimal? Symma,
    string? NameDov
);

public class ViplMainFRow
{
    public long Id { get; set; }
    public int? Year { get; set; }
    public long? Quarter { get; set; }
    public string? DepartmentBase { get; set; }
    public long? UserId { get; set; }
    public string? NumDoc { get; set; }
    public DateTime? DataPost { get; set; }
    public long? FizLico { get; set; }
    public string? FizLicoName { get; set; }
    public string? FizLicoInn { get; set; }
    public int? NumKol { get; set; }
    public decimal? Symma { get; set; }
    public string? NameDov { get; set; }
}

public class ViplDetailRow
{
    public long Id { get; set; }
    public long? IdVipl { get; set; }
    public int? RecId { get; set; }
    public string? SerialBegin { get; set; }
    public string? SerialEnd { get; set; }
    public DateTime? RestBeginDate { get; set; }
    public DateTime? RestEndDate { get; set; }
    public decimal? Price { get; set; }
    public string? StatusPutyovki { get; set; }
    public int? SanatoriumId { get; set; }
    public string? SanatoriumName { get; set; }
}

public record ViplStatsRow(
    int? Year,
    long? Quarter,
    int? TotalVouchers,
    decimal? TotalAmount,
    int? TotalNakladnoys
);

public record CreateViplMainRequest(
    int Year,
    int Quarter,
    string? NumDoc,
    DateTime? DataPost,
    long IdOrg,
    string? NameDov
);

public record UpdateViplMainRequest(
    string? NumDoc,
    DateTime? DataPost,
    long IdOrg,
    string? NameDov
);

public record CreateViplMainFRequest(
    int Year,
    int Quarter,
    string? NumDoc,
    DateTime? DataPost,
    long FizLico,
    string? NameDov
);

public record UpdateViplMainFRequest(
    string? NumDoc,
    DateTime? DataPost,
    long FizLico,
    string? NameDov
);

public record AddVouchersRequest(long[] VoucherIds);

public record SchetFakturaRow(
    long Id,
    string? NumDoc,
    DateTime? DataPost,
    string? OrgName,
    string? OrgInn,
    string? NaKogo,
    string? NumDov,
    DateTime? DataDov,
    int TotalVouchers,
    decimal TotalAmount,
    List<ViplDetailRow> Lines
);

// ──────────────────────────────────────────────────────────────────────────────
// Controller: org distribution (tblVipl)
// ──────────────────────────────────────────────────────────────────────────────

[ApiController]
[Route("api/rasxod-putevok")]
[Authorize]
public class RasxodPutyovokController(KasabaKIATContext db) : ControllerBase
{
    // GET /api/rasxod-putevok?year={}&quarter={}&otraslId={}
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int? year,
        [FromQuery] int? quarter,
        [FromQuery] long? otraslId)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        // Build dynamic WHERE clauses
        var conditions = new List<string>();
        var parameters = new List<object>();
        int pIdx = 0;

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
        {
            conditions.Add($"v.DepartmentBase = {{{pIdx++}}}");
            parameters.Add(depBase);
        }
        if (year.HasValue)
        {
            conditions.Add($"v.SmFil = {{{pIdx++}}}");
            parameters.Add(year.Value);
        }
        if (quarter.HasValue)
        {
            conditions.Add($"v.Id_Kv = {{{pIdx++}}}");
            parameters.Add(quarter.Value);
        }
        if (otraslId.HasValue)
        {
            conditions.Add($"o.ID_Ot = {{{pIdx++}}}");
            parameters.Add(otraslId.Value);
        }

        var whereClause = conditions.Count > 0
            ? "WHERE " + string.Join(" AND ", conditions)
            : string.Empty;

        var sql = $@"
            SELECT
                v.ID         AS Id,
                v.SmFil      AS Year,
                v.Id_Kv      AS Quarter,
                v.DepartmentBase,
                v.UserId,
                v.Num_Doc    AS NumDoc,
                v.Data_Post  AS DataPost,
                v.ID_Org     AS IdOrg,
                o.Name_Org   AS OrgName,
                v.Num_Kol    AS NumKol,
                v.Symma,
                v.Name_Dov   AS NameDov
            FROM tblVipl v
            LEFT JOIN tblSprav_Org o ON o.ID = v.ID_Org
            {whereClause}
            ORDER BY v.Data_Post DESC";

        var rows = db.Database.SqlQueryRaw<ViplMainRow>(sql, parameters.ToArray());
        var nakladnoys = await rows.ToListAsync();

        // Stats aggregate
        var totalVouchers = 0;
        decimal totalAmount = 0;
        if (nakladnoys.Count > 0)
        {
            var ids = string.Join(",", nakladnoys.Select(r => r.Id));
            var statsSql = $@"
                SELECT
                    COUNT(vp.ID)   AS TotalVouchers,
                    SUM(vp.Symma)  AS TotalAmount
                FROM tblVipl_Put vp
                WHERE vp.ID_Vipl IN ({ids})";
            var statsRows = await db.Database.SqlQueryRaw<VoucherAggregate>(statsSql).ToListAsync();
            if (statsRows.Count > 0)
            {
                totalVouchers = statsRows[0].TotalVouchers ?? 0;
                totalAmount = statsRows[0].TotalAmount ?? 0;
            }
        }

        return Ok(new
        {
            nakladnoys,
            stats = new
            {
                totalNakladnoys = nakladnoys.Count,
                totalVouchers,
                totalAmount
            }
        });
    }

    // GET /api/rasxod-putevok/{id}/details
    [HttpGet("{id:long}/details")]
    public async Task<IActionResult> GetDetails(long id)
    {
        var sql = @"
            SELECT
                vp.ID                           AS Id,
                vp.ID_Vipl                      AS IdVipl,
                vp.ID_Sanat                     AS RecId,
                vp.Num_Put                      AS SerialBegin,
                CAST(NULL AS NVARCHAR(100))     AS SerialEnd,
                vp.Data_Post                    AS RestBeginDate,
                CAST(NULL AS DATETIME)          AS RestEndDate,
                vp.Symma                        AS Price,
                CAST(NULL AS NVARCHAR(50))      AS StatusPutyovki,
                vp.ID_Sanat                     AS SanatoriumId,
                s.Name_San                      AS SanatoriumName
            FROM tblVipl_Put vp
            LEFT JOIN tblSprav_San s ON s.ID = vp.ID_Sanat
            WHERE vp.ID_Vipl = {0}
            ORDER BY vp.ID";

        var rows = await db.Database.SqlQueryRaw<ViplDetailRow>(sql, id).ToListAsync();
        return Ok(rows);
    }

    // GET /api/rasxod-putevok/stats?year={}&quarter={}&otraslId={}
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(
        [FromQuery] int? year,
        [FromQuery] int? quarter,
        [FromQuery] long? otraslId)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var conditions = new List<string>();
        var parameters = new List<object>();
        int pIdx = 0;

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
        {
            conditions.Add($"v.DepartmentBase = {{{pIdx++}}}");
            parameters.Add(depBase);
        }
        if (year.HasValue)
        {
            conditions.Add($"YEAR(v.Data_Post) = {{{pIdx++}}}");
            parameters.Add(year.Value);
        }
        if (quarter.HasValue)
        {
            conditions.Add($"v.Id_Kv = {{{pIdx++}}}");
            parameters.Add(quarter.Value);
        }
        if (otraslId.HasValue)
        {
            conditions.Add($"o.ID_Ot = {{{pIdx++}}}");
            parameters.Add(otraslId.Value);
        }

        var whereClause = conditions.Count > 0
            ? "WHERE " + string.Join(" AND ", conditions)
            : string.Empty;

        var sql = $@"
            SELECT
                v.SmFil              AS Year,
                v.ID_Kv              AS Quarter,
                COUNT(DISTINCT v.ID) AS TotalNakladnoys,
                COUNT(vp.ID)         AS TotalVouchers,
                SUM(vp.Symma)        AS TotalAmount
            FROM tblVipl v
            LEFT JOIN tblSprav_Org o ON o.ID = v.ID_Org
            LEFT JOIN tblVipl_Put vp  ON vp.ID_Vipl = v.ID
            {whereClause}
            GROUP BY v.SmFil, v.ID_Kv
            ORDER BY v.SmFil DESC, v.ID_Kv DESC";

        var rows = await db.Database.SqlQueryRaw<ViplStatsExtRow>(sql, parameters.ToArray()).ToListAsync();
        return Ok(rows);
    }

    // POST /api/rasxod-putevok
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateViplMainRequest req)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        var idRow = await db.Database
            .SqlQueryRaw<IdResult>(@"
                INSERT INTO tblVipl
                    (SmFil, Id_Kv, DepartmentBase, UserId, Num_Doc, Data_Post, ID_Org, Name_Dov)
                OUTPUT INSERTED.ID AS Id
                VALUES
                    ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                req.Year, req.Quarter, depBase, userId,
                req.NumDoc, req.DataPost ?? DateTime.Now,
                req.IdOrg, req.NameDov)
            .ToListAsync();
        var newId = idRow.FirstOrDefault()?.Id ?? 0;

        return Ok(new { id = newId });
    }

    // PUT /api/rasxod-putevok/{id}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateViplMainRequest req)
    {
        var affected = await db.Database.ExecuteSqlRawAsync(@"
            UPDATE tblVipl
            SET Num_Doc   = {0},
                Data_Post = {1},
                ID_Org    = {2},
                Name_Dov  = {3}
            WHERE ID = {4}",
            req.NumDoc, req.DataPost, req.IdOrg, req.NameDov, id);

        if (affected == 0) return NotFound();
        return NoContent();
    }

    // DELETE /api/rasxod-putevok/{id}
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        // Remove linked vouchers first
        await db.Database.ExecuteSqlRawAsync(
            "DELETE FROM tblVipl_Put WHERE ID_Vipl = {0}", id);

        var affected = await db.Database.ExecuteSqlRawAsync(
            "DELETE FROM tblVipl WHERE ID = {0}", id);

        if (affected == 0) return NotFound();
        return NoContent();
    }

    // POST /api/rasxod-putevok/{id}/provodka
    [HttpPost("{id:long}/provodka")]
    public async Task<IActionResult> Provodka(long id)
    {
        var headerCheck = await db.Database
            .SqlQueryRaw<IdResult>("SELECT ID AS Id FROM tblVipl WHERE ID = {0}", id)
            .ToListAsync();
        if (headerCheck.Count == 0) return NotFound();

        return Ok(new { id, status = "CONDUCTED" });
    }

    // GET /api/rasxod-putevok/available-vouchers?search={}
    [HttpGet("available-vouchers")]
    public async Task<IActionResult> GetAvailableVouchers([FromQuery] string? search)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var conditions = new List<string> { "(pp.ID_PolVoz = 0 OR pp.ID_PolVoz IS NULL)" };
        var parameters = new List<object>();
        int pIdx = 0;

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
        {
            conditions.Add($"pp.Departmentbase = {{{pIdx++}}}");
            parameters.Add(depBase);
        }

        if (!string.IsNullOrEmpty(search))
        {
            conditions.Add($"pp.Num_Put LIKE {{{pIdx++}}}");
            parameters.Add($"%{search}%");
        }

        var whereClause = "WHERE " + string.Join(" AND ", conditions);

        var sql = $@"
            SELECT
                pp.ID         AS Id,
                pp.ID_Sanat   AS SanatoriumId,
                s.Name_San    AS SanatoriumName,
                pp.Num_Put    AS NumPut,
                pp.DataSan    AS DataSan,
                pp.DataSanEnd AS DataSanEnd,
                pp.Symma_Put  AS SymmaPut
            FROM tblPos_Put pp
            LEFT JOIN tblSprav_San s ON s.ID = pp.ID_Sanat
            {whereClause}
            ORDER BY pp.DataSan";

        var items = await db.Database
            .SqlQueryRaw<AvailablePrixodRow>(sql, parameters.ToArray())
            .ToListAsync();

        return Ok(items);
    }

    // DELETE /api/rasxod-putevok/{id}/vouchers/{voucherId}
    [HttpDelete("{id:long}/vouchers/{voucherId:long}")]
    public async Task<IActionResult> RemoveVoucher(long id, long voucherId)
    {
        // Look up the tblPos_Put ID for this detail row so we can un-mark it
        var detailRows = await db.Database
            .SqlQueryRaw<PrixodPutLookupRow>(
                "SELECT vp.ID AS Id, pp.ID AS PosPutId FROM tblVipl_Put vp LEFT JOIN tblPos_Put pp ON pp.Num_Put = vp.Num_Put AND pp.ID_Sanat = vp.ID_Sanat WHERE vp.ID = {0} AND vp.ID_Vipl = {1}",
                voucherId, id)
            .ToListAsync();

        if (detailRows.Count == 0) return NotFound();
        var row = detailRows[0];

        // Delete the detail line
        var affected = await db.Database.ExecuteSqlRawAsync(
            "DELETE FROM tblVipl_Put WHERE ID = {0} AND ID_Vipl = {1}", voucherId, id);

        if (affected == 0) return NotFound();

        // Un-mark the source Prixod voucher as available again
        if (row.PosPutId.HasValue)
        {
            await db.Database.ExecuteSqlRawAsync(
                "UPDATE tblPos_Put SET ID_PolVoz = 0 WHERE ID = {0}", row.PosPutId.Value);
        }

        return NoContent();
    }

    // POST /api/rasxod-putevok/{id}/add-vouchers
    [HttpPost("{id:long}/add-vouchers")]
    public async Task<IActionResult> AddVouchers(long id, [FromBody] AddVouchersRequest req)
    {
        if (req.VoucherIds == null || req.VoucherIds.Length == 0)
            return BadRequest(new { message = "voucherIds must not be empty" });

        // Verify the header exists
        var headerCheck = await db.Database
            .SqlQueryRaw<IdResult>("SELECT ID AS Id FROM tblVipl WHERE ID = {0}", id)
            .ToListAsync();
        if (headerCheck.Count == 0) return NotFound(new { message = "Distribution invoice not found" });

        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        var inserted = 0;
        foreach (var posPutId in req.VoucherIds)
        {
            // Look up the tblPos_Put record (individual Prixod line)
            var prixodRows = await db.Database
                .SqlQueryRaw<PrixodPutRow>(
                    "SELECT ID, ID_Sanat AS IdSanat, Num_Put AS NumPut, Symma_Put AS SymmaPut FROM tblPos_Put WHERE ID = {0} AND (ID_PolVoz = 0 OR ID_PolVoz IS NULL)",
                    posPutId)
                .ToListAsync();

            if (prixodRows.Count == 0) continue;
            var pp = prixodRows[0];

            // Skip if already linked to this nakladnoy (match by sanatorium + serial)
            var exists = await db.Database
                .SqlQueryRaw<CountResult>(
                    "SELECT COUNT(1) AS Count FROM tblVipl_Put WHERE ID_Vipl = {0} AND ID_Sanat = {1} AND Num_Put = {2}",
                    id, pp.IdSanat as object ?? DBNull.Value, pp.NumPut as object ?? DBNull.Value)
                .ToListAsync();

            if (exists.FirstOrDefault()?.Count > 0) continue;

            // Insert into tblVipl_Put using the real sanatorium ID and serial number
            await db.Database.ExecuteSqlRawAsync(@"
                INSERT INTO tblVipl_Put
                    (ID_Vipl, ID_Sanat, Num_Put, Num_Proc, Symma, ID_PolVoz, Data_Post, UserId, DepartmentBase)
                VALUES
                    ({0}, {1}, {2}, 100, {3}, 0, GETDATE(), {4}, {5})",
                id,
                pp.IdSanat as object ?? DBNull.Value,
                pp.NumPut as object ?? DBNull.Value,
                pp.SymmaPut as object ?? DBNull.Value,
                userId,
                depBase as object ?? DBNull.Value);

            // Mark the source Prixod record as used
            await db.Database.ExecuteSqlRawAsync(
                "UPDATE tblPos_Put SET ID_PolVoz = 1 WHERE ID = {0}",
                posPutId);

            inserted++;
        }

        return Ok(new { id, inserted });
    }
}

// ──────────────────────────────────────────────────────────────────────────────
// Controller: physical-person distribution (tblViplF)
// ──────────────────────────────────────────────────────────────────────────────

[ApiController]
[Route("api/rasxod-putevok-fiz")]
[Authorize]
public class RasxodPutyovokFizController(KasabaKIATContext db) : ControllerBase
{
    // GET /api/rasxod-putevok-fiz?year={}&quarter={}
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int? year,
        [FromQuery] int? quarter)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var conditions = new List<string>();
        var parameters = new List<object>();
        int pIdx = 0;

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
        {
            conditions.Add($"v.DepartmentBase = {{{pIdx++}}}");
            parameters.Add(depBase);
        }
        if (year.HasValue)
        {
            conditions.Add($"v.SmFil = {{{pIdx++}}}");
            parameters.Add(year.Value);
        }
        if (quarter.HasValue)
        {
            conditions.Add($"v.Id_Kv = {{{pIdx++}}}");
            parameters.Add(quarter.Value);
        }

        var whereClause = conditions.Count > 0
            ? "WHERE " + string.Join(" AND ", conditions)
            : string.Empty;

        var sql = $@"
            SELECT
                v.ID                                        AS Id,
                v.SmFil                                     AS Year,
                v.ID_Kv                                     AS Quarter,
                v.DepartmentBase,
                v.UserId,
                v.Num_Doc                                   AS NumDoc,
                v.Data_Post                                 AS DataPost,
                v.ID_Org                                    AS FizLico,
                CAST(o.Name_Org  AS NVARCHAR(200))          AS FizLicoName,
                CAST(o.INN       AS NVARCHAR(20))           AS FizLicoInn,
                v.Num_Kol                                   AS NumKol,
                v.Symma,
                v.Name_Dov                                  AS NameDov
            FROM tblViplF v
            LEFT JOIN tblSprav_Org o ON o.ID = v.ID_Org
            {whereClause}
            ORDER BY v.Data_Post DESC";

        var rows = await db.Database.SqlQueryRaw<ViplMainFRow>(sql, parameters.ToArray()).ToListAsync();

        var totalNakladnoy = rows.Count;
        var totalSumma     = rows.Sum(r => r.Symma ?? 0);
        var totalVouchers  = rows.Sum(r => r.NumKol ?? 0);
        var provedeno      = 0;

        var nakladnoys = rows.Select(r => new
        {
            id          = r.Id,
            nomer       = r.NumDoc,
            data        = r.DataPost,
            fizLicoId   = r.FizLico,
            fizLicoFio  = r.FizLicoName,
            fizLicoInn  = r.FizLicoInn,
            summa       = r.Symma,
            kolvo       = r.NumKol,
            status      = r.NameDov,
            provodka    = false,
        });

        return Ok(new
        {
            nakladnoys,
            stats = new
            {
                totalNakladnoy,
                totalSumma,
                totalVouchers,
                provedeno,
            }
        });
    }

    // GET /api/rasxod-putevok-fiz/{id}/details
    [HttpGet("{id:long}/details")]
    public async Task<IActionResult> GetDetails(long id)
    {
        var sql = @"
            SELECT
                vp.ID                           AS Id,
                vp.ID_Vipl                      AS IdVipl,
                vp.ID_Sanat                     AS RecId,
                vp.Num_Put                      AS SerialBegin,
                CAST(NULL AS NVARCHAR(100))     AS SerialEnd,
                vp.Data_Post                    AS RestBeginDate,
                CAST(NULL AS DATETIME)          AS RestEndDate,
                vp.Symma                        AS Price,
                CAST(NULL AS NVARCHAR(50))      AS StatusPutyovki,
                vp.ID_Sanat                     AS SanatoriumId,
                s.Name_San                      AS SanatoriumName
            FROM tblVipl_Put vp
            LEFT JOIN tblSprav_San s ON s.ID = vp.ID_Sanat
            WHERE vp.ID_Vipl = {0}
            ORDER BY vp.ID";

        var rows = await db.Database.SqlQueryRaw<ViplDetailRow>(sql, id).ToListAsync();
        return Ok(rows);
    }

    // POST /api/rasxod-putevok-fiz
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateViplMainFRequest req)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        var idRow = await db.Database
            .SqlQueryRaw<IdResult>(@"
                INSERT INTO tblViplF
                    (SmFil, ID_Kv, DepartmentBase, UserId, Num_Doc, Data_Post, ID_Org, Name_Dov)
                OUTPUT INSERTED.ID AS Id
                VALUES
                    ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                req.Year, req.Quarter, depBase, userId,
                req.NumDoc, req.DataPost ?? DateTime.Now,
                req.FizLico, req.NameDov)
            .ToListAsync();
        var newId = idRow.FirstOrDefault()?.Id ?? 0;

        return Ok(new { id = newId });
    }

    // PUT /api/rasxod-putevok-fiz/{id}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateViplMainFRequest req)
    {
        var affected = await db.Database.ExecuteSqlRawAsync(@"
            UPDATE tblViplF
            SET Num_Doc   = {0},
                Data_Post = {1},
                ID_Org    = {2},
                Name_Dov  = {3}
            WHERE ID = {4}",
            req.NumDoc, req.DataPost, req.FizLico, req.NameDov, id);

        if (affected == 0) return NotFound();
        return NoContent();
    }

    // DELETE /api/rasxod-putevok-fiz/{id}
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await db.Database.ExecuteSqlRawAsync(
            "DELETE FROM tblVipl_Put WHERE ID_Vipl = {0}", id);

        var affected = await db.Database.ExecuteSqlRawAsync(
            "DELETE FROM tblViplF WHERE ID = {0}", id);

        if (affected == 0) return NotFound();
        return NoContent();
    }

    // POST /api/rasxod-putevok-fiz/{id}/provodka
    [HttpPost("{id:long}/provodka")]
    public async Task<IActionResult> Provodka(long id)
    {
        var headerCheck = await db.Database
            .SqlQueryRaw<IdResult>("SELECT ID AS Id FROM tblViplF WHERE ID = {0}", id)
            .ToListAsync();
        if (headerCheck.Count == 0) return NotFound();

        return Ok(new { id, status = "CONDUCTED" });
    }

    // POST /api/rasxod-putevok-fiz/{id}/add-vouchers
    [HttpPost("{id:long}/add-vouchers")]
    public async Task<IActionResult> AddVouchers(long id, [FromBody] AddVouchersRequest req)
    {
        if (req.VoucherIds == null || req.VoucherIds.Length == 0)
            return BadRequest(new { message = "voucherIds must not be empty" });

        var headerCheck = await db.Database
            .SqlQueryRaw<IdResult>("SELECT ID AS Id FROM tblViplF WHERE ID = {0}", id)
            .ToListAsync();
        if (headerCheck.Count == 0) return NotFound(new { message = "Distribution invoice not found" });

        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        var inserted = 0;
        foreach (var posPutId in req.VoucherIds)
        {
            // Look up the tblPos_Put record (individual Prixod line)
            var prixodRows = await db.Database
                .SqlQueryRaw<PrixodPutRow>(
                    "SELECT ID, ID_Sanat AS IdSanat, Num_Put AS NumPut, Symma_Put AS SymmaPut FROM tblPos_Put WHERE ID = {0} AND (ID_PolVoz = 0 OR ID_PolVoz IS NULL)",
                    posPutId)
                .ToListAsync();

            if (prixodRows.Count == 0) continue;
            var pp = prixodRows[0];

            // Skip if already linked to this nakladnoy
            var exists = await db.Database
                .SqlQueryRaw<CountResult>(
                    "SELECT COUNT(1) AS Count FROM tblVipl_Put WHERE ID_Vipl = {0} AND ID_Sanat = {1} AND Num_Put = {2}",
                    id, pp.IdSanat as object ?? DBNull.Value, pp.NumPut as object ?? DBNull.Value)
                .ToListAsync();

            if (exists.FirstOrDefault()?.Count > 0) continue;

            await db.Database.ExecuteSqlRawAsync(@"
                INSERT INTO tblVipl_Put
                    (ID_Vipl, ID_Sanat, Num_Put, Num_Proc, Symma, ID_PolVoz, Data_Post, UserId, DepartmentBase)
                VALUES
                    ({0}, {1}, {2}, 100, {3}, 0, GETDATE(), {4}, {5})",
                id,
                pp.IdSanat as object ?? DBNull.Value,
                pp.NumPut as object ?? DBNull.Value,
                pp.SymmaPut as object ?? DBNull.Value,
                userId,
                depBase as object ?? DBNull.Value);

            // Mark the source Prixod record as used
            await db.Database.ExecuteSqlRawAsync(
                "UPDATE tblPos_Put SET ID_PolVoz = 1 WHERE ID = {0}",
                posPutId);

            inserted++;
        }

        return Ok(new { id, inserted });
    }

    // DELETE /api/rasxod-putevok-fiz/{id}/vouchers/{voucherId}
    [HttpDelete("{id:long}/vouchers/{voucherId:long}")]
    public async Task<IActionResult> RemoveVoucher(long id, long voucherId)
    {
        var detailRows = await db.Database
            .SqlQueryRaw<PrixodPutLookupRow>(
                "SELECT vp.ID AS Id, pp.ID AS PosPutId FROM tblVipl_Put vp LEFT JOIN tblPos_Put pp ON pp.Num_Put = vp.Num_Put AND pp.ID_Sanat = vp.ID_Sanat WHERE vp.ID = {0} AND vp.ID_Vipl = {1}",
                voucherId, id)
            .ToListAsync();

        if (detailRows.Count == 0) return NotFound();
        var row = detailRows[0];

        var affected = await db.Database.ExecuteSqlRawAsync(
            "DELETE FROM tblVipl_Put WHERE ID = {0} AND ID_Vipl = {1}", voucherId, id);

        if (affected == 0) return NotFound();

        if (row.PosPutId.HasValue)
        {
            await db.Database.ExecuteSqlRawAsync(
                "UPDATE tblPos_Put SET ID_PolVoz = 0 WHERE ID = {0}", row.PosPutId.Value);
        }

        return NoContent();
    }

    // GET /api/rasxod-putevok-fiz/{id}/schet-faktura
    [HttpGet("{id:long}/schet-faktura")]
    public async Task<IActionResult> GetSchetFaktura(long id)
    {
        var headerSql = $@"
            SELECT
                v.ID         AS Id,
                v.Num_Doc    AS NumDoc,
                v.Data_Post  AS DataPost,
                f.Name_Face  AS OrgName,
                NULL         AS OrgInn,
                v.Na_Kogo    AS NaKogo,
                v.Num_Dov    AS NumDov,
                v.Data_Dov   AS DataDov
            FROM tblViplF v
            LEFT JOIN tblSprav_Face f ON f.ID = v.FizLico
            WHERE v.ID = {0}";

        var headers = await db.Database
            .SqlQueryRaw<SchetFakturaHeaderRow>(headerSql, id)
            .ToListAsync();

        if (headers.Count == 0) return NotFound();
        var h = headers[0];

        var linesSql = $@"
            SELECT
                vp.ID             AS Id,
                vp.IdVipl,
                vp.rec_id         AS RecId,
                p.SERIAL_BEGIN    AS SerialBegin,
                p.SERIAL_END      AS SerialEnd,
                p.REST_BEGIN_DATE AS RestBeginDate,
                p.REST_END_DATE   AS RestEndDate,
                p.PRICE,
                vp.StatusPutyovki,
                p.SANATORIUM_ID   AS SanatoriumId,
                s.Name_San        AS SanatoriumName
            FROM tblVipl_Put vp
            JOIN tbpt_putyovki p   ON p.REC_ID  = vp.rec_id
            LEFT JOIN tblSprav_San s ON s.ID     = p.SANATORIUM_ID
            WHERE vp.IdVipl = {0}
            ORDER BY vp.ID";

        var lines = await db.Database
            .SqlQueryRaw<ViplDetailRow>(linesSql, id)
            .ToListAsync();

        var result = new SchetFakturaRow(
            Id:           h.Id,
            NumDoc:       h.NumDoc,
            DataPost:     h.DataPost,
            OrgName:      h.OrgName,
            OrgInn:       h.OrgInn,
            NaKogo:       h.NaKogo,
            NumDov:       h.NumDov,
            DataDov:      h.DataDov,
            TotalVouchers: lines.Count,
            TotalAmount:   lines.Sum(l => l.Price ?? 0m),
            Lines:         lines
        );

        return Ok(result);
    }
}

// ──────────────────────────────────────────────────────────────────────────────
// Controller: fizlica — full CRUD for physical persons (tblSprav_Face)
// DTOs (FizLicoDto / FizLicoSaveDto) are defined in OrganizaciiController.cs
// ──────────────────────────────────────────────────────────────────────────────

[ApiController]
[Route("api/fizlica")]
[Authorize]
public class FizLicaController(KasabaKIATContext db) : ControllerBase
{
    private long? CurrentUserId =>
        long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var uid) ? uid : null;

    // GET /api/fizlica?search=&inn=&pas_ser=&pas_number=  (max 30)
    [HttpGet]
    public async Task<IActionResult> GetFizLisoFilter(
        [FromQuery] string? search,
        [FromQuery] string? inn,
        [FromQuery] string? pas_ser,
        [FromQuery] string? pas_number)
    {
        var conditions = new List<string> { "1=1" };
        var parameters = new List<object>();
        int paramIdx = 0;

        if (!string.IsNullOrWhiteSpace(search))
        {
            conditions.Add($"(Name_Face LIKE {{{paramIdx}}} OR inn LIKE {{{paramIdx + 1}}})");
            parameters.Add($"%{search}%");
            parameters.Add($"%{search}%");
            paramIdx += 2;
        }

        if (!string.IsNullOrWhiteSpace(inn))
        {
            conditions.Add($"inn LIKE {{{paramIdx}}}");
            parameters.Add($"%{inn}%");
            paramIdx++;
        }

        if (!string.IsNullOrWhiteSpace(pas_ser))
        {
            conditions.Add($"pas_ser LIKE {{{paramIdx}}}");
            parameters.Add($"%{pas_ser}%");
            paramIdx++;
        }

        if (!string.IsNullOrWhiteSpace(pas_number))
        {
            conditions.Add($"pas_Num LIKE {{{paramIdx}}}");
            parameters.Add($"%{pas_number}%");
            paramIdx++;
        }

        var where = string.Join(" AND ", conditions);
        var sql = $"SELECT TOP 30 ID, Name_Face, UserId, " +
                  $"CAST(inn AS NVARCHAR(50)) AS inn, " +
                  $"CAST(adress AS NVARCHAR(500)) AS adress, " +
                  $"CAST(telefon AS NVARCHAR(50)) AS telefon, " +
                  $"CAST(pas_ser AS NVARCHAR(20)) AS pas_ser, " +
                  $"CAST(pas_Num AS NVARCHAR(20)) AS pas_Num, " +
                  $"CAST(pas_kd AS NVARCHAR(50)) AS pas_kd, " +
                  $"CAST(pas_data AS DATETIME) AS pas_data " +
                  $"FROM tblSprav_Face WHERE {where} ORDER BY Name_Face";

        try
        {
            var items = await db.Database
                .SqlQueryRaw<FizLicoDto>(sql, parameters.ToArray())
                .ToListAsync();
            return Ok(items);
        }
        catch
        {
            var fallback = await db.TblSpravFaces
                .Where(f => string.IsNullOrWhiteSpace(search) ||
                            (f.Name_Face != null && f.Name_Face.Contains(search!)))
                .OrderBy(f => f.Name_Face)
                .Take(30)
                .Select(f => new FizLicoDto { ID = f.ID, Name_Face = f.Name_Face, UserId = f.UserId })
                .ToListAsync();
            return Ok(fallback);
        }
    }

    // GET /api/fizlica/{id}
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetFizLisoData(long id)
    {
        var sql = "SELECT TOP 1 ID, Name_Face, UserId, " +
                  "CAST(inn AS NVARCHAR(50)) AS inn, " +
                  "CAST(adress AS NVARCHAR(500)) AS adress, " +
                  "CAST(telefon AS NVARCHAR(50)) AS telefon, " +
                  "CAST(pas_ser AS NVARCHAR(20)) AS pas_ser, " +
                  "CAST(pas_Num AS NVARCHAR(20)) AS pas_Num, " +
                  "CAST(pas_kd AS NVARCHAR(50)) AS pas_kd, " +
                  "CAST(pas_data AS DATETIME) AS pas_data " +
                  "FROM tblSprav_Face WHERE ID = {0}";
        try
        {
            var item = await db.Database.SqlQueryRaw<FizLicoDto>(sql, id).FirstOrDefaultAsync();
            if (item == null) return NotFound();
            return Ok(item);
        }
        catch
        {
            var fallback = await db.TblSpravFaces.FindAsync(id);
            if (fallback == null) return NotFound();
            return Ok(new FizLicoDto { ID = fallback.ID, Name_Face = fallback.Name_Face, UserId = fallback.UserId });
        }
    }

    // GET /api/fizlica/empty-carcas
    [HttpGet("empty-carcas")]
    public IActionResult GetEmptyCarcasFizLiso() => Ok(new FizLicoSaveDto());

    // POST /api/fizlica
    [HttpPost]
    public async Task<IActionResult> SaveFizLiso([FromBody] FizLicoSaveDto dto)
    {
        var userId = CurrentUserId;
        try
        {
            await db.Database.ExecuteSqlRawAsync(
                "INSERT INTO tblSprav_Face (Name_Face, UserId, inn, adress, telefon, pas_ser, pas_Num, pas_kd, pas_data) " +
                "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})",
                dto.Name_Face ?? string.Empty,
                userId as object ?? DBNull.Value,
                dto.inn as object ?? DBNull.Value,
                dto.adress as object ?? DBNull.Value,
                dto.telefon as object ?? DBNull.Value,
                dto.pas_ser as object ?? DBNull.Value,
                dto.pas_Num as object ?? DBNull.Value,
                dto.pas_kd as object ?? DBNull.Value,
                dto.pas_data as object ?? DBNull.Value);

            var created = await db.TblSpravFaces.OrderByDescending(f => f.ID).FirstOrDefaultAsync();
            return Created($"/api/fizlica/{created?.ID}", new FizLicoDto
            {
                ID = created?.ID ?? 0,
                Name_Face = created?.Name_Face,
                UserId = created?.UserId,
                inn = dto.inn,
                adress = dto.adress,
                telefon = dto.telefon,
                pas_ser = dto.pas_ser,
                pas_Num = dto.pas_Num,
                pas_kd = dto.pas_kd,
                pas_data = dto.pas_data
            });
        }
        catch
        {
            var face = new TblSpravFace { Name_Face = dto.Name_Face, UserId = userId };
            db.TblSpravFaces.Add(face);
            await db.SaveChangesAsync();
            return Created($"/api/fizlica/{face.ID}", new FizLicoDto { ID = face.ID, Name_Face = face.Name_Face, UserId = face.UserId });
        }
    }

    // PUT /api/fizlica/{id}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateFizLiso(long id, [FromBody] FizLicoSaveDto dto)
    {
        var userId = CurrentUserId;
        try
        {
            var affected = await db.Database.ExecuteSqlRawAsync(
                "UPDATE tblSprav_Face SET Name_Face={0}, UserId={1}, inn={2}, adress={3}, " +
                "telefon={4}, pas_ser={5}, pas_Num={6}, pas_kd={7}, pas_data={8} WHERE ID={9}",
                dto.Name_Face ?? string.Empty,
                userId as object ?? DBNull.Value,
                dto.inn as object ?? DBNull.Value,
                dto.adress as object ?? DBNull.Value,
                dto.telefon as object ?? DBNull.Value,
                dto.pas_ser as object ?? DBNull.Value,
                dto.pas_Num as object ?? DBNull.Value,
                dto.pas_kd as object ?? DBNull.Value,
                dto.pas_data as object ?? DBNull.Value,
                id);
            if (affected == 0) return NotFound();
            return Ok(new FizLicoDto
            {
                ID = id, Name_Face = dto.Name_Face, UserId = userId,
                inn = dto.inn, adress = dto.adress, telefon = dto.telefon,
                pas_ser = dto.pas_ser, pas_Num = dto.pas_Num,
                pas_kd = dto.pas_kd, pas_data = dto.pas_data
            });
        }
        catch
        {
            var face = await db.TblSpravFaces.FindAsync(id);
            if (face == null) return NotFound();
            face.Name_Face = dto.Name_Face;
            face.UserId = userId ?? face.UserId;
            await db.SaveChangesAsync();
            return Ok(new FizLicoDto { ID = face.ID, Name_Face = face.Name_Face, UserId = face.UserId });
        }
    }

    // DELETE /api/fizlica/{id}
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteFizLiso(long id)
    {
        var face = await db.TblSpravFaces.FindAsync(id);
        if (face == null) return NotFound();
        db.TblSpravFaces.Remove(face);
        await db.SaveChangesAsync();
        return NoContent();
    }
}

// ──────────────────────────────────────────────────────────────────────────────
// Private projection types used by SqlQueryRaw (must be non-nested classes
// or records with parameterless constructors or settable properties)
// ──────────────────────────────────────────────────────────────────────────────

internal class IdResult
{
    public long Id { get; set; }
}

internal class CountResult
{
    public int Count { get; set; }
}

internal class VoucherAggregate
{
    public int? TotalVouchers { get; set; }
    public decimal? TotalAmount { get; set; }
}

internal class ViplStatsExtRow
{
    public int? Year { get; set; }
    public long? Quarter { get; set; }
    public int? TotalNakladnoys { get; set; }
    public int? TotalVouchers { get; set; }
    public decimal? TotalAmount { get; set; }
}

internal class SchetFakturaHeaderRow
{
    public long Id { get; set; }
    public string? NumDoc { get; set; }
    public DateTime? DataPost { get; set; }
    public string? OrgName { get; set; }
    public string? OrgInn { get; set; }
    public string? NaKogo { get; set; }
    public string? NumDov { get; set; }
    public DateTime? DataDov { get; set; }
}

internal class AvailablePrixodRow
{
    public long Id { get; set; }
    public int? SanatoriumId { get; set; }
    public string? SanatoriumName { get; set; }
    public string? NumPut { get; set; }
    public DateTime? DataSan { get; set; }
    public DateTime? DataSanEnd { get; set; }
    public decimal? SymmaPut { get; set; }
}

internal class PrixodPutRow
{
    public long Id { get; set; }
    public int? IdSanat { get; set; }
    public string? NumPut { get; set; }
    public decimal? SymmaPut { get; set; }
}

internal class PrixodPutLookupRow
{
    public long Id { get; set; }
    public long? PosPutId { get; set; }
}
