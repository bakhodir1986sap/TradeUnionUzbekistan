using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs.Reports;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Services;

public class ReportService(KasabaKIATContext db) : IReportService
{
    // ---- Existing 8 endpoints ----

    public Task<List<SvodniyOtchotRow>> GetSvodniyOtchotAsync(DateTime? beginDt, DateTime? endDt, string? soato, string? branch, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<SvodniyOtchotRow>(
            "EXEC sp_rt_SvodniyOtchet @p0, @p1, @p2, @p3, @p4",
            beginDt, endDt, soato, branch, depBase)
        .ToListAsync(ct);

    public Task<List<PostupleniyaRow>> GetPostupleniyaAsync(string? inn, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<PostupleniyaRow>(
            "EXEC sp_rt_Postupleniya @p0, @p1",
            inn, depBase)
        .ToListAsync(ct);

    public Task<List<PostupleniyaNRow>> GetPostupleniyaNAsync(string? inn, string depBase, DateTime? zb, DateTime? ze, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<PostupleniyaNRow>(
            "EXEC sp_rt_PostupleniyaN2 @p0, @p1, @p2, @p3",
            inn, depBase, zb, ze)
        .ToListAsync(ct);

    public Task<List<ProvodkiDetailRow>> GetProvodkiDetailsAsync(string depBase, DateTime? bg, DateTime? end, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<ProvodkiDetailRow>(
            "EXEC sp_rt_Provodka_Details @p0, @p1, @p2",
            depBase, bg, end)
        .ToListAsync(ct);

    public Task<List<ProvodkiSummaryRow>> GetProvodkiSummaryAsync(string depBase, DateTime? bg, DateTime? end, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<ProvodkiSummaryRow>(
            "EXEC sp_rt_Provodka_Summary @p0, @p1, @p2",
            depBase, bg, end)
        .ToListAsync(ct);

    public Task<List<TalabnomRow>> GetTalabnomlarKorxonaAsync(int yil, string? inn, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<TalabnomRow>(
            "EXEC sp_rt_TalabnomaKorxona @p0, @p1",
            yil, inn)
        .ToListAsync(ct);

    public Task<List<TalabnomRow>> GetTalabnomlarTarmoqAsync(int yil, string? soato, string? branch, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<TalabnomRow>(
            "EXEC sp_rt_TalabnomaTarmoq @p0, @p1, @p2",
            yil, soato, branch)
        .ToListAsync(ct);

    public Task<List<Forma13Row>> GetForma13Async(int yil, string? inn, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<Forma13Row>(
            "EXEC sp_rt_FormaN13 @p0, @p1, @p2",
            yil, inn, depBase)
        .ToListAsync(ct);

    // ---- Group A — Putyovki ----

    public Task<List<OperativkaPutyovkiRow>> GetOperativkaPutyovkiAsync(string depBase, DateTime? beginDate, DateTime? endDate, int? sanatId, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<OperativkaPutyovkiRow>(
            "EXEC sp_rt_OperativkaPutyovki @p0, @p1, @p2, @p3",
            depBase, beginDate, endDate, sanatId)
        .ToListAsync(ct);

    public Task<List<OperativkaSubRow>> GetOperativkaOstatokAsync(DateTime? beginDate, DateTime? endDate, int? sanatId, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<OperativkaSubRow>(
            "EXEC sp_rt_OperativkaPutyovkiOstatok @p0, @p1, @p2, @p3",
            beginDate, endDate, sanatId, depBase)
        .ToListAsync(ct);

    public Task<List<OperativkaSubRow>> GetOperativkaPrixodAsync(DateTime? beginDate, DateTime? endDate, int? sanatId, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<OperativkaSubRow>(
            "EXEC sp_rt_OperativkaPutyovkiPrixod @p0, @p1, @p2, @p3",
            beginDate, endDate, sanatId, depBase)
        .ToListAsync(ct);

    public Task<List<OperativkaRasxodRow>> GetOperativkaRasxodAsync(DateTime? beginDate, DateTime? endDate, int? sanatId, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<OperativkaRasxodRow>(
            "EXEC sp_rt_OperativkaPutyovkiRasxod @p0, @p1, @p2, @p3",
            beginDate, endDate, sanatId, depBase)
        .ToListAsync(ct);

    public Task<List<OperativkaVozvratRow>> GetOperativkaVozvratAsync(DateTime? beginDate, DateTime? endDate, int? sanatId, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<OperativkaVozvratRow>(
            "EXEC sp_rt_OperativkaPutyovkiVozvrat @p0, @p1, @p2, @p3",
            beginDate, endDate, sanatId, depBase)
        .ToListAsync(ct);

    public Task<List<SvodPutyovkamRow>> GetSvodPoPutyovkamAsync(string depBase, int? year, int? kvartal, string? otras, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<SvodPutyovkamRow>(
            "EXEC sp_rt_SvodniyPoPutyovkam @p0, @p1, @p2, @p3",
            depBase, year, kvartal, otras)
        .ToListAsync(ct);

    public Task<List<SvodniyOtchetPutyovkiRow>> GetSvodniyOtchetPutyovkiAsync(string depBase, int? kvartal, string? smfil, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<SvodniyOtchetPutyovkiRow>(
            "EXEC sp_rt_Buxoro_Halima_v3 @p0, @p1, @p2",
            depBase, kvartal, smfil)
        .ToListAsync(ct);

    public Task<List<ZaprosPutyovkiRow>> GetZaprosPutyovkiAsync(string depBase, DateTime? beginDate, DateTime? endDate, long? idOrg, int? idSanat, string? lastName, string? numPut, string? pol, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<ZaprosPutyovkiRow>(
            "EXEC sp_rt_Zapros_Putyovki @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
            depBase, beginDate, endDate, idOrg, idSanat, lastName, numPut, pol)
        .ToListAsync(ct);

    public Task<List<KnigaVladelsevRow>> GetKnigaVladelsevAsync(DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<KnigaVladelsevRow>(
            "EXEC sp_rt_UchetVladelsevPutyovok @p0, @p1, @p2",
            beginDate, endDate, depBase)
        .ToListAsync(ct);

    public Task<List<Forma14Row>> GetForma14Async(DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<Forma14Row>(
            "EXEC sp_rt_FormaN14 @p0, @p1, @p2",
            beginDate, endDate, depBase)
        .ToListAsync(ct);

    public Task<List<TekushiyeOstatokRow>> GetTekushiyeOstatokAsync(string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<TekushiyeOstatokRow>(
            """
            SELECT s.Name_San, p.Num_Put, p.Data_San AS DataSan, p.Symma_Put
            FROM tblPos_Put p
            JOIN tblPos_San s ON p.ID_San = s.ID
            WHERE p.STATUS = '1'
              AND p.DEPARTMENT_CODE = @p0
            """,
            depBase)
        .ToListAsync(ct);

    // ---- Group B — Financial ----

    public Task<List<PostupleniyaEspOtrasliRow>> GetPostupleniyaEspOtrasliAsync(DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<PostupleniyaEspOtrasliRow>(
            "EXEC sp_rt_PostupleniyaEspOtrasli @p0, @p1, @p2",
            beginDate, endDate, depBase)
        .ToListAsync(ct);

    public Task<List<PostupleniyaSvodnoyeRow>> GetPostupleniyaSvodnoyeAsync(DateTime? beginDate, DateTime? endDate, string? department, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<PostupleniyaSvodnoyeRow>(
            "EXEC sp_rt_PostupleniyaSvodnoye @p0, @p1, @p2",
            beginDate, endDate, department)
        .ToListAsync(ct);

    public Task<List<OborotkaRegionamRow>> GetOborotkaRegionamAsync(string depBase, DateTime? beginDate, DateTime? endDate, string? soato, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<OborotkaRegionamRow>(
            "EXEC sp_rt_OborotkaPoRegionam @p0, @p1, @p2, @p3",
            depBase, beginDate, endDate, soato)
        .ToListAsync(ct);

    public Task<List<BalansPredpriyatiyRow>> GetBalansPredpriyatiyAsync(DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<BalansPredpriyatiyRow>(
            "EXEC sp_rt_BalanciPredpriyatiy @p0, @p1, @p2",
            beginDate, endDate, depBase)
        .ToListAsync(ct);

    public Task<List<OrganizasiyaRow>> GetDebitorskayaAsync(string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<OrganizasiyaRow>(
            "EXEC sp_rt_DebitorskayaZadoljennost @p0",
            depBase)
        .ToListAsync(ct);

    public Task<List<OrganizasiyaRow>> GetOrganizasiiOtrasliAsync(string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<OrganizasiyaRow>(
            "EXEC sp_rt_OrganizasiiOtrasli @p0",
            depBase)
        .ToListAsync(ct);

    public Task<List<OrganizasiyaRow>> GetOrganizasiiRayonyAsync(string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<OrganizasiyaRow>(
            "EXEC sp_rt_OrganizasiiRayony @p0",
            depBase)
        .ToListAsync(ct);

    public Task<List<OrganizasiyaRow>> GetSpisokOrgAsync(string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<OrganizasiyaRow>(
            "EXEC sp_rt_SpisokOrganizasiy @p0",
            depBase)
        .ToListAsync(ct);

    // ---- Group C — Journal Orders ----

    public async Task<JurOrderResult> GetJurOrder4Async(int year, int kvartal, string depBase, CancellationToken ct = default)
    {
        var summary = await db.Database.SqlQueryRaw<JurOrderRow>(
            "EXEC sp_rt_JurnalniyOrder4 @p0, @p1, @p2",
            year, kvartal, depBase)
        .ToListAsync(ct);
        var details = await db.Database.SqlQueryRaw<JurOrderRow>(
            "EXEC sp_rt_JurnalniyOrder4Provodka @p0, @p1, @p2",
            year, kvartal, depBase)
        .ToListAsync(ct);
        return new JurOrderResult { Summary = summary, Details = details };
    }

    public async Task<JurOrderResult> GetJurOrder5Async(int year, int kvartal, string depBase, CancellationToken ct = default)
    {
        var summary = await db.Database.SqlQueryRaw<JurOrderRow>(
            "EXEC sp_rt_JurnalniyOrder5 @p0, @p1, @p2",
            year, kvartal, depBase)
        .ToListAsync(ct);
        var details = await db.Database.SqlQueryRaw<JurOrderRow>(
            "EXEC sp_rt_JurnalniyOrder5Provodka @p0, @p1, @p2",
            year, kvartal, depBase)
        .ToListAsync(ct);
        return new JurOrderResult { Summary = summary, Details = details };
    }

    public async Task<JurOrderResult> GetJurOrder6Async(int year, int kvartal, string depBase, CancellationToken ct = default)
    {
        var summary = await db.Database.SqlQueryRaw<JurOrderRow>(
            "EXEC sp_rt_JurnalniyOrder6 @p0, @p1, @p2",
            year, kvartal, depBase)
        .ToListAsync(ct);
        var details = await db.Database.SqlQueryRaw<JurOrderRow>(
            "EXEC sp_rt_JurnalniyOrder6Provodka @p0, @p1, @p2",
            year, kvartal, depBase)
        .ToListAsync(ct);
        return new JurOrderResult { Summary = summary, Details = details };
    }

    public Task<List<JurOrderOtchetiRow>> GetJurOrderOtchetiAsync(string depBase, int jorType, DateTime? bg, DateTime? end, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<JurOrderOtchetiRow>(
            "EXEC sp_rt_JurnalniyOrderOtcheti @p0, @p1, @p2, @p3",
            depBase, jorType, bg, end)
        .ToListAsync(ct);

    // ---- Group D — Monitoring ----

    public Task<List<MonitoringOxvatRow>> GetMonitoringOxvatAsync(string depBase, int? year, int? kvartal, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<MonitoringOxvatRow>(
            "EXEC sp_monit_MonitoringOxvataSistemi @p0, @p1, @p2",
            depBase, year, kvartal)
        .ToListAsync(ct);

    public Task<List<MonitoringNarusheniyaRow>> GetMonitoringNarusheniyaAsync(string depBase, int? year, int? kvartal, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<MonitoringNarusheniyaRow>(
            "EXEC sp_monit_NarusheniyaPoProvodkam @p0, @p1, @p2",
            depBase, year, kvartal)
        .ToListAsync(ct);

    public Task<List<MonitoringPererasxodPlanRow>> GetMonitoringPererasxodPlanAsync(string depBase, int? year, int? kvartal, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<MonitoringPererasxodPlanRow>(
            "EXEC sp_monit_PererasxodPlanaPoFinOtchetu @p0, @p1, @p2",
            depBase, year, kvartal)
        .ToListAsync(ct);

    public Task<List<MonitoringPererasxodPutyovkiRow>> GetMonitoringPererasxodPutyovkiAsync(string depBase, int? year, int? kvartal, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<MonitoringPererasxodPutyovkiRow>(
            "EXEC sp_monit_PererasxodPoPutyovkam @p0, @p1, @p2",
            depBase, year, kvartal)
        .ToListAsync(ct);

    public Task<List<MonitoringKaznacheystvRow>> GetMonitoringKaznacheystvAsync(string depBase, int? year, int? kvartal, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<MonitoringKaznacheystvRow>(
            "EXEC sp_monit_PostupleniyeOtKaznaCheystva @p0, @p1, @p2",
            depBase, year, kvartal)
        .ToListAsync(ct);

    // ---- Group E — Organization Cards ----

    public Task<List<KartochkaOrgRow>> GetKartochkaOrgAsync(string? orgInn, string depBase, DateTime? beginDate, DateTime? endDate, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<KartochkaOrgRow>(
            "EXEC sp_rt_KartochkaOrganizasii @p0, @p1, @p2, @p3",
            orgInn, depBase, beginDate, endDate)
        .ToListAsync(ct);

    public Task<List<KartochkaHistoryRow>> GetKartochkaHistoryAsync(long? idOrg, DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<KartochkaHistoryRow>(
            "EXEC sp_rt_KartochkaOrganizasiiHistory @p0, @p1, @p2, @p3",
            idOrg, beginDate, endDate, depBase)
        .ToListAsync(ct);

    public Task<List<KartochkaOplataRow>> GetKartochkaOplataAsync(long? idOrg, DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<KartochkaOplataRow>(
            "EXEC sp_rt_KartochkaOrganizasiiOplata @p0, @p1, @p2, @p3",
            idOrg, beginDate, endDate, depBase)
        .ToListAsync(ct);

    // ---- Group F — Medical / Other ----

    public Task<List<MedRegistraciyaRow>> GetMedRegistraciyaAsync(string depBase, long? idOrg, DateTime? beginDate, DateTime? endDate, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<MedRegistraciyaRow>(
            "EXEC sp_rt_OtchetMedRegRabotnikov @p0, @p1, @p2, @p3",
            depBase, idOrg, beginDate, endDate)
        .ToListAsync(ct);

    public Task<List<UchetStoimostiRow>> GetUchetStoimostiAsync(DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<UchetStoimostiRow>(
            "EXEC sp_rt_OtchetUchetStoimostiVidachi @p0, @p1, @p2",
            beginDate, endDate, depBase)
        .ToListAsync(ct);

    public Task<List<PoiskPutyovkiRow>> GetPoiskPutyovkiAsync(string? numPut, string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<PoiskPutyovkiRow>(
            """
            SELECT o.ProfsoyzNaimenovanie, s.Name_San, p.Num_Put, p.Symma_Put, 'Приход' AS Operasiya
            FROM tblPos_Put p
            JOIN tblPos_San s ON p.ID_San = s.ID
            JOIN tblSprav_Org o ON p.ID_Org = o.ID
            WHERE p.Num_Put = @p0
              AND p.DEPARTMENT_CODE = @p1
            """,
            numPut, depBase)
        .ToListAsync(ct);

    public Task<List<OstatokNizhestvuyushixRow>> GetOstatokNizhestvuyushixAsync(string depBase, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<OstatokNizhestvuyushixRow>(
            """
            SELECT o.INN, o.Name_Org, s.Name_San, p.Num_Put
            FROM tblPos_Put p
            JOIN tblPos_San s ON p.ID_San = s.ID
            JOIN tblSprav_Org o ON p.ID_Org = o.ID
            WHERE p.STATUS = '1'
              AND p.DEPARTMENT_CODE LIKE @p0 + '%'
            """,
            depBase)
        .ToListAsync(ct);

    public Task<List<TalabnomRow>> GetTalabnomlarViloyatAsync(int yil, string? soato, CancellationToken ct = default) =>
        db.Database.SqlQueryRaw<TalabnomRow>(
            "EXEC sp_rt_TalabnomaViloyat @p0, @p1",
            yil, soato)
        .ToListAsync(ct);
}
