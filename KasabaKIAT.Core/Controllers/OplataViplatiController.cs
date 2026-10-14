using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace KasabaKIAT.Core.Controllers;

// ---------------------------------------------------------------------------
// Inline DTOs
// ---------------------------------------------------------------------------

public record OplataMainRequest(
    int Year,
    int Quarter,
    long? OrgId,
    string? NumDoc,
    DateTime? DataPost,
    decimal? SummaDt,
    decimal? SummaKt,
    int TipOplaty,   // 1=org receipt, 2=voucher payment, 3=resort, 4=return
    int VidOplaty
);

public record OplataDetailRequest(
    long OplataId,
    long? IdViplPut,
    string? NameProc,
    decimal Symma
);

public record OplataSummaRequest(
    long OrgId,
    long? ProcentId
);

// ---------------------------------------------------------------------------
// Controller
// ---------------------------------------------------------------------------

/// <summary>
/// Оплата и выплаты за путёвки — covers tblOplata (payment headers) and
/// tblOplata_Rasxod (payment detail lines).  Both tables are managed via
/// raw SQL because they are not in the EF DbContext.
/// </summary>
[ApiController]
[Route("api/oplata-viplati")]
[Authorize]
public class OplataViplatiController(KasabaKIATContext db) : ControllerBase
{
    // -----------------------------------------------------------------------
    // Helper: open a connection from the EF context
    // -----------------------------------------------------------------------
    private IDbConnection OpenConnection()
    {
        var conn = db.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open)
            conn.Open();
        return conn;
    }

    // -----------------------------------------------------------------------
    // GET /api/oplata-viplati?year=&quarter=&tipId=&vidId=
    // Returns oplataMains list + totals (summaDebit, summaCredit, saldo)
    // -----------------------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetOplataMain(
        [FromQuery] int year,
        [FromQuery] int quarter,
        [FromQuery] int? tipId,
        [FromQuery] int? vidId)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            using var cmd = conn.CreateCommand();

            var sb = new StringBuilder(@"
                SELECT
                    o.ID,
                    o.SmFil        AS Year,
                    o.Id_Kv        AS Quarter,
                    o.DepartmentBase,
                    o.UserId,
                    o.Data_Post,
                    o.Num_Doc,
                    o.ID_Org,
                    org.Name_Org   AS OrgName,
                    o.Symma_DT,
                    o.Symma_KT,
                    o.TipOrg       AS TipOplaty,
                    o.Priz         AS VidOplaty
                FROM tblOplata o
                LEFT JOIN tblSprav_Org org ON org.ID = o.ID_Org
                WHERE o.SmFil = @year
                  AND o.Id_Kv = @quarter");

            cmd.Parameters.Add(new SqlParameter("@year", year));
            cmd.Parameters.Add(new SqlParameter("@quarter", quarter));

            if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            {
                sb.Append(" AND o.DepartmentBase = @depBase");
                cmd.Parameters.Add(new SqlParameter("@depBase", depBase));
            }

            if (tipId.HasValue)
            {
                sb.Append(" AND o.TipOrg = @tipId");
                cmd.Parameters.Add(new SqlParameter("@tipId", tipId.Value));
            }

            if (vidId.HasValue)
            {
                sb.Append(" AND o.Priz = @vidId");
                cmd.Parameters.Add(new SqlParameter("@vidId", vidId.Value));
            }

            sb.Append(" ORDER BY o.Data_Post DESC, o.ID DESC");
            cmd.CommandText = sb.ToString();

            var rows = new List<object>();
            decimal totalDt = 0, totalKt = 0;

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    decimal dt = reader["Symma_DT"] == DBNull.Value ? 0 : (decimal)reader["Symma_DT"];
                    decimal kt = reader["Symma_KT"] == DBNull.Value ? 0 : (decimal)reader["Symma_KT"];
                    totalDt += dt;
                    totalKt += kt;

                    rows.Add(new
                    {
                        id         = (long)reader["ID"],
                        year       = (int)reader["Year"],
                        quarter    = (int)reader["Quarter"],
                        depBase    = reader["DepartmentBase"]  == DBNull.Value ? null : (string)reader["DepartmentBase"],
                        userId     = reader["UserId"]          == DBNull.Value ? (long?)null : (long)reader["UserId"],
                        dataPost   = reader["Data_Post"]       == DBNull.Value ? (DateTime?)null : (DateTime)reader["Data_Post"],
                        numDoc     = reader["Num_Doc"]         == DBNull.Value ? null : (string)reader["Num_Doc"],
                        idOrg      = reader["ID_Org"]          == DBNull.Value ? (long?)null : (long)reader["ID_Org"],
                        orgName    = reader["OrgName"]         == DBNull.Value ? null : (string)reader["OrgName"],
                        summaDt    = dt,
                        summaKt    = kt,
                        tipOplaty  = reader["TipOplaty"]       == DBNull.Value ? (int?)null : (int)reader["TipOplaty"],
                        vidOplaty  = reader["VidOplaty"]       == DBNull.Value ? (int?)null : (int)reader["VidOplaty"]
                    });
                }
            }

            return Ok(new
            {
                oplataMains = rows,
                totals = new
                {
                    summaDebit  = totalDt,
                    summaCredit = totalKt,
                    saldo       = totalDt - totalKt
                }
            });
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    // -----------------------------------------------------------------------
    // GET /api/oplata-viplati/{id}/details
    // Payment detail rows with sanatorium name and voucher info
    // -----------------------------------------------------------------------
    [HttpGet("{id:long}/details")]
    public async Task<IActionResult> GetOplataDetail(long id)
    {
        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT
                    r.ID,
                    r.IdOplata,
                    r.IdVipl_Put,
                    r.Name_Proc,
                    r.Symma,
                    s.Name_San  AS IdSanatName,
                    p.SERIAL_BEGIN AS Num_Put
                FROM tblOplata_Rasxod r
                LEFT JOIN tblVipl_Put vp  ON vp.ID        = r.IdVipl_Put
                LEFT JOIN tblSprav_San s  ON s.ID         = vp.ID_San
                LEFT JOIN tbpt_putyovki p ON p.REC_ID     = vp.ID_Put
                WHERE r.IdOplata = @id
                ORDER BY r.ID";

            cmd.Parameters.Add(new SqlParameter("@id", id));

            var rows = new List<object>();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    rows.Add(new
                    {
                        id         = (long)reader["ID"],
                        idOplata   = (long)reader["IdOplata"],
                        idViplPut  = reader["IdVipl_Put"]  == DBNull.Value ? (long?)null : (long)reader["IdVipl_Put"],
                        nameProc   = reader["Name_Proc"]   == DBNull.Value ? null : (string)reader["Name_Proc"],
                        symma      = reader["Symma"]        == DBNull.Value ? 0m   : (decimal)reader["Symma"],
                        idSanatName= reader["IdSanatName"] == DBNull.Value ? null : (string)reader["IdSanatName"],
                        numPut     = reader["Num_Put"]      == DBNull.Value ? null : (string)reader["Num_Put"]
                    });
                }
            }

            return Ok(rows);
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    // -----------------------------------------------------------------------
    // POST /api/oplata-viplati  — create payment header
    // tipOplaty: 1=org receipt, 2=voucher payment, 3=resort payment, 4=return
    // -----------------------------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> CreateOplataMain([FromBody] OplataMainRequest req)
    {
        var userId   = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase  = User.FindFirstValue("bkut_id");

        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO tblOplata
                    (SmFil, Id_Kv, DepartmentBase, UserId, Data_Post, Num_Doc, ID_Org,
                     Symma_DT, Symma_KT, TipOrg, Priz)
                OUTPUT INSERTED.ID
                VALUES
                    (@year, @quarter, @depBase, @userId, @dataPost, @numDoc, @idOrg,
                     @summaDt, @summaKt, @tipOplaty, @vidOplaty)";

            cmd.Parameters.Add(new SqlParameter("@year",      req.Year));
            cmd.Parameters.Add(new SqlParameter("@quarter",   req.Quarter));
            cmd.Parameters.Add(new SqlParameter("@depBase",   (object?)depBase     ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@userId",    userId));
            cmd.Parameters.Add(new SqlParameter("@dataPost",  (object?)req.DataPost ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@numDoc",    (object?)req.NumDoc  ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@idOrg",     (object?)req.OrgId   ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@summaDt",   req.SummaDt  ?? 0m));
            cmd.Parameters.Add(new SqlParameter("@summaKt",   req.SummaKt  ?? 0m));
            cmd.Parameters.Add(new SqlParameter("@tipOplaty", req.TipOplaty));
            cmd.Parameters.Add(new SqlParameter("@vidOplaty", req.VidOplaty));

            var newId = (long)await cmd.ExecuteScalarAsync()!;
            return Ok(new { id = newId });
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    // -----------------------------------------------------------------------
    // PUT /api/oplata-viplati/{id} — update payment header
    // -----------------------------------------------------------------------
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateOplataMain(long id, [FromBody] OplataMainRequest req)
    {
        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE tblOplata SET
                    SmFil       = @year,
                    Id_Kv       = @quarter,
                    Data_Post   = @dataPost,
                    Num_Doc     = @numDoc,
                    ID_Org      = @idOrg,
                    Symma_DT    = @summaDt,
                    Symma_KT    = @summaKt,
                    TipOrg      = @tipOplaty,
                    Priz        = @vidOplaty
                WHERE ID = @id";

            cmd.Parameters.Add(new SqlParameter("@year",      req.Year));
            cmd.Parameters.Add(new SqlParameter("@quarter",   req.Quarter));
            cmd.Parameters.Add(new SqlParameter("@dataPost",  (object?)req.DataPost ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@numDoc",    (object?)req.NumDoc   ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@idOrg",     (object?)req.OrgId    ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@summaDt",   req.SummaDt  ?? 0m));
            cmd.Parameters.Add(new SqlParameter("@summaKt",   req.SummaKt  ?? 0m));
            cmd.Parameters.Add(new SqlParameter("@tipOplaty", req.TipOplaty));
            cmd.Parameters.Add(new SqlParameter("@vidOplaty", req.VidOplaty));
            cmd.Parameters.Add(new SqlParameter("@id",        id));

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows == 0 ? NotFound() : NoContent();
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    // -----------------------------------------------------------------------
    // DELETE /api/oplata-viplati/{id} — delete header and all detail lines
    // -----------------------------------------------------------------------
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteOplataMain(long id)
    {
        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            // First delete detail lines (child rows)
            using (var cmdDetail = conn.CreateCommand())
            {
                cmdDetail.CommandText = "DELETE FROM tblOplata_Rasxod WHERE IdOplata = @id";
                cmdDetail.Parameters.Add(new SqlParameter("@id", id));
                await cmdDetail.ExecuteNonQueryAsync();
            }

            // Then delete the header
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM tblOplata WHERE ID = @id";
            cmd.Parameters.Add(new SqlParameter("@id", id));
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows == 0 ? NotFound() : NoContent();
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    // -----------------------------------------------------------------------
    // POST /api/oplata-viplati/parse-excel
    // Accepts multipart/form-data with a CSV or Excel file and bulk-inserts
    // payment rows.  EPPlus is not in the project dependencies, so this
    // implementation reads a simple CSV:
    //   NumDoc,OrgId,DataPost,SummaDt,SummaKt,TipOplaty,VidOplaty
    // -----------------------------------------------------------------------
    [HttpPost("parse-excel")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ParseExcel(
        [FromForm] IFormFile file,
        [FromQuery] int year,
        [FromQuery] int quarter)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file provided." });

        var userId  = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        // Read text content (CSV expected; Excel would need EPPlus/NPOI)
        string content;
        using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            content = await reader.ReadToEndAsync();

        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2)
            return BadRequest(new { message = "File must contain a header row and at least one data row." });

        var imported = new List<long>();
        var errors   = new List<string>();

        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            // Skip header line (index 0)
            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Trim().Split(',');
                if (parts.Length < 7)
                {
                    errors.Add($"Row {i + 1}: expected 7 columns, got {parts.Length}.");
                    continue;
                }

                try
                {
                    string  numDoc   = parts[0].Trim();
                    long?   orgId    = string.IsNullOrWhiteSpace(parts[1]) ? null : long.Parse(parts[1].Trim());
                    DateTime? dataPost = string.IsNullOrWhiteSpace(parts[2]) ? null : DateTime.Parse(parts[2].Trim());
                    decimal summaDt  = decimal.Parse(parts[3].Trim());
                    decimal summaKt  = decimal.Parse(parts[4].Trim());
                    int tipOplaty    = int.Parse(parts[5].Trim());
                    int vidOplaty    = int.Parse(parts[6].Trim());

                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                        INSERT INTO tblOplata
                            (SmFil, Id_Kv, DepartmentBase, UserId, Data_Post, Num_Doc, ID_Org,
                             Symma_DT, Symma_KT, TipOrg, Priz)
                        OUTPUT INSERTED.ID
                        VALUES
                            (@year, @quarter, @depBase, @userId, @dataPost, @numDoc, @idOrg,
                             @summaDt, @summaKt, @tipOplaty, @vidOplaty)";

                    cmd.Parameters.Add(new SqlParameter("@year",      year));
                    cmd.Parameters.Add(new SqlParameter("@quarter",   quarter));
                    cmd.Parameters.Add(new SqlParameter("@depBase",   (object?)depBase  ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userId",    userId));
                    cmd.Parameters.Add(new SqlParameter("@dataPost",  (object?)dataPost ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@numDoc",    (object?)numDoc   ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@idOrg",     (object?)orgId    ?? DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@summaDt",   summaDt));
                    cmd.Parameters.Add(new SqlParameter("@summaKt",   summaKt));
                    cmd.Parameters.Add(new SqlParameter("@tipOplaty", tipOplaty));
                    cmd.Parameters.Add(new SqlParameter("@vidOplaty", vidOplaty));

                    var newId = (long)await cmd.ExecuteScalarAsync()!;
                    imported.Add(newId);
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {i + 1}: {ex.Message}");
                }
            }
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }

        return Ok(new { importedIds = imported, errors });
    }

    // -----------------------------------------------------------------------
    // GET /api/oplata-viplati/template
    // Returns a minimal CSV template for download
    // -----------------------------------------------------------------------
    [HttpGet("template")]
    public IActionResult DownloadTemplate()
    {
        const string csvContent =
            "NumDoc,OrgId,DataPost,SummaDt,SummaKt,TipOplaty,VidOplaty\r\n" +
            "DOC-001,,2024-01-01,0.00,0.00,1,1\r\n";

        var bytes = Encoding.UTF8.GetBytes(csvContent);
        return File(bytes, "text/csv", "oplata_viplati_template.csv");
    }

    // -----------------------------------------------------------------------
    // GET /api/oplata-viplati/summa?orgId=&procentId=
    // Returns total debt for the org and list of uncovered putyovkas
    // -----------------------------------------------------------------------
    [HttpGet("summa")]
    public async Task<IActionResult> GetSummaInfo([FromQuery] long orgId, [FromQuery] long? procentId)
    {
        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        decimal debtByOrg = 0;

        try
        {
            // Total debt: sum of (Symma_DT - Symma_KT) for the org
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT ISNULL(SUM(Symma_DT - Symma_KT), 0)
                    FROM tblOplata
                    WHERE ID_Org = @orgId";
                cmd.Parameters.Add(new SqlParameter("@orgId", orgId));
                var result = await cmd.ExecuteScalarAsync();
                debtByOrg = result == DBNull.Value ? 0m : (decimal)result!;
            }

            // Not-covered putyovkas: voucher distribution lines not yet fully paid
            var notCovered = new List<object>();

            var vpSb = new StringBuilder(@"
                SELECT
                    vp.ID,
                    vp.ID_San,
                    s.Name_San,
                    vp.ID_Put,
                    p.SERIAL_BEGIN AS Num_Put,
                    vp.Symma,
                    vp.ID_Org
                FROM tblVipl_Put vp
                LEFT JOIN tblSprav_San s   ON s.ID       = vp.ID_San
                LEFT JOIN tbpt_putyovki p  ON p.REC_ID   = vp.ID_Put
                WHERE vp.ID_Org = @orgId2
                  AND ISNULL(vp.Symma, 0) > ISNULL(
                        (SELECT SUM(r.Symma) FROM tblOplata_Rasxod r WHERE r.IdVipl_Put = vp.ID), 0)");

            if (procentId.HasValue)
            {
                vpSb.Append(" AND vp.ID_Proc = @procentId");
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = vpSb.ToString();
                cmd.Parameters.Add(new SqlParameter("@orgId2", orgId));
                if (procentId.HasValue)
                    cmd.Parameters.Add(new SqlParameter("@procentId", procentId.Value));

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    notCovered.Add(new
                    {
                        id       = (long)reader["ID"],
                        idSan    = reader["ID_San"]    == DBNull.Value ? (long?)null : (long)reader["ID_San"],
                        sanName  = reader["Name_San"]  == DBNull.Value ? null : (string)reader["Name_San"],
                        idPut    = reader["ID_Put"]    == DBNull.Value ? (long?)null : (long)reader["ID_Put"],
                        numPut   = reader["Num_Put"]   == DBNull.Value ? null : (string)reader["Num_Put"],
                        symma    = reader["Symma"]      == DBNull.Value ? 0m   : (decimal)reader["Symma"],
                        idOrg    = reader["ID_Org"]    == DBNull.Value ? (long?)null : (long)reader["ID_Org"]
                    });
                }
            }

            return Ok(new
            {
                debtByOrg,
                notCoveredPutyovkas = notCovered
            });
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    // -----------------------------------------------------------------------
    // GET /api/oplata-viplati/tip-oplaty — payment types from tbcs_dictionary
    // -----------------------------------------------------------------------
    [HttpGet("tip-oplaty")]
    public async Task<IActionResult> GetTipOplaty()
    {
        var items = await db.TbcsDictionaries
            .Join(db.TbcsDictionaryTypes,
                d  => d.DICTIONARY_TYPE_ID,
                dt => dt.REC_ID,
                (d, dt) => new { d, dt })
            .Where(x => x.dt.CODE == "TipOplaty")
            .Select(x => new
            {
                id      = x.d.REC_ID,
                nameRu  = x.d.NAME_RU,
                nameUz  = x.d.NAME_UZ,
                code    = x.d.CODE
            })
            .ToListAsync();

        return Ok(items);
    }

    // -----------------------------------------------------------------------
    // GET /api/oplata-viplati/vid-oplaty — payment kinds from tbcs_dictionary
    // -----------------------------------------------------------------------
    [HttpGet("vid-oplaty")]
    public async Task<IActionResult> GetVidOplaty()
    {
        var items = await db.TbcsDictionaries
            .Join(db.TbcsDictionaryTypes,
                d  => d.DICTIONARY_TYPE_ID,
                dt => dt.REC_ID,
                (d, dt) => new { d, dt })
            .Where(x => x.dt.CODE == "VidOplaty")
            .Select(x => new
            {
                id      = x.d.REC_ID,
                nameRu  = x.d.NAME_RU,
                nameUz  = x.d.NAME_UZ,
                code    = x.d.CODE
            })
            .ToListAsync();

        return Ok(items);
    }

    // -----------------------------------------------------------------------
    // POST /api/oplata-viplati/detail — add a detail line to a payment
    // -----------------------------------------------------------------------
    [HttpPost("detail")]
    public async Task<IActionResult> AddOplataDetail([FromBody] OplataDetailRequest req)
    {
        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO tblOplata_Rasxod (IdOplata, IdVipl_Put, Name_Proc, Symma)
                OUTPUT INSERTED.ID
                VALUES (@idOplata, @idViplPut, @nameProc, @symma)";

            cmd.Parameters.Add(new SqlParameter("@idOplata",  req.OplataId));
            cmd.Parameters.Add(new SqlParameter("@idViplPut", (object?)req.IdViplPut ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@nameProc",  (object?)req.NameProc  ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@symma",     req.Symma));

            var newId = (long)await cmd.ExecuteScalarAsync()!;
            return Ok(new { id = newId });
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    // -----------------------------------------------------------------------
    // DELETE /api/oplata-viplati/detail/{id} — remove a single detail line
    // -----------------------------------------------------------------------
    [HttpDelete("detail/{id:long}")]
    public async Task<IActionResult> RemoveOplataDetail(long id)
    {
        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM tblOplata_Rasxod WHERE ID = @id";
            cmd.Parameters.Add(new SqlParameter("@id", id));
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows == 0 ? NotFound() : NoContent();
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    // -----------------------------------------------------------------------
    // GET /api/oplata-viplati/saldo?orgId=&year=&quarter=
    // Saldo (running balance) for an org in the given period
    // -----------------------------------------------------------------------
    [HttpGet("saldo")]
    public async Task<IActionResult> GetOplataSaldo(
        [FromQuery] long orgId,
        [FromQuery] int year,
        [FromQuery] int quarter)
    {
        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT
                    ISNULL(SUM(Symma_DT), 0) AS TotalDt,
                    ISNULL(SUM(Symma_KT), 0) AS TotalKt,
                    ISNULL(SUM(Symma_DT - Symma_KT), 0) AS Saldo
                FROM tblOplata
                WHERE ID_Org = @orgId
                  AND SmFil  = @year
                  AND Id_Kv  = @quarter";

            cmd.Parameters.Add(new SqlParameter("@orgId",   orgId));
            cmd.Parameters.Add(new SqlParameter("@year",    year));
            cmd.Parameters.Add(new SqlParameter("@quarter", quarter));

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return Ok(new
                {
                    totalDt  = (decimal)reader["TotalDt"],
                    totalKt  = (decimal)reader["TotalKt"],
                    saldo    = (decimal)reader["Saldo"]
                });
            }

            return Ok(new { totalDt = 0m, totalKt = 0m, saldo = 0m });
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    // -----------------------------------------------------------------------
    // GET /api/oplata-viplati/general-remaining?orgId=
    // Total remaining balance across all periods for an org
    // -----------------------------------------------------------------------
    [HttpGet("general-remaining")]
    public async Task<IActionResult> GetGeneralRemainingByOrg([FromQuery] long orgId)
    {
        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT ISNULL(SUM(Symma_DT - Symma_KT), 0) AS Remaining
                FROM tblOplata
                WHERE ID_Org = @orgId";

            cmd.Parameters.Add(new SqlParameter("@orgId", orgId));
            var result = await cmd.ExecuteScalarAsync();
            decimal remaining = result == DBNull.Value ? 0m : (decimal)result!;
            return Ok(new { orgId, remaining });
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    // -----------------------------------------------------------------------
    // GET /api/oplata-viplati/total-debt?orgId=
    // Total payment debt (DT side) for an org across all periods
    // -----------------------------------------------------------------------
    [HttpGet("total-debt")]
    public async Task<IActionResult> GetTotalDebtByOrg([FromQuery] long orgId)
    {
        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT ISNULL(SUM(Symma_DT), 0) AS TotalDebt
                FROM tblOplata
                WHERE ID_Org = @orgId";

            cmd.Parameters.Add(new SqlParameter("@orgId", orgId));
            var result = await cmd.ExecuteScalarAsync();
            decimal totalDebt = result == DBNull.Value ? 0m : (decimal)result!;
            return Ok(new { orgId, totalDebt });
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }

    // -----------------------------------------------------------------------
    // GET /api/oplata-viplati/not-covered-putyovka?orgId=
    // Voucher distribution lines not yet fully covered by payments
    // -----------------------------------------------------------------------
    [HttpGet("not-covered-putyovka")]
    public async Task<IActionResult> GetNotCoveredPutyovka([FromQuery] long orgId)
    {
        var conn = db.Database.GetDbConnection();
        await db.Database.OpenConnectionAsync();

        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT
                    vp.ID,
                    vp.ID_San,
                    s.Name_San,
                    vp.ID_Put,
                    p.SERIAL_BEGIN   AS Num_Put,
                    vp.Symma         AS TotalSymma,
                    ISNULL(
                        (SELECT SUM(r.Symma) FROM tblOplata_Rasxod r WHERE r.IdVipl_Put = vp.ID),
                        0)             AS PaidSymma,
                    vp.Symma - ISNULL(
                        (SELECT SUM(r.Symma) FROM tblOplata_Rasxod r WHERE r.IdVipl_Put = vp.ID),
                        0)             AS Remaining
                FROM tblVipl_Put vp
                LEFT JOIN tblSprav_San  s  ON s.ID      = vp.ID_San
                LEFT JOIN tbpt_putyovki p  ON p.REC_ID  = vp.ID_Put
                WHERE vp.ID_Org = @orgId
                  AND ISNULL(vp.Symma, 0) > ISNULL(
                        (SELECT SUM(r.Symma) FROM tblOplata_Rasxod r WHERE r.IdVipl_Put = vp.ID), 0)
                ORDER BY vp.ID";

            cmd.Parameters.Add(new SqlParameter("@orgId", orgId));

            var rows = new List<object>();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    rows.Add(new
                    {
                        id          = (long)reader["ID"],
                        idSan       = reader["ID_San"]      == DBNull.Value ? (long?)null : (long)reader["ID_San"],
                        sanName     = reader["Name_San"]    == DBNull.Value ? null : (string)reader["Name_San"],
                        idPut       = reader["ID_Put"]      == DBNull.Value ? (long?)null : (long)reader["ID_Put"],
                        numPut      = reader["Num_Put"]     == DBNull.Value ? null : (string)reader["Num_Put"],
                        totalSymma  = reader["TotalSymma"]  == DBNull.Value ? 0m   : (decimal)reader["TotalSymma"],
                        paidSymma   = reader["PaidSymma"]   == DBNull.Value ? 0m   : (decimal)reader["PaidSymma"],
                        remaining   = reader["Remaining"]   == DBNull.Value ? 0m   : (decimal)reader["Remaining"]
                    });
                }
            }

            return Ok(rows);
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }
}
