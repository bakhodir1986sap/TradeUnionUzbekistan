using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

/// <summary>
/// Chastichka (partial payment) — processes per-region partial voucher payments.
/// Ported from BusinessLogic/Buxgalteriya/Chastichka/*
/// </summary>
[ApiController]
[Route("api/chastichka")]
[Authorize]
public class ChastichkaController(KasabaKIATContext db) : ControllerBase
{
    public record ChastichkaInputRow(
        string OrgInn,
        string OrgName,
        decimal Summa,
        string? Description);

    public record ChastichkaResult(
        string OrgInn,
        string OrgName,
        decimal Summa,
        string DebitAccount,
        string CreditAccount,
        string? Description,
        bool Success,
        string? Error);

    // ── Process partial payment ───────────────────────────────────────────
    [HttpPost("process")]
    public async Task<IActionResult> Process([FromBody] ChastichkaRequest req)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        var results = new List<ChastichkaResult>();

        foreach (var row in req.Rows)
        {
            try
            {
                var org = await db.TblSpravOrgs
                    .Where(o => o.INN == row.OrgInn && o.DepartmentBase == depBase)
                    .FirstOrDefaultAsync();

                if (org == null)
                {
                    results.Add(new ChastichkaResult(row.OrgInn, row.OrgName, row.Summa,
                        req.DebitAccount, req.CreditAccount, row.Description,
                        false, $"Organisation with INN {row.OrgInn} not found."));
                    continue;
                }

                var creditAcc = GetRegionalCreditAccount(depBase, req.CreditAccount);

                var provodok = new Provodok
                {
                    score_begin = req.DebitAccount,
                    score_end = creditAcc,
                    description = $"{row.Summa:F2}|{row.Description ?? row.OrgName}|{req.Year}/{req.Kvartal}",
                    USER_ID = userId,
                    DEPARTMENT_BASE = depBase
                };

                db.Provodoks.Add(provodok);

                results.Add(new ChastichkaResult(row.OrgInn, row.OrgName, row.Summa,
                    req.DebitAccount, creditAcc, row.Description, true, null));
            }
            catch (Exception ex)
            {
                results.Add(new ChastichkaResult(row.OrgInn, row.OrgName, row.Summa,
                    req.DebitAccount, req.CreditAccount, row.Description,
                    false, ex.Message));
            }
        }

        await db.SaveChangesAsync();

        return Ok(new
        {
            processed = results.Count(r => r.Success),
            failed = results.Count(r => !r.Success),
            results
        });
    }

    // ── Federasiya: process from uploaded file data ───────────────────────
    [HttpPost("federasiya/process")]
    public async Task<IActionResult> ProcessFederasiya([FromBody] FederasiyaChastichkaRequest req)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        var results = new List<object>();

        foreach (var row in req.Rows)
        {
            var org = await db.TblSpravOrgs
                .Where(o => o.INN == row.Inn && o.DepartmentBase == depBase)
                .FirstOrDefaultAsync();

            if (org == null)
            {
                results.Add(new { row.Inn, row.Name, success = false, error = "Org not found" });
                continue;
            }

            var provodok = new Provodok
            {
                score_begin = row.DebitAccount ?? "6010",
                score_end = row.CreditAccount ?? "6700",
                description = $"{row.Summa:F2}|Федерация частичная: {row.Name}|{req.Year}/{req.Kvartal}",
                USER_ID = userId,
                DEPARTMENT_BASE = depBase
            };

            db.Provodoks.Add(provodok);
            results.Add(new { row.Inn, row.Name, row.Summa, success = true });
        }

        await db.SaveChangesAsync();
        return Ok(new { total = req.Rows.Count, results });
    }

    // ── Get chastichka provodki ───────────────────────────────────────────
    [HttpGet]
    public async Task<IActionResult> GetChastichka([FromQuery] string? depCode)
    {
        var depBase = depCode ?? User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.Provodoks.AsQueryable();

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DEPARTMENT_BASE == depBase);

        var items = await query
            .Select(p => new
            {
                p.id,
                p.score_begin,
                p.score_end,
                p.description,
                p.DEPARTMENT_BASE
            })
            .ToListAsync();

        return Ok(items);
    }

    private static string GetRegionalCreditAccount(string? depBase, string defaultAccount)
    {
        if (string.IsNullOrEmpty(depBase)) return defaultAccount;
        if (depBase.StartsWith("AND", StringComparison.OrdinalIgnoreCase)) return "6710";
        if (depBase.StartsWith("BUX", StringComparison.OrdinalIgnoreCase)) return "6720";
        if (depBase.StartsWith("XOR", StringComparison.OrdinalIgnoreCase)) return "6730";
        return defaultAccount;
    }
}

public class ChastichkaRequest
{
    public int Year { get; set; }
    public int Kvartal { get; set; }
    public string DebitAccount { get; set; } = "6010";
    public string CreditAccount { get; set; } = "6700";
    public List<ChastichkaController.ChastichkaInputRow> Rows { get; set; } = [];
}

public class FederasiyaChastichkaRequest
{
    public int Year { get; set; }
    public int Kvartal { get; set; }
    public List<FederasiyaRow> Rows { get; set; } = [];
}

public class FederasiyaRow
{
    public string Inn { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Summa { get; set; }
    public string? DebitAccount { get; set; }
    public string? CreditAccount { get; set; }
}
