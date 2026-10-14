namespace KasabaKIAT.Core.DTOs.Reports;

public class MonitoringOxvatRow
{
    public int? KolVoOrgEstProfKom { get; set; }
    public int? KolVoOrgIspSystemu { get; set; }
}

public class MonitoringNarusheniyaRow
{
    public DateTime? DataProvodki { get; set; }
    public string? NazvaniyeOrganizasii { get; set; }
    public string? ImyaPolzovatelya { get; set; }
    public string? DanniyeProvodki { get; set; }
}

public class MonitoringPererasxodPlanRow
{
    public string? OrganizationName { get; set; }
    public string? PeriodPostroeniyaOtcheta { get; set; }
    public string? NazvaniyaParamentra { get; set; }
    public decimal? PlanovayaSumma { get; set; }
    public decimal? FakticheskayaSumma { get; set; }
    public decimal? Raznisa { get; set; }
}

public class MonitoringPererasxodPutyovkiRow
{
    public string? OrganizationName { get; set; }
    public string? PeriodPostroeniyaOtcheta { get; set; }
    public decimal? PostupleniyaZaPeriod { get; set; }
    public int? KolVoMogutPoluchit { get; set; }
    public int? KolVoFakticheskiPoluch { get; set; }
}

public class MonitoringKaznacheystvRow
{
    public DateTime? Data { get; set; }
    public int? KolVoOrganizasiy { get; set; }
    public decimal? ObshayaSumma { get; set; }
}
