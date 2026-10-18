using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

// ─── Request / Response types ───────────────────────────────────────────────

public record CreateNakladnoyRequest(
    int Year,
    int Quarter,
    string? PosName,
    string? NumDoc,
    DateTime? DataPost,
    string? NumDov
);

public record UpdateNakladnoyRequest(
    string? PosName,
    string? NumDoc,
    DateTime? DataPost,
    string? NumDov
);

public record AddVouchersRuchnoyRequest(
    long IdSan,
    int BeginNumber,
    int EndNumber,
    DateTime? SanatDate,
    decimal Price
);

public record SetBronRequest(DateTime? ExpireDate, string? Prim);

public record ChangeVoucherDateRequest(DateTime NewDate);

// ─── Result projections (for raw-SQL SqlQueryRaw) ───────────────────────────

public class RuchnoyNakladnoyRow
{
    public long Id { get; set; }
    public int? SmFil { get; set; }
    public long? IdKv { get; set; }
    public string? DepartmentBase { get; set; }
    public long? UserId { get; set; }
    public string? NumDoc { get; set; }
    public DateTime? DataPost { get; set; }
    public string? PosName { get; set; }
    public string? NumDov { get; set; }
    public string? Status { get; set; }
    public int? PutyovkaCount { get; set; }
    public decimal? SummaLinked { get; set; }
}

public class NakladnoyDetailRow
{
    public long Id { get; set; }
    public long? IdPost { get; set; }
    public int? IdSan { get; set; }
    public string? NumPut { get; set; }
    public DateTime? DataSan { get; set; }
    public decimal? SymmaPut { get; set; }
    public string? StatusPutyovki { get; set; }
    public string? SanName { get; set; }
}

public class NakladnoyStats
{
    public int NakladnoyCount { get; set; }
    public int PutyovkaCount { get; set; }
    public decimal SummaNakladnix { get; set; }
}

public class BronListRow
{
    public long Id { get; set; }
    public long? IdSanat { get; set; }
    public string? NumPut { get; set; }
    public bool? BronFlg { get; set; }
    public DateTime? BronDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public long? UserId { get; set; }
    public string? DepartmentBase { get; set; }
    public string? Prim { get; set; }
    public string? SanName { get; set; }
}

// ─── Controller ─────────────────────────────────────────────────────────────

[ApiController]
[Route("api/prixod-putevok/ruchnoy")]
[Authorize]
public class PrixodPutyovokRuchnoyController(KasabaKIATContext db) : ControllerBase
{
    // ── Helpers ──────────────────────────────────────────────────────────────

    private long GetUserId() =>
        long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

    private string? GetDepartmentBase() =>
        User.FindFirstValue("bkut_id");

    // ── GET /api/prixod-putevok/ruchnoy?year=&quarter= ───────────────────────
    // Returns { nakladnoys, stats }

    [HttpGet]
    public async Task<IActionResult> GetMainList(
        [FromQuery] int year,
        [FromQuery] int quarter,
        CancellationToken ct)
    {
        var depBase = GetDepartmentBase();
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var sqlBase = """
            SELECT
                p.ID,
                p.SmFil,
                p.ID_KV   AS IdKv,
                p.DepartmentBase,
                p.UserId,
                p.Num_Doc  AS NumDoc,
                p.Data_Post AS DataPost,
                p.Name_Dov  AS PosName,
                p.DovNum    AS NumDov,
                CAST(p.Vid AS varchar(10)) AS Status,
                COUNT(pp.ID)              AS PutyovkaCount,
                ISNULL(SUM(pp.Symma_Put), 0) AS SummaLinked
            FROM tblPost p
            LEFT JOIN tblPos_Put pp ON pp.ID_Post = p.ID
            WHERE YEAR(p.Data_Post) = {0}
              AND p.ID_KV = {1}
            """;
        var sqlGroupOrder = """

            GROUP BY
                p.ID, p.SmFil, p.ID_KV, p.DepartmentBase, p.UserId,
                p.Num_Doc, p.Data_Post, p.Name_Dov, p.DovNum, p.Vid
            ORDER BY p.Data_Post DESC
            """;

        List<RuchnoyNakladnoyRow> nakladnoys;
        try
        {
            nakladnoys = isSuperAdmin
                ? await db.Database
                    .SqlQueryRaw<RuchnoyNakladnoyRow>(sqlBase + sqlGroupOrder, year, quarter)
                    .ToListAsync(ct)
                : await db.Database
                    .SqlQueryRaw<RuchnoyNakladnoyRow>(
                        sqlBase + "  AND p.DepartmentBase = {2}" + sqlGroupOrder,
                        year, quarter, depBase!)
                    .ToListAsync(ct);
        }
        catch
        {
            nakladnoys = [];
        }

        var stats = new NakladnoyStats
        {
            NakladnoyCount = nakladnoys.Count,
            PutyovkaCount = nakladnoys.Sum(n => n.PutyovkaCount ?? 0),
            SummaNakladnix = nakladnoys.Sum(n => n.SummaLinked ?? 0m)
        };

        return Ok(new { nakladnoys, stats });
    }

    [HttpGet("{id:long}/details")]
    public async Task<IActionResult> GetDetailList(long id, CancellationToken ct)
    {
        const string sql = """
            SELECT
                pp.ID,
                pp.ID_Post   AS IdPost,
                pp.ID_Sanat  AS IdSan,
                pp.Num_Put   AS NumPut,
                pp.DataNach  AS DataSan,
                pp.Symma_Put AS SymmaPut,
                pp.Status    AS StatusPutyovki,
                s.Name_San   AS SanName
            FROM tblPos_Put pp
            LEFT JOIN tblSprav_San s ON s.ID = pp.ID_Sanat
            WHERE pp.ID_Post = {0}
            ORDER BY pp.Num_Put
            """;

        List<NakladnoyDetailRow> rows;
        try
        {
            rows = await db.Database
                .SqlQueryRaw<NakladnoyDetailRow>(sql, id)
                .ToListAsync(ct);
        }
        catch
        {
            rows = [];
        }

        return Ok(rows);
    }

    // ── GET /api/prixod-putevok/ruchnoy/{id}/details-resolved ────────────────
    // Detail rows joined with tblBrons resolution status

    [HttpGet("{id:long}/details-resolved")]
    public async Task<IActionResult> GetDetailListRes(long id, CancellationToken ct)
    {
        var depBase = GetDepartmentBase();

        const string sql = """
            SELECT
                pp.ID,
                pp.ID_Post   AS IdPost,
                pp.ID_Sanat  AS IdSan,
                pp.Num_Put   AS NumPut,
                pp.DataNach  AS DataSan,
                pp.Symma_Put AS SymmaPut,
                pp.Status    AS StatusPutyovki,
                s.Name_San   AS SanName
            FROM tblPos_Put pp
            LEFT JOIN tblSprav_San s ON s.ID = pp.ID_Sanat
            WHERE pp.ID_Post = {0}
            ORDER BY pp.Num_Put
            """;

        List<NakladnoyDetailRow> rows;
        try
        {
            rows = await db.Database
                .SqlQueryRaw<NakladnoyDetailRow>(sql, id)
                .ToListAsync(ct);
        }
        catch
        {
            rows = [];
        }

        // Enrich with bron info from EF context
        if (rows.Count > 0)
        {
            var sanId = rows.First().IdSan;
            var numPuts = rows.Select(r => r.NumPut).ToList();

            var brons = await db.TblBrons
                .Where(b => b.ID_Sanat == sanId
                            && b.Num_Put != null
                            && numPuts.Contains(b.Num_Put)
                            && b.BRON_FLG == true
                            && b.DEPARTMENT_BASE == depBase)
                .Select(b => new { b.Num_Put, b.BRON_FLG, b.EXPIRE_DATE })
                .ToListAsync(ct);

            var bronDict = brons.ToDictionary(b => b.Num_Put!, b => b);

            var enriched = rows.Select(r => new
            {
                r.Id,
                r.IdPost,
                r.IdSan,
                r.NumPut,
                r.DataSan,
                r.SymmaPut,
                r.StatusPutyovki,
                r.SanName,
                IsBroned = bronDict.ContainsKey(r.NumPut ?? ""),
                ExpireDate = bronDict.TryGetValue(r.NumPut ?? "", out var br) ? br.EXPIRE_DATE : (DateTime?)null
            }).ToList();

            return Ok(enriched);
        }

        return Ok(rows);
    }

    // ── GET /api/prixod-putevok/ruchnoy/stats?year=&quarter= ─────────────────

    [HttpGet("stats")]
    public async Task<IActionResult> GetTotalResults(
        [FromQuery] int year,
        [FromQuery] int quarter,
        CancellationToken ct)
    {
        var depBase = GetDepartmentBase();
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var sqlBase = """
            SELECT
                COUNT(DISTINCT p.ID)             AS NakladnoyCount,
                ISNULL(COUNT(pp.ID), 0)           AS PutyovkaCount,
                ISNULL(SUM(pp.Symma_Put), 0)      AS SummaNakladnix
            FROM tblPost p
            LEFT JOIN tblPos_Put pp ON pp.ID_Post = p.ID
            WHERE YEAR(p.Data_Post) = {0}
              AND p.ID_KV = {1}
            """;

        List<NakladnoyStats> statsRows;
        try
        {
            statsRows = isSuperAdmin
                ? await db.Database
                    .SqlQueryRaw<NakladnoyStats>(sqlBase, year, quarter)
                    .ToListAsync(ct)
                : await db.Database
                    .SqlQueryRaw<NakladnoyStats>(sqlBase + "  AND p.DepartmentBase = {2}", year, quarter, depBase!)
                    .ToListAsync(ct);
        }
        catch
        {
            statsRows = [];
        }

        var stats = statsRows.FirstOrDefault() ?? new NakladnoyStats();
        return Ok(stats);
    }

    // ── POST /api/prixod-putevok/ruchnoy — create invoice header ─────────────

    [HttpPost]
    public async Task<IActionResult> AddMain(
        [FromBody] CreateNakladnoyRequest req,
        CancellationToken ct)
    {
        var userId = GetUserId();
        var depBase = GetDepartmentBase();

        const string sql = """
            INSERT INTO tblPost (SmFil, ID_KV, DepartmentBase, UserId, Num_Doc, Data_Post, Name_Dov, DovNum, Vid)
            OUTPUT INSERTED.ID
            VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, 0)
            """;

        long newId;
        try
        {
            var result = await db.Database
                .SqlQueryRaw<long>(sql,
                    req.Year, req.Quarter, depBase!, userId,
                    req.NumDoc, req.DataPost ?? DateTime.Now, req.PosName, req.NumDov)
                .ToListAsync(ct);
            newId = result.FirstOrDefault();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        return Ok(new { id = newId });
    }

    // ── PUT /api/prixod-putevok/ruchnoy/{id} — update invoice header ──────────

    [HttpPut("{id:long}")]
    public async Task<IActionResult> ModifyMain(
        long id,
        [FromBody] UpdateNakladnoyRequest req,
        CancellationToken ct)
    {
        const string sql = """
            UPDATE tblPost
            SET Name_Dov  = {0},
                Num_Doc   = {1},
                Data_Post = {2},
                DovNum    = {3}
            WHERE ID = {4}
            """;

        try
        {
            var affected = await db.Database.ExecuteSqlRawAsync(
                sql, req.PosName, req.NumDoc, req.DataPost, req.NumDov, id, ct);

            if (affected == 0) return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        return NoContent();
    }

    // ── DELETE /api/prixod-putevok/ruchnoy/{id} — delete header + lines ───────

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> RemoveAllWithLinked(long id, CancellationToken ct)
    {
        try
        {
            // Delete detail lines first
            await db.Database.ExecuteSqlRawAsync(
                "DELETE FROM tblPos_Put WHERE ID_Post = {0}", id, ct);

            // Delete header
            var affected = await db.Database.ExecuteSqlRawAsync(
                "DELETE FROM tblPost WHERE ID = {0}", id, ct);

            if (affected == 0) return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        return NoContent();
    }

    // ── POST /api/prixod-putevok/ruchnoy/{id}/provodka — проведено ───────────

    [HttpPost("{id:long}/provodka")]
    public async Task<IActionResult> CloseNakladnoy(long id, CancellationToken ct)
    {
        try
        {
            var affected = await db.Database.ExecuteSqlRawAsync(
                "UPDATE tblPost SET Status = 'CLOSED' WHERE ID = {0}", id, ct);

            if (affected == 0) return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        return Ok(new { id, status = "CLOSED" });
    }

    // ── POST /api/prixod-putevok/ruchnoy/{id}/vouchers — add vouchers ─────────
    // Generates voucher numbers from beginNumber to endNumber and inserts rows.

    [HttpPost("{id:long}/vouchers")]
    public async Task<IActionResult> AddDetail(
        long id,
        [FromBody] AddVouchersRuchnoyRequest req,
        CancellationToken ct)
    {
        if (req.BeginNumber > req.EndNumber)
            return BadRequest(new { error = "beginNumber must be <= endNumber" });

        // Verify the header exists
        List<long> headerCheck;
        try
        {
            headerCheck = await db.Database
                .SqlQueryRaw<long>("SELECT ID FROM tblPost WHERE ID = {0}", id)
                .ToListAsync(ct);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        if (headerCheck.Count == 0)
            return NotFound(new { error = $"Invoice header {id} not found" });

        var inserted = new List<string>();
        var errors = new List<string>();
        int padWidth = Math.Max(4, req.EndNumber.ToString().Length);

        for (int num = req.BeginNumber; num <= req.EndNumber; num++)
        {
            var numPut = num.ToString().PadLeft(padWidth, '0');
            try
            {
                await db.Database.ExecuteSqlRawAsync(
                    """
                    INSERT INTO tblPos_Put (ID_Post, ID_Sanat, Num_Put, DataNach, Symma_Put, Status, MODIFY_DATE)
                    VALUES ({0}, {1}, {2}, {3}, {4}, 'NEW', GETDATE())
                    """,
                    id, req.IdSan, numPut, req.SanatDate, req.Price, ct);
                inserted.Add(numPut);
            }
            catch (Exception ex)
            {
                errors.Add($"{numPut}: {ex.Message}");
            }
        }

        return Ok(new { inserted = inserted.Count, failed = errors.Count, errors });
    }

    // ── DELETE /api/prixod-putevok/ruchnoy/vouchers/{voucherId} ──────────────

    [HttpDelete("vouchers/{voucherId:long}")]
    public async Task<IActionResult> RemovePutyovkaDetail(long voucherId, CancellationToken ct)
    {
        try
        {
            var affected = await db.Database.ExecuteSqlRawAsync(
                "DELETE FROM tblPos_Put WHERE ID = {0}", voucherId, ct);

            if (affected == 0) return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        return NoContent();
    }

    // ── PUT /api/prixod-putevok/ruchnoy/vouchers/{voucherId}/bron ─────────────
    // Set бронь: inserts or updates a record in tblBrons and marks the line.

    [HttpPut("vouchers/{voucherId:long}/bron")]
    public async Task<IActionResult> PostavitVBron(
        long voucherId,
        [FromBody] SetBronRequest req,
        CancellationToken ct)
    {
        var userId = GetUserId();
        var depBase = GetDepartmentBase();

        // Fetch voucher info
        List<NakladnoyDetailRow> voucherRows;
        try
        {
            voucherRows = await db.Database
                .SqlQueryRaw<NakladnoyDetailRow>(
                    """
                    SELECT pp.ID, pp.ID_Post AS IdPost, pp.ID_Sanat AS IdSan, pp.Num_Put AS NumPut,
                           pp.DataNach AS DataSan, pp.Symma_Put AS SymmaPut, pp.Status AS StatusPutyovki, '' AS SanName
                    FROM tblPos_Put pp
                    WHERE pp.ID = {0}
                    """, voucherId)
                .ToListAsync(ct);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        if (voucherRows.Count == 0) return NotFound(new { error = "Voucher not found" });

        var voucher = voucherRows[0];

        // Check if bron already exists in EF TblBrons
        var existingBron = await db.TblBrons
            .FirstOrDefaultAsync(b => b.ID_Sanat == voucher.IdSan
                                      && b.Num_Put == voucher.NumPut
                                      && b.DEPARTMENT_BASE == depBase, ct);

        if (existingBron != null)
        {
            existingBron.BRON_FLG = true;
            existingBron.BRON_DATE = DateTime.Now;
            existingBron.EXPIRE_DATE = req.ExpireDate;
            existingBron.USER_ID = userId;
            existingBron.PRIM = req.Prim;
        }
        else
        {
            var bron = new TblBron
            {
                ID_Sanat = voucher.IdSan,
                Num_Put = voucher.NumPut,
                BRON_FLG = true,
                BRON_DATE = DateTime.Now,
                EXPIRE_DATE = req.ExpireDate,
                USER_ID = userId,
                DEPARTMENT_BASE = depBase,
                PRIM = req.Prim
            };
            db.TblBrons.Add(bron);
        }

        // Update voucher status
        try
        {
            await db.Database.ExecuteSqlRawAsync(
                "UPDATE tblPos_Put SET Status = 'BRON' WHERE ID = {0}",
                voucherId, ct);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"Failed to update voucher status: {ex.Message}" });
        }

        await db.SaveChangesAsync(ct);
        return Ok(new { voucherId, status = "BRON" });
    }

    // ── PUT /api/prixod-putevok/ruchnoy/vouchers/{voucherId}/unbron ───────────
    // Снять бронь

    [HttpPut("vouchers/{voucherId:long}/unbron")]
    public async Task<IActionResult> SnyatsBron(long voucherId, CancellationToken ct)
    {
        var depBase = GetDepartmentBase();

        // Fetch voucher info
        List<NakladnoyDetailRow> voucherRows;
        try
        {
            voucherRows = await db.Database
                .SqlQueryRaw<NakladnoyDetailRow>(
                    """
                    SELECT pp.ID, pp.ID_Post AS IdPost, pp.ID_Sanat AS IdSan, pp.Num_Put AS NumPut,
                           pp.DataNach AS DataSan, pp.Symma_Put AS SymmaPut, pp.Status AS StatusPutyovki, '' AS SanName
                    FROM tblPos_Put pp
                    WHERE pp.ID = {0}
                    """, voucherId)
                .ToListAsync(ct);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        if (voucherRows.Count == 0) return NotFound(new { error = "Voucher not found" });

        var voucher = voucherRows[0];

        // Remove bron in EF context
        var brons = await db.TblBrons
            .Where(b => b.ID_Sanat == voucher.IdSan
                        && b.Num_Put == voucher.NumPut
                        && b.DEPARTMENT_BASE == depBase)
            .ToListAsync(ct);

        foreach (var b in brons)
            b.BRON_FLG = false;

        // Update voucher status back to FREE / SVOB
        try
        {
            await db.Database.ExecuteSqlRawAsync(
                "UPDATE tblPos_Put SET Status = 'FREE' WHERE ID = {0}",
                voucherId, ct);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = $"Failed to update voucher status: {ex.Message}" });
        }

        await db.SaveChangesAsync(ct);
        return Ok(new { voucherId, status = "FREE" });
    }

    // ── PUT /api/prixod-putevok/ruchnoy/vouchers/{voucherId}/date ─────────────
    // Change voucher date (IzmData)

    [HttpPut("vouchers/{voucherId:long}/date")]
    public async Task<IActionResult> IzmData(
        long voucherId,
        [FromBody] ChangeVoucherDateRequest req,
        CancellationToken ct)
    {
        try
        {
            var affected = await db.Database.ExecuteSqlRawAsync(
                "UPDATE tblPos_Put SET DataNach = {0} WHERE ID = {1}",
                req.NewDate, voucherId, ct);

            if (affected == 0) return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        return Ok(new { voucherId, newDate = req.NewDate });
    }

    // ── GET /api/prixod-putevok/ruchnoy/bron-list?year=&quarter= ─────────────

    [HttpGet("bron-list")]
    public async Task<IActionResult> GetBronList(
        [FromQuery] int year,
        [FromQuery] int quarter,
        CancellationToken ct)
    {
        var depBase = GetDepartmentBase();

        // Join brons with tblPos_Put and tblPost to filter by year/quarter
        const string sql = """
            SELECT
                b.ID,
                b.ID_Sanat  AS IdSanat,
                b.Num_Put   AS NumPut,
                b.BRON_FLG  AS BronFlg,
                b.BRON_DATE AS BronDate,
                b.EXPIRE_DATE AS ExpireDate,
                b.USER_ID   AS UserId,
                b.DEPARTMENT_BASE AS DepartmentBase,
                b.PRIM      AS Prim,
                s.Name_San  AS SanName
            FROM tblBrons b
            LEFT JOIN tblSprav_San s ON s.ID = b.ID_Sanat
            INNER JOIN tblPos_Put pp ON pp.ID_Sanat = b.ID_Sanat AND pp.Num_Put = b.Num_Put
            INNER JOIN tblPost p ON p.ID = pp.ID_Post
                                 AND YEAR(p.Data_Post) = {0}
                                 AND p.ID_KV = {1}
                                 AND p.DepartmentBase = {2}
            WHERE b.BRON_FLG = 1
              AND b.DEPARTMENT_BASE = {2}
            ORDER BY b.BRON_DATE DESC
            """;

        List<BronListRow> rows;
        try
        {
            rows = await db.Database
                .SqlQueryRaw<BronListRow>(sql, year, quarter, depBase!)
                .ToListAsync(ct);
        }
        catch
        {
            // Fallback: return brons directly from EF without join to tblPost
            rows = await db.TblBrons
                .Where(b => b.BRON_FLG == true && b.DEPARTMENT_BASE == depBase)
                .OrderByDescending(b => b.BRON_DATE)
                .Select(b => new BronListRow
                {
                    Id = b.ID,
                    IdSanat = b.ID_Sanat,
                    NumPut = b.Num_Put,
                    BronFlg = b.BRON_FLG,
                    BronDate = b.BRON_DATE,
                    ExpireDate = b.EXPIRE_DATE,
                    UserId = b.USER_ID,
                    DepartmentBase = b.DEPARTMENT_BASE,
                    Prim = b.PRIM,
                    SanName = null
                })
                .ToListAsync(ct);
        }

        return Ok(rows);
    }

    // ── GET /api/prixod-putevok/ruchnoy/print?year=&quarter= ─────────────────
    // Returns list for print (PechatPrixodPutyovok)

    [HttpGet("print")]
    public async Task<IActionResult> PechatPrixodPutyovok(
        [FromQuery] int year,
        [FromQuery] int quarter,
        CancellationToken ct)
    {
        var depBase = GetDepartmentBase();

        const string sql = """
            SELECT
                p.ID,
                p.SmFil,
                p.ID_KV   AS IdKv,
                p.DepartmentBase,
                p.UserId,
                p.Num_Doc  AS NumDoc,
                p.Data_Post AS DataPost,
                p.Name_Dov  AS PosName,
                p.DovNum    AS NumDov,
                CAST(p.Vid AS varchar(10)) AS Status,
                COUNT(pp.ID)              AS PutyovkaCount,
                ISNULL(SUM(pp.Symma_Put), 0) AS SummaLinked
            FROM tblPost p
            LEFT JOIN tblPos_Put pp ON pp.ID_Post = p.ID
            WHERE YEAR(p.Data_Post) = {0}
              AND p.ID_KV = {1}
              AND p.DepartmentBase = {2}
            GROUP BY
                p.ID, p.SmFil, p.ID_KV, p.DepartmentBase, p.UserId,
                p.Num_Doc, p.Data_Post, p.Name_Dov, p.DovNum, p.Vid
            ORDER BY p.Data_Post
            """;

        List<RuchnoyNakladnoyRow> rows;
        try
        {
            rows = await db.Database
                .SqlQueryRaw<RuchnoyNakladnoyRow>(sql, year, quarter, depBase!)
                .ToListAsync(ct);
        }
        catch
        {
            rows = [];
        }

        // For print, enrich each nakladnoy with its detail lines
        var printData = new List<object>();
        foreach (var row in rows)
        {
            List<NakladnoyDetailRow> details;
            try
            {
                details = await db.Database
                    .SqlQueryRaw<NakladnoyDetailRow>(
                        """
                        SELECT pp.ID, pp.ID_Post AS IdPost, pp.ID_Sanat AS IdSan, pp.Num_Put AS NumPut,
                               pp.DataNach AS DataSan, pp.Symma_Put AS SymmaPut, pp.Status AS StatusPutyovki, s.Name_San AS SanName
                        FROM tblPos_Put pp
                        LEFT JOIN tblSprav_San s ON s.ID = pp.ID_Sanat
                        WHERE pp.ID_Post = {0}
                        ORDER BY pp.Num_Put
                        """, row.Id)
                    .ToListAsync(ct);
            }
            catch
            {
                details = [];
            }

            printData.Add(new
            {
                header = row,
                details
            });
        }

        return Ok(printData);
    }
}
