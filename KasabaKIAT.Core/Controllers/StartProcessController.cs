using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/start-process")]
[Authorize]
public class StartProcessController(KasabaKIATContext db) : ControllerBase
{
    // ── Balance Saldo ─────────────────────────────────────────────────────

    [HttpGet("balance-saldo")]
    public async Task<IActionResult> GetBalanceSaldo([FromQuery] int year, [FromQuery] int kv)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TbbuBalances
            .Where(b => b.CUR_YEAR == year && b.CUR_KV == kv);

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(b => b.DEPARTMENT_BASE == depBase);

        var items = await query.OrderBy(b => b.ACCOUNT_CODE).ToListAsync();
        return Ok(items);
    }

    [HttpPost("balance-saldo")]
    public async Task<IActionResult> UpsertBalanceSaldo([FromBody] TbbuBalance model)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        if (!isSuperAdmin) model.DEPARTMENT_BASE = depBase;

        var existing = await db.TbbuBalances
            .Where(b => b.DEPARTMENT_BASE == model.DEPARTMENT_BASE
                     && b.ACCOUNT_CODE == model.ACCOUNT_CODE
                     && b.CUR_YEAR == model.CUR_YEAR
                     && b.CUR_KV == model.CUR_KV)
            .FirstOrDefaultAsync();

        if (existing != null)
        {
            existing.DEBIT_BAL = model.DEBIT_BAL;
            existing.CREDIT_BAL = model.CREDIT_BAL;
            existing.DEBIT_COUNT = 0;
            existing.CREDIT_COUNT = 0;
            existing.DEBIT_TRANS = 0;
            existing.CREDIT_TRANS = 0;
        }
        else
        {
            model.ID = 0;
            model.DEBIT_COUNT = 0;
            model.CREDIT_COUNT = 0;
            model.DEBIT_TRANS = 0;
            model.CREDIT_TRANS = 0;
            db.TbbuBalances.Add(model);
        }

        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("balance-saldo/{id}")]
    public async Task<IActionResult> DeleteBalanceSaldo(long id)
    {
        var item = await db.TbbuBalances.FindAsync(id);
        if (item == null) return NotFound();
        db.TbbuBalances.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ── Debitor / Creditor ────────────────────────────────────────────────

    [HttpGet("debitor-creditor")]
    public async Task<IActionResult> GetDebitorCreditor([FromQuery] int year, [FromQuery] int kv)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TbbuDebitorCreditors
            .Where(d => d.CUR_YEAR == year && d.CUR_KV == kv);

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(d => d.DEPARTMENT_BASE == depBase);

        var items = await query
            .Select(d => new
            {
                d.ID,
                d.DEPARTMENT_BASE,
                d.ACCOUNT_CODE,
                d.CUR_YEAR,
                d.CUR_KV,
                d.DEBIT_BAL,
                d.CREDIT_BAL,
                d.ORG_ID,
                OrgName = db.TblSpravOrgs
                    .Where(o => o.ID == d.ORG_ID)
                    .Select(o => o.Name_Org)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost("debitor-creditor")]
    public async Task<IActionResult> UpsertDebitorCreditor([FromBody] TbbuDebitorCreditor model)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        if (!isSuperAdmin) model.DEPARTMENT_BASE = depBase;

        var existing = await db.TbbuDebitorCreditors
            .Where(d => d.DEPARTMENT_BASE == model.DEPARTMENT_BASE
                     && d.ACCOUNT_CODE == model.ACCOUNT_CODE
                     && d.CUR_YEAR == model.CUR_YEAR
                     && d.CUR_KV == model.CUR_KV
                     && d.ORG_ID == model.ORG_ID)
            .FirstOrDefaultAsync();

        if (existing != null)
        {
            existing.DEBIT_BAL = model.DEBIT_BAL;
            existing.CREDIT_BAL = model.CREDIT_BAL;
        }
        else
        {
            model.ID = 0;
            db.TbbuDebitorCreditors.Add(model);
        }

        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("debitor-creditor/{id}")]
    public async Task<IActionResult> DeleteDebitorCreditor(long id)
    {
        var item = await db.TbbuDebitorCreditors.FindAsync(id);
        if (item == null) return NotFound();
        db.TbbuDebitorCreditors.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ── Putyovka Ostatok (voucher opening balance) ────────────────────────

    [HttpGet("putyovka-ostatok")]
    public async Task<IActionResult> GetPutyovkaOstatok([FromQuery] int year, [FromQuery] int kv)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        var query = db.TbptPutyovkaOstatokLoaders
            .Where(p => p.CUR_YEAR == year && p.CUR_KV == kv);

        if (!isSuperAdmin && !string.IsNullOrEmpty(depBase))
            query = query.Where(p => p.DEPARTMENT_BASE == depBase);

        var items = await query
            .Select(p => new
            {
                p.ID,
                p.DEPARTMENT_BASE,
                p.ID_SANAT,
                SanatName = db.TblSpravSans
                    .Where(s => s.ID == p.ID_SANAT)
                    .Select(s => s.Name_San)
                    .FirstOrDefault(),
                p.NUM_PUT,
                p.PRICE,
                p.CUR_YEAR,
                p.CUR_KV
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost("putyovka-ostatok")]
    public async Task<IActionResult> CreatePutyovkaOstatok([FromBody] TbptPutyovkaOstatokLoader model)
    {
        var depBase = User.FindFirstValue("bkut_id");
        var isSuperAdmin = User.FindFirstValue("is_super_admin") == "True";

        if (!isSuperAdmin) model.DEPARTMENT_BASE = depBase;

        model.ID = 0;
        db.TbptPutyovkaOstatokLoaders.Add(model);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPutyovkaOstatok),
            new { year = model.CUR_YEAR, kv = model.CUR_KV }, model);
    }

    [HttpDelete("putyovka-ostatok/{id}")]
    public async Task<IActionResult> DeletePutyovkaOstatok(long id)
    {
        var item = await db.TbptPutyovkaOstatokLoaders.FindAsync(id);
        if (item == null) return NotFound();
        db.TbptPutyovkaOstatokLoaders.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ── Upload templates ──────────────────────────────────────────────────

    [HttpGet("templates/putyovki")]
    public IActionResult DownloadPutyovkiTemplate()
    {
        var header = "GodZaezdov\tSanatoryKod\tSanatoryNaimenovanie\tSanatoryOblast\tSanatoryRazmeshenieKod\tPutevok1kv\tPutevok2kv\tPutevok3kv\tPutevok4kv\r\n";
        var bytes = System.Text.Encoding.UTF8.GetBytes(header);
        return File(bytes, "text/tab-separated-values", "putyovki_template.txt");
    }

    [HttpGet("templates/putyovki-kengash")]
    public IActionResult DownloadPutyovkiKengashTemplate()
    {
        var header = "GodZaezdov\tSanatoryKod\tSanatoryNaimenovanie\tKengashKod\tPutevok1kv\tPutevok2kv\tPutevok3kv\tPutevok4kv\r\n";
        var bytes = System.Text.Encoding.UTF8.GetBytes(header);
        return File(bytes, "text/tab-separated-values", "putyovki_kengash_template.txt");
    }

    [HttpGet("templates/balance")]
    public IActionResult DownloadBalanceTemplate()
    {
        var header = "SanatKod\tOblKod\tSaldoDebit\tSaldoCredit\r\n";
        var bytes = System.Text.Encoding.UTF8.GetBytes(header);
        return File(bytes, "text/tab-separated-values", "balance_template.txt");
    }

    [HttpGet("templates/debitor")]
    public IActionResult DownloadDebitorTemplate()
    {
        var header = "OrgINN\tOrgName\tDebitSumma\tCreditSumma\tPeriod\r\n";
        var bytes = System.Text.Encoding.UTF8.GetBytes(header);
        return File(bytes, "text/tab-separated-values", "debitor_template.txt");
    }
}
