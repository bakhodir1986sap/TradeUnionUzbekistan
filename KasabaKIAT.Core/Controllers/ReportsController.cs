using KasabaKIAT.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController(KasabaKIATContext db) : ControllerBase
{
    private string GetDepBase() => User.FindFirstValue("bkut_id") ?? string.Empty;

    // GET /api/reports/svodniy-otchot?beginDt=...&endDt=...&soato=...&branch=...
    [HttpGet("svodniy-otchot")]
    public async Task<IActionResult> GetSvodniyOtchot(
        [FromQuery] DateTime? beginDt,
        [FromQuery] DateTime? endDt,
        [FromQuery] string? soato,
        [FromQuery] string? branch)
    {
        var depBase = GetDepBase();
        var data = await db.Database
            .SqlQueryRaw<SvodniyOtchotRow>(
                "EXEC sp_rt_SvodniyOtchet @p0, @p1, @p2, @p3, @p4",
                beginDt, endDt, soato, branch, depBase)
            .ToListAsync();
        return Ok(data);
    }

    // GET /api/reports/postupleniya?inn=...
    [HttpGet("postupleniya")]
    public async Task<IActionResult> GetPostupleniya([FromQuery] string? inn)
    {
        var depBase = GetDepBase();
        var data = await db.Database
            .SqlQueryRaw<PostupleniyaRow>(
                "EXEC sp_rt_Postupleniya @p0, @p1",
                inn, depBase)
            .ToListAsync();
        return Ok(data);
    }

    // GET /api/reports/postupleniya-n?inn=...&zb=...&ze=...
    [HttpGet("postupleniya-n")]
    public async Task<IActionResult> GetPostupleniyaN(
        [FromQuery] string? inn,
        [FromQuery] DateTime? zb,
        [FromQuery] DateTime? ze)
    {
        var depBase = GetDepBase();
        var data = await db.Database
            .SqlQueryRaw<PostupleniyaNRow>(
                "EXEC sp_rt_PostupleniyaN2 @p0, @p1, @p2, @p3",
                inn, depBase, zb, ze)
            .ToListAsync();
        return Ok(data);
    }

    // GET /api/reports/provodki-details?bg=...&end=...
    [HttpGet("provodki-details")]
    public async Task<IActionResult> GetProvodkiDetails(
        [FromQuery] DateTime? bg,
        [FromQuery] DateTime? end)
    {
        var depBase = GetDepBase();
        var data = await db.Database
            .SqlQueryRaw<ProvodkiDetailRow>(
                "EXEC sp_rt_Provodka_Details @p0, @p1, @p2",
                depBase, bg, end)
            .ToListAsync();
        return Ok(data);
    }

    // GET /api/reports/provodki-summary?bg=...&end=...
    [HttpGet("provodki-summary")]
    public async Task<IActionResult> GetProvodkiSummary(
        [FromQuery] DateTime? bg,
        [FromQuery] DateTime? end)
    {
        var depBase = GetDepBase();
        var data = await db.Database
            .SqlQueryRaw<ProvodkiSummaryRow>(
                "EXEC sp_rt_Provodka_Summary @p0, @p1, @p2",
                depBase, bg, end)
            .ToListAsync();
        return Ok(data);
    }

    // GET /api/reports/talabnomalar-korxona?yil=...&inn=...
    [HttpGet("talabnomalar-korxona")]
    public async Task<IActionResult> GetTalabnomlarKorxona(
        [FromQuery] int yil,
        [FromQuery] string? inn)
    {
        var data = await db.Database
            .SqlQueryRaw<TalabnomRow>(
                "EXEC sp_rt_TalabnomaKorxona @p0, @p1",
                yil, inn)
            .ToListAsync();
        return Ok(data);
    }

    // GET /api/reports/talabnomalar-tarmoq?yil=...&soato=...&branch=...
    [HttpGet("talabnomalar-tarmoq")]
    public async Task<IActionResult> GetTalabnomlarTarmoq(
        [FromQuery] int yil,
        [FromQuery] string? soato,
        [FromQuery] string? branch)
    {
        var data = await db.Database
            .SqlQueryRaw<TalabnomRow>(
                "EXEC sp_rt_TalabnomaTarmoq @p0, @p1, @p2",
                yil, soato, branch)
            .ToListAsync();
        return Ok(data);
    }

    // GET /api/reports/forma13?yil=...&inn=...
    [HttpGet("forma13")]
    public async Task<IActionResult> GetForma13([FromQuery] int yil, [FromQuery] string? inn)
    {
        var depBase = GetDepBase();
        var data = await db.Database
            .SqlQueryRaw<Forma13Row>(
                "EXEC sp_rt_FormaN13 @p0, @p1, @p2",
                yil, inn, depBase)
            .ToListAsync();
        return Ok(data);
    }
}

// ---- Raw SP result DTOs ----

public class SvodniyOtchotRow
{
    public string? ProfName { get; set; }
    public string? FullName { get; set; }
    public string? PlaceAndPos { get; set; }
    public DateTime? MedSprevDate { get; set; }
    public DateTime? ZayavkaDate { get; set; }
    public string? SanName { get; set; }
    public DateTime? zaezdBegin { get; set; }
}

public class PostupleniyaRow
{
    public string? K_TYPE { get; set; }
    public string? OPERDAY { get; set; }
    public long? ID { get; set; }
    public string? BANKID { get; set; }
    public decimal? SUMPAY { get; set; }
    public string? CLMFO { get; set; }
    public string? CLACC { get; set; }
    public string? CLINN { get; set; }
    public string? CLNAME { get; set; }
    public string? PURPOSE { get; set; }
}

public class PostupleniyaNRow
{
    public string? OPERDAY { get; set; }
    public string? INN { get; set; }
    public string? CLNAME { get; set; }
    public decimal? POSTUPLENIYA { get; set; }
    public decimal? Obshiy01 { get; set; }
    public decimal? FEDERATION_AMT { get; set; }
    public decimal? OBLSOVPROF_AMT { get; set; }
    public string? PURPOSE { get; set; }
}

public class ProvodkiDetailRow
{
    public long REC_ID { get; set; }
    public string? TRDT { get; set; }
    public string? DEBIT_ACCOUNT { get; set; }
    public string? CREDIT_ACCOUNT { get; set; }
    public decimal? AMOUNT { get; set; }
    public string? DETALI { get; set; }
    public string? ADDITIONAL { get; set; }
    public string? OPISANIYE { get; set; }
    public string? NAME_RU { get; set; }
}

public class ProvodkiSummaryRow
{
    public string? DEBIT_ACCOUNT { get; set; }
    public string? CREDIT_ACCOUNT { get; set; }
    public decimal? AMOUNT { get; set; }
}

public class TalabnomRow
{
    public string? sanName { get; set; }
    public int? KV1 { get; set; }
    public int? KV2 { get; set; }
    public int? KV3 { get; set; }
    public int? KV4 { get; set; }
}

public class Forma13Row
{
    public DateTime? Data_Postupleniya { get; set; }
    public string? Nomer_SchetFacturi { get; set; }
    public string? NameimenovaniyeOrg { get; set; }
    public string? NomerPutyovki { get; set; }
    public DateTime? DataZaezda { get; set; }
    public decimal? SenaPutyovki { get; set; }
    public string? FIO { get; set; }
    public string? MestoRaboti { get; set; }
    public decimal? SymmaPlateja { get; set; }
}
