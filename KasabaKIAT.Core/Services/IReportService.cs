using KasabaKIAT.Core.DTOs.Reports;

namespace KasabaKIAT.Core.Services;

public interface IReportService
{
    // Existing 8 endpoints
    Task<List<SvodniyOtchotRow>> GetSvodniyOtchotAsync(DateTime? beginDt, DateTime? endDt, string? soato, string? branch, string depBase, CancellationToken ct = default);
    Task<List<PostupleniyaRow>> GetPostupleniyaAsync(string? inn, string depBase, CancellationToken ct = default);
    Task<List<PostupleniyaNRow>> GetPostupleniyaNAsync(string? inn, string depBase, DateTime? zb, DateTime? ze, CancellationToken ct = default);
    Task<List<ProvodkiDetailRow>> GetProvodkiDetailsAsync(string depBase, DateTime? bg, DateTime? end, CancellationToken ct = default);
    Task<List<ProvodkiSummaryRow>> GetProvodkiSummaryAsync(string depBase, DateTime? bg, DateTime? end, CancellationToken ct = default);
    Task<List<TalabnomRow>> GetTalabnomlarKorxonaAsync(int yil, string? inn, CancellationToken ct = default);
    Task<List<TalabnomRow>> GetTalabnomlarTarmoqAsync(int yil, string? soato, string? branch, CancellationToken ct = default);
    Task<List<Forma13Row>> GetForma13Async(int yil, string? inn, string depBase, CancellationToken ct = default);

    // Group A — Putyovki
    Task<List<OperativkaPutyovkiRow>> GetOperativkaPutyovkiAsync(string depBase, DateTime? beginDate, DateTime? endDate, int? sanatId, CancellationToken ct = default);
    Task<List<OperativkaSubRow>> GetOperativkaOstatokAsync(DateTime? beginDate, DateTime? endDate, int? sanatId, string depBase, CancellationToken ct = default);
    Task<List<OperativkaSubRow>> GetOperativkaPrixodAsync(DateTime? beginDate, DateTime? endDate, int? sanatId, string depBase, CancellationToken ct = default);
    Task<List<OperativkaRasxodRow>> GetOperativkaRasxodAsync(DateTime? beginDate, DateTime? endDate, int? sanatId, string depBase, CancellationToken ct = default);
    Task<List<OperativkaVozvratRow>> GetOperativkaVozvratAsync(DateTime? beginDate, DateTime? endDate, int? sanatId, string depBase, CancellationToken ct = default);
    Task<List<SvodPutyovkamRow>> GetSvodPoPutyovkamAsync(string depBase, int? year, int? kvartal, string? otras, CancellationToken ct = default);
    Task<List<SvodniyOtchetPutyovkiRow>> GetSvodniyOtchetPutyovkiAsync(string depBase, int? kvartal, string? smfil, CancellationToken ct = default);
    Task<List<ZaprosPutyovkiRow>> GetZaprosPutyovkiAsync(string depBase, DateTime? beginDate, DateTime? endDate, long? idOrg, int? idSanat, string? lastName, string? numPut, string? pol, CancellationToken ct = default);
    Task<List<KnigaVladelsevRow>> GetKnigaVladelsevAsync(DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default);
    Task<List<Forma14Row>> GetForma14Async(DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default);
    Task<List<TekushiyeOstatokRow>> GetTekushiyeOstatokAsync(string depBase, CancellationToken ct = default);

    // Group B — Financial
    Task<List<PostupleniyaEspOtrasliRow>> GetPostupleniyaEspOtrasliAsync(DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default);
    Task<List<PostupleniyaSvodnoyeRow>> GetPostupleniyaSvodnoyeAsync(DateTime? beginDate, DateTime? endDate, string? department, CancellationToken ct = default);
    Task<List<OborotkaRegionamRow>> GetOborotkaRegionamAsync(string depBase, DateTime? beginDate, DateTime? endDate, string? soato, CancellationToken ct = default);
    Task<List<BalansPredpriyatiyRow>> GetBalansPredpriyatiyAsync(DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default);
    Task<List<OrganizasiyaRow>> GetDebitorskayaAsync(string depBase, CancellationToken ct = default);
    Task<List<OrganizasiyaRow>> GetOrganizasiiOtrasliAsync(string depBase, CancellationToken ct = default);
    Task<List<OrganizasiyaRow>> GetOrganizasiiRayonyAsync(string depBase, CancellationToken ct = default);
    Task<List<OrganizasiyaRow>> GetSpisokOrgAsync(string depBase, CancellationToken ct = default);

    // Group C — Journal Orders
    Task<JurOrderResult> GetJurOrder4Async(int year, int kvartal, string depBase, CancellationToken ct = default);
    Task<JurOrderResult> GetJurOrder5Async(int year, int kvartal, string depBase, CancellationToken ct = default);
    Task<JurOrderResult> GetJurOrder6Async(int year, int kvartal, string depBase, CancellationToken ct = default);
    Task<List<JurOrderOtchetiRow>> GetJurOrderOtchetiAsync(string depBase, int jorType, DateTime? bg, DateTime? end, CancellationToken ct = default);

    // Group D — Monitoring
    Task<List<MonitoringOxvatRow>> GetMonitoringOxvatAsync(string depBase, int? year, int? kvartal, CancellationToken ct = default);
    Task<List<MonitoringNarusheniyaRow>> GetMonitoringNarusheniyaAsync(string depBase, int? year, int? kvartal, CancellationToken ct = default);
    Task<List<MonitoringPererasxodPlanRow>> GetMonitoringPererasxodPlanAsync(string depBase, int? year, int? kvartal, CancellationToken ct = default);
    Task<List<MonitoringPererasxodPutyovkiRow>> GetMonitoringPererasxodPutyovkiAsync(string depBase, int? year, int? kvartal, CancellationToken ct = default);
    Task<List<MonitoringKaznacheystvRow>> GetMonitoringKaznacheystvAsync(string depBase, int? year, int? kvartal, CancellationToken ct = default);

    // Group E — Organization Cards
    Task<List<KartochkaOrgRow>> GetKartochkaOrgAsync(string? orgInn, string depBase, DateTime? beginDate, DateTime? endDate, CancellationToken ct = default);
    Task<List<KartochkaHistoryRow>> GetKartochkaHistoryAsync(long? idOrg, DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default);
    Task<List<KartochkaOplataRow>> GetKartochkaOplataAsync(long? idOrg, DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default);

    // Group F — Medical / Other
    Task<List<MedRegistraciyaRow>> GetMedRegistraciyaAsync(string depBase, long? idOrg, DateTime? beginDate, DateTime? endDate, CancellationToken ct = default);
    Task<List<UchetStoimostiRow>> GetUchetStoimostiAsync(DateTime? beginDate, DateTime? endDate, string depBase, CancellationToken ct = default);
    Task<List<PoiskPutyovkiRow>> GetPoiskPutyovkiAsync(string? numPut, string depBase, CancellationToken ct = default);
    Task<List<OstatokNizhestvuyushixRow>> GetOstatokNizhestvuyushixAsync(string depBase, CancellationToken ct = default);
    Task<List<TalabnomRow>> GetTalabnomlarViloyatAsync(int yil, string? soato, CancellationToken ct = default);
}
