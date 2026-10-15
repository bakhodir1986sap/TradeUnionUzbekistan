using KasabaKIAT.Core.DTOs.Reports;
using KasabaKIAT.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController(IReportService reports, IFinOtchetService finOtchet, IGlavnayaKniService glavnayaKni) : ControllerBase
{
    private string GetDepBase() => User.FindFirstValue("dep_base") ?? string.Empty;

    // ---- Existing 8 endpoints ----

    [HttpGet("svodniy-otchot")]
    public async Task<IActionResult> GetSvodniyOtchot(
        [FromQuery] DateTime? beginDt, [FromQuery] DateTime? endDt,
        [FromQuery] string? soato, [FromQuery] string? branch,
        CancellationToken ct)
    {
        var data = await reports.GetSvodniyOtchotAsync(beginDt, endDt, soato, branch, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("postupleniya")]
    public async Task<IActionResult> GetPostupleniya([FromQuery] string? inn, CancellationToken ct)
    {
        var data = await reports.GetPostupleniyaAsync(inn, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("postupleniya-n")]
    public async Task<IActionResult> GetPostupleniyaN(
        [FromQuery] string? inn, [FromQuery] DateTime? zb, [FromQuery] DateTime? ze,
        CancellationToken ct)
    {
        var data = await reports.GetPostupleniyaNAsync(inn, GetDepBase(), zb, ze, ct);
        return Ok(data);
    }

    [HttpGet("provodki-details")]
    public async Task<IActionResult> GetProvodkiDetails(
        [FromQuery] DateTime? bg, [FromQuery] DateTime? end,
        CancellationToken ct)
    {
        var data = await reports.GetProvodkiDetailsAsync(GetDepBase(), bg, end, ct);
        return Ok(data);
    }

    [HttpGet("provodki-summary")]
    public async Task<IActionResult> GetProvodkiSummary(
        [FromQuery] DateTime? bg, [FromQuery] DateTime? end,
        CancellationToken ct)
    {
        var data = await reports.GetProvodkiSummaryAsync(GetDepBase(), bg, end, ct);
        return Ok(data);
    }

    [HttpGet("talabnomalar-korxona")]
    public async Task<IActionResult> GetTalabnomlarKorxona([FromQuery] int yil, [FromQuery] string? inn, CancellationToken ct)
    {
        var data = await reports.GetTalabnomlarKorxonaAsync(yil, inn, ct);
        return Ok(data);
    }

    [HttpGet("talabnomalar-tarmoq")]
    public async Task<IActionResult> GetTalabnomlarTarmoq(
        [FromQuery] int yil, [FromQuery] string? soato, [FromQuery] string? branch,
        CancellationToken ct)
    {
        var data = await reports.GetTalabnomlarTarmoqAsync(yil, soato, branch, ct);
        return Ok(data);
    }

    [HttpGet("forma13")]
    public async Task<IActionResult> GetForma13([FromQuery] int yil, [FromQuery] string? inn, CancellationToken ct)
    {
        var data = await reports.GetForma13Async(yil, inn, GetDepBase(), ct);
        return Ok(data);
    }

    // ---- Group A — Putyovki ----

    [HttpGet("operativka-putyovki")]
    public async Task<IActionResult> GetOperativkaPutyovki(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate, [FromQuery] int? sanatId,
        CancellationToken ct)
    {
        var data = await reports.GetOperativkaPutyovkiAsync(GetDepBase(), beginDate, endDate, sanatId, ct);
        return Ok(data);
    }

    [HttpGet("operativka-ostatok")]
    public async Task<IActionResult> GetOperativkaOstatok(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate, [FromQuery] int? sanatId,
        CancellationToken ct)
    {
        var data = await reports.GetOperativkaOstatokAsync(beginDate, endDate, sanatId, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("operativka-prixod")]
    public async Task<IActionResult> GetOperativkaPrixod(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate, [FromQuery] int? sanatId,
        CancellationToken ct)
    {
        var data = await reports.GetOperativkaPrixodAsync(beginDate, endDate, sanatId, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("operativka-rasxod")]
    public async Task<IActionResult> GetOperativkaRasxod(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate, [FromQuery] int? sanatId,
        CancellationToken ct)
    {
        var data = await reports.GetOperativkaRasxodAsync(beginDate, endDate, sanatId, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("operativka-vozvrat")]
    public async Task<IActionResult> GetOperativkaVozvrat(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate, [FromQuery] int? sanatId,
        CancellationToken ct)
    {
        var data = await reports.GetOperativkaVozvratAsync(beginDate, endDate, sanatId, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("svod-po-putyovkam")]
    public async Task<IActionResult> GetSvodPoPutyovkam(
        [FromQuery] int? year, [FromQuery] int? kvartal, [FromQuery] string? otras,
        CancellationToken ct)
    {
        var data = await reports.GetSvodPoPutyovkamAsync(GetDepBase(), year, kvartal, otras, ct);
        return Ok(data);
    }

    [HttpGet("svodny-otchet-putyovki")]
    public async Task<IActionResult> GetSvodniyOtchetPutyovki(
        [FromQuery] int? kvartal, [FromQuery] string? smfil,
        CancellationToken ct)
    {
        var data = await reports.GetSvodniyOtchetPutyovkiAsync(GetDepBase(), kvartal, smfil, ct);
        return Ok(data);
    }

    [HttpGet("zapros-putyovki")]
    public async Task<IActionResult> GetZaprosPutyovki(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate,
        [FromQuery] long? idOrg, [FromQuery] int? idSanat,
        [FromQuery] string? lastName, [FromQuery] string? numPut, [FromQuery] string? pol,
        CancellationToken ct)
    {
        var data = await reports.GetZaprosPutyovkiAsync(GetDepBase(), beginDate, endDate, idOrg, idSanat, lastName, numPut, pol, ct);
        return Ok(data);
    }

    [HttpGet("kniga-vladelsev")]
    public async Task<IActionResult> GetKnigaVladelsev(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate,
        CancellationToken ct)
    {
        var data = await reports.GetKnigaVladelsevAsync(beginDate, endDate, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("forma14")]
    public async Task<IActionResult> GetForma14(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate,
        CancellationToken ct)
    {
        var data = await reports.GetForma14Async(beginDate, endDate, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("tekushiye-ostatok")]
    public async Task<IActionResult> GetTekushiyeOstatok(CancellationToken ct)
    {
        var data = await reports.GetTekushiyeOstatokAsync(GetDepBase(), ct);
        return Ok(data);
    }

    // ---- Group B — Financial ----

    [HttpGet("postupleniya-esp-otrasli")]
    public async Task<IActionResult> GetPostupleniyaEspOtrasli(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate,
        CancellationToken ct)
    {
        var data = await reports.GetPostupleniyaEspOtrasliAsync(beginDate, endDate, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("postupleniya-svodnoye")]
    public async Task<IActionResult> GetPostupleniyaSvodnoye(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate, [FromQuery] string? department,
        CancellationToken ct)
    {
        var data = await reports.GetPostupleniyaSvodnoyeAsync(beginDate, endDate, department, ct);
        return Ok(data);
    }

    [HttpGet("oborotka-regionam")]
    public async Task<IActionResult> GetOborotkaRegionam(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate, [FromQuery] string? soato,
        CancellationToken ct)
    {
        var data = await reports.GetOborotkaRegionamAsync(GetDepBase(), beginDate, endDate, soato, ct);
        return Ok(data);
    }

    [HttpGet("balans-predpriyatiy")]
    public async Task<IActionResult> GetBalansPredpriyatiy(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate,
        CancellationToken ct)
    {
        var data = await reports.GetBalansPredpriyatiyAsync(beginDate, endDate, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("debitorskaya")]
    public async Task<IActionResult> GetDebitorskaya(CancellationToken ct)
    {
        var data = await reports.GetDebitorskayaAsync(GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("organizasii-otrasli")]
    public async Task<IActionResult> GetOrganizasiiOtrasli(CancellationToken ct)
    {
        var data = await reports.GetOrganizasiiOtrasliAsync(GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("organizasii-rayony")]
    public async Task<IActionResult> GetOrganizasiiRayony(CancellationToken ct)
    {
        var data = await reports.GetOrganizasiiRayonyAsync(GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("spisok-org")]
    public async Task<IActionResult> GetSpisokOrg(CancellationToken ct)
    {
        var data = await reports.GetSpisokOrgAsync(GetDepBase(), ct);
        return Ok(data);
    }

    // ---- Group C — Journal Orders ----

    [HttpGet("jurorder/4")]
    public async Task<IActionResult> GetJurOrder4(
        [FromQuery] int year, [FromQuery] int kvartal,
        CancellationToken ct)
    {
        var data = await reports.GetJurOrder4Async(year, kvartal, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("jurorder/5")]
    public async Task<IActionResult> GetJurOrder5(
        [FromQuery] int year, [FromQuery] int kvartal,
        CancellationToken ct)
    {
        var data = await reports.GetJurOrder5Async(year, kvartal, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("jurorder/6")]
    public async Task<IActionResult> GetJurOrder6(
        [FromQuery] int year, [FromQuery] int kvartal,
        CancellationToken ct)
    {
        var data = await reports.GetJurOrder6Async(year, kvartal, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("jurorder")]
    public async Task<IActionResult> GetJurOrder(
        [FromQuery] int jorType, [FromQuery] DateTime? bg, [FromQuery] DateTime? end,
        CancellationToken ct)
    {
        var data = await reports.GetJurOrderOtchetiAsync(GetDepBase(), jorType, bg, end, ct);
        return Ok(data);
    }

    // ---- Group D — Monitoring ----

    [HttpGet("monitoring/oxvat")]
    public async Task<IActionResult> GetMonitoringOxvat(
        [FromQuery] int? year, [FromQuery] int? kvartal,
        CancellationToken ct)
    {
        var data = await reports.GetMonitoringOxvatAsync(GetDepBase(), year, kvartal, ct);
        return Ok(data);
    }

    [HttpGet("monitoring/narusheniya")]
    public async Task<IActionResult> GetMonitoringNarusheniya(
        [FromQuery] int? year, [FromQuery] int? kvartal,
        CancellationToken ct)
    {
        var data = await reports.GetMonitoringNarusheniyaAsync(GetDepBase(), year, kvartal, ct);
        return Ok(data);
    }

    [HttpGet("monitoring/pererasxod-plan")]
    public async Task<IActionResult> GetMonitoringPererasxodPlan(
        [FromQuery] int? year, [FromQuery] int? kvartal,
        CancellationToken ct)
    {
        var data = await reports.GetMonitoringPererasxodPlanAsync(GetDepBase(), year, kvartal, ct);
        return Ok(data);
    }

    [HttpGet("monitoring/pererasxod-putyovki")]
    public async Task<IActionResult> GetMonitoringPererasxodPutyovki(
        [FromQuery] int? year, [FromQuery] int? kvartal,
        CancellationToken ct)
    {
        var data = await reports.GetMonitoringPererasxodPutyovkiAsync(GetDepBase(), year, kvartal, ct);
        return Ok(data);
    }

    [HttpGet("monitoring/kaznacheystvo")]
    public async Task<IActionResult> GetMonitoringKaznacheystvo(
        [FromQuery] int? year, [FromQuery] int? kvartal,
        CancellationToken ct)
    {
        var data = await reports.GetMonitoringKaznacheystvAsync(GetDepBase(), year, kvartal, ct);
        return Ok(data);
    }

    // ---- Group E — Organization Cards ----

    [HttpGet("kartochka-org")]
    public async Task<IActionResult> GetKartochkaOrg(
        [FromQuery] string? orgInn, [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate,
        CancellationToken ct)
    {
        var data = await reports.GetKartochkaOrgAsync(orgInn, GetDepBase(), beginDate, endDate, ct);
        return Ok(data);
    }

    [HttpGet("kartochka-history")]
    public async Task<IActionResult> GetKartochkaHistory(
        [FromQuery] long? idOrg, [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate,
        CancellationToken ct)
    {
        var data = await reports.GetKartochkaHistoryAsync(idOrg, beginDate, endDate, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("kartochka-oplata")]
    public async Task<IActionResult> GetKartochkaOplata(
        [FromQuery] long? idOrg, [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate,
        CancellationToken ct)
    {
        var data = await reports.GetKartochkaOplataAsync(idOrg, beginDate, endDate, GetDepBase(), ct);
        return Ok(data);
    }

    // ---- Group F — Medical / Other ----

    [HttpGet("med-registraciya")]
    public async Task<IActionResult> GetMedRegistraciya(
        [FromQuery] long? idOrg, [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate,
        CancellationToken ct)
    {
        var data = await reports.GetMedRegistraciyaAsync(GetDepBase(), idOrg, beginDate, endDate, ct);
        return Ok(data);
    }

    [HttpGet("uchet-stoimosti")]
    public async Task<IActionResult> GetUchetStoimosti(
        [FromQuery] DateTime? beginDate, [FromQuery] DateTime? endDate,
        CancellationToken ct)
    {
        var data = await reports.GetUchetStoimostiAsync(beginDate, endDate, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("poisk-putyovki")]
    public async Task<IActionResult> GetPoiskPutyovki([FromQuery] string? numPut, CancellationToken ct)
    {
        var data = await reports.GetPoiskPutyovkiAsync(numPut, GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("ostatok-nizhestvuyushix")]
    public async Task<IActionResult> GetOstatokNizhestvuyushix(CancellationToken ct)
    {
        var data = await reports.GetOstatokNizhestvuyushixAsync(GetDepBase(), ct);
        return Ok(data);
    }

    [HttpGet("talabnomalar-viloyat")]
    public async Task<IActionResult> GetTalabnomlarViloyat([FromQuery] int yil, [FromQuery] string? soato, CancellationToken ct)
    {
        var data = await reports.GetTalabnomlarViloyatAsync(yil, soato, ct);
        return Ok(data);
    }

    // ---- Group G — FinOtchet / GlavnayaKniga ----

    [HttpGet("fin-otchet")]
    public async Task<IActionResult> GetFinOtchet(
        [FromQuery] int year, [FromQuery] int kvartal, [FromQuery] bool isSvodniy,
        CancellationToken ct)
    {
        var data = await finOtchet.GetFinOtchetAsync(year, kvartal, GetDepBase(), isSvodniy, ct);
        return Ok(data);
    }

    [HttpGet("glavnaya-kniga")]
    public async Task<IActionResult> GetGlavnayaKniga(
        [FromQuery] int year, [FromQuery] int kvartal, [FromQuery] bool isSvodniy,
        CancellationToken ct)
    {
        var data = await glavnayaKni.GetGlavnayaKnigaAsync(year, kvartal, GetDepBase(), isSvodniy, ct);
        return Ok(data);
    }
}
