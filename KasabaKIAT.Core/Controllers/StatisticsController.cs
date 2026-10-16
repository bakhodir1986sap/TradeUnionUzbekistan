using KasabaKIAT.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/statistics")]
[Authorize]
public class StatisticsController(KasabaKIATContext db) : ControllerBase
{
    // ── Dashboard summary (4_Etap FourStepController equivalent) ─────────

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard([FromQuery] int year, [FromQuery] int kv)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        // Active users (users active in the system)
        var activeUserCount = await db.Users.CountAsync(u => u.s_status == "ACTIVE");

        // Bron stats
        var bronQuery = db.TblBrons.AsQueryable();
        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            bronQuery = bronQuery.Where(b => b.DEPARTMENT_BASE == depBase);

        var bronTotal = await bronQuery.CountAsync();
        var bronActive = await bronQuery.CountAsync(b => b.BRON_FLG == true);

        // Putyovki stats
        var putQuery = db.TbptPutyovkis.AsQueryable();
        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            putQuery = putQuery.Where(p => p.DEPARTMENT_CODE == depBase);

        var putYear = putQuery.Where(p => p.REST_BEGIN_DATE != null && p.REST_BEGIN_DATE.Value.Year == year);
        var putTotal = await putYear.SumAsync(p => (int?)p.QUANTITY_IN ?? 0);
        var putRemain = await putYear.SumAsync(p => (int?)p.QUANTITY_REMAIN ?? 0);
        var putUsed = putTotal - putRemain;

        // KU invoice stats
        var kuQuery = db.TblPosts.AsQueryable();
        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            kuQuery = kuQuery.Where(p => p.DepartmentBase == depBase);

        var kuYear = kuQuery.Where(p => p.SmFil == year);
        var kuKv = kuYear.Where(p => p.ID_KV == kv);
        var kuCount = await kuKv.CountAsync();
        var kuSumma = await kuKv.SumAsync(p => p.Symma ?? 0);

        return Ok(new
        {
            year,
            kv,
            activeUsers = activeUserCount,
            bron = new { total = bronTotal, active = bronActive },
            putyovki = new { total = putTotal, used = putUsed, remain = putRemain },
            ku = new { count = kuCount, summa = kuSumma }
        });
    }

    // ── Pie chart: putyovki by sanatorium ────────────────────────────────

    [HttpGet("pie/putyovki-by-sanatorium")]
    public async Task<IActionResult> PutyovkiBySanatorium([FromQuery] int year)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TbptPutyovkis
            .Where(p => p.REST_BEGIN_DATE != null && p.REST_BEGIN_DATE.Value.Year == year);

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DEPARTMENT_CODE == depBase);

        var items = await query
            .GroupBy(p => p.SANATORIUM_ID)
            .Select(g => new
            {
                sanatoriumId = g.Key,
                sanatoriumName = db.TblSpravSans
                    .Where(s => s.ID == g.Key)
                    .Select(s => s.Name_San)
                    .FirstOrDefault(),
                quantityIn = g.Sum(p => p.QUANTITY_IN ?? 0),
                quantityRemain = g.Sum(p => p.QUANTITY_REMAIN ?? 0)
            })
            .ToListAsync();

        return Ok(items);
    }

    // ── Zadoljennost (debt/arrears) stats ─────────────────────────────────

    [HttpGet("zadoljennost")]
    public async Task<IActionResult> GetZadoljennost([FromQuery] int year, [FromQuery] int kv)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var balanceQuery = db.TbbuBalances
            .Where(b => b.CUR_YEAR == year && b.CUR_KV == kv);

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            balanceQuery = balanceQuery.Where(b => b.DEPARTMENT_BASE == depBase);

        var items = await balanceQuery
            .Select(b => new
            {
                b.DEPARTMENT_BASE,
                b.ACCOUNT_CODE,
                debit = (b.DEBIT_BAL ?? 0) + (b.DEBIT_TRANS ?? 0),
                credit = (b.CREDIT_BAL ?? 0) + (b.CREDIT_TRANS ?? 0),
                saldo = ((b.DEBIT_BAL ?? 0) + (b.DEBIT_TRANS ?? 0))
                      - ((b.CREDIT_BAL ?? 0) + (b.CREDIT_TRANS ?? 0))
            })
            .ToListAsync();

        return Ok(items);
    }

    // ── Urgent putyovki (expiring soon) ──────────────────────────────────

    [HttpGet("urgent-putyovki")]
    public async Task<IActionResult> GetUrgentPutyovki([FromQuery] int daysAhead = 7)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var deadline = DateTime.Today.AddDays(daysAhead);

        var query = db.TbptPutyovkis
            .Where(p => p.REST_BEGIN_DATE != null
                     && p.REST_BEGIN_DATE.Value <= deadline
                     && p.REST_BEGIN_DATE.Value >= DateTime.Today
                     && (p.QUANTITY_REMAIN ?? 0) > 0);

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DEPARTMENT_CODE == depBase);

        var items = await query
            .Select(p => new
            {
                p.REC_ID,
                p.SANATORIUM_ID,
                sanatoriumName = db.TblSpravSans
                    .Where(s => s.ID == p.SANATORIUM_ID)
                    .Select(s => s.Name_San).FirstOrDefault(),
                p.REST_BEGIN_DATE,
                p.REST_END_DATE,
                p.QUANTITY_REMAIN,
                p.PRICE
            })
            .OrderBy(p => p.REST_BEGIN_DATE)
            .ToListAsync();

        return Ok(items);
    }

    // ── KaznaInfo (treasury info) ─────────────────────────────────────────

    [HttpGet("kazna-info")]
    public async Task<IActionResult> GetKaznaInfo([FromQuery] int year, [FromQuery] int kv)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var kuQuery = db.TblPosts.AsQueryable();
        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            kuQuery = kuQuery.Where(p => p.DepartmentBase == depBase);

        var kvPosts = await kuQuery
            .Where(p => p.SmFil == year && p.ID_KV == kv)
            .ToListAsync();

        var totalPutyovki = kvPosts.Sum(p => p.Num_Kol ?? 0);
        var totalSumma = kvPosts.Sum(p => p.Symma ?? 0);
        var closedCount = kvPosts.Count(p => p.Vid);
        var openCount = kvPosts.Count(p => !p.Vid);

        return Ok(new
        {
            year,
            kv,
            totalInvoices = kvPosts.Count,
            closed = closedCount,
            open = openCount,
            totalPutyovki,
            totalSumma
        });
    }

    // ── Realization stats ─────────────────────────────────────────────────

    [HttpGet("realizatsiya")]
    public async Task<IActionResult> GetRealizatsiya([FromQuery] int year, [FromQuery] int kv)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var detailQuery = db.TblPosPutDetails
            .Where(d => d.ID_PolVoz == true);

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            detailQuery = detailQuery.Where(d => d.Departmentbase == depBase);

        if (year > 0)
            detailQuery = detailQuery.Where(d => d.Data_Post != null && d.Data_Post.Value.Year == year);

        var total = await detailQuery.CountAsync();
        var summa = await detailQuery.SumAsync(d => d.Symma_Put ?? 0);

        return Ok(new { year, kv, realizedCount = total, realizedSumma = summa });
    }

    // ── 3_Etap: Federasiya level stats ────────────────────────────────────

    [HttpGet("federasiya")]
    public async Task<IActionResult> GetFederasiyaStats([FromQuery] int year, [FromQuery] int kv)
    {
        var orgs = await db.TblSpravOrgs
            .Where(o => o.PRAVORGLEVEL == 1)
            .Select(o => new { o.ID, o.Name_Org, o.DepartmentBase })
            .ToListAsync();

        var result = new List<object>();

        foreach (var org in orgs)
        {
            var putyovki = await db.TbptPutyovkis
                .Where(p => p.DEPARTMENT_CODE == org.DepartmentBase
                         && p.REST_BEGIN_DATE != null
                         && p.REST_BEGIN_DATE.Value.Year == year)
                .SumAsync(p => (int?)p.QUANTITY_IN ?? 0);

            var remain = await db.TbptPutyovkis
                .Where(p => p.DEPARTMENT_CODE == org.DepartmentBase
                         && p.REST_BEGIN_DATE != null
                         && p.REST_BEGIN_DATE.Value.Year == year)
                .SumAsync(p => (int?)p.QUANTITY_REMAIN ?? 0);

            result.Add(new
            {
                orgId = org.ID,
                orgName = org.Name_Org,
                departmentBase = org.DepartmentBase,
                putyovkiTotal = putyovki,
                putyovkiRemain = remain,
                putyovkiUsed = putyovki - remain
            });
        }

        return Ok(result);
    }

    // ── 3_Etap: OblSovProf level stats ───────────────────────────────────

    [HttpGet("obl-sov-prof")]
    public async Task<IActionResult> GetOblSovProfStats([FromQuery] int year, [FromQuery] int kv,
        [FromQuery] string? depBase)
    {
        var filterDep = depBase ?? User.FindFirstValue("bkut_id");

        var orgs = await db.TblSpravOrgs
            .Where(o => o.PRAVORGLEVEL == 2 && o.DepartmentBase == filterDep)
            .Select(o => new { o.ID, o.Name_Org, o.DepartmentBase })
            .ToListAsync();

        var result = orgs.Select(org => new
        {
            orgId = org.ID,
            orgName = org.Name_Org,
            departmentBase = org.DepartmentBase
        });

        return Ok(result);
    }
}
