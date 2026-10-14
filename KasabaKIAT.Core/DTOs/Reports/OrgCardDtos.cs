namespace KasabaKIAT.Core.DTOs.Reports;

public class KartochkaOrgRow
{
    public string? Name_Org { get; set; }
    public long? ID { get; set; }
    public string? NameReg { get; set; }
    public string? MFO { get; set; }
    public string? Adress { get; set; }
    public string? Telefon { get; set; }
    public string? RS { get; set; }
    public decimal? DT_OST { get; set; }
    public decimal? KT_OST { get; set; }
}

public class KartochkaHistoryRow
{
    public string? TipDokumenta { get; set; }
    public DateTime? DataDocumenta { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Doverennost { get; set; }
    public decimal? Summa { get; set; }
    public string? UniQueId { get; set; }
}

public class KartochkaOplataRow
{
    public long? ID_Otras { get; set; }
    public int? KolVo { get; set; }
    public decimal? NaSummu { get; set; }
    public long? ID_Org { get; set; }
    public long? ID_Ot { get; set; }
    public string? Name_Proc { get; set; }
}

public class MedRegistraciyaRow
{
    public string? FirstName { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Adress { get; set; }
    public string? Pas_ser { get; set; }
    public string? Pas_Num { get; set; }
    public DateTime? Pas_Data { get; set; }
    public string? Pas_KD { get; set; }
    public string? Med_Doc { get; set; }
    public DateTime? Med_Data { get; set; }
    public string? Name_San { get; set; }
    public long? ID { get; set; }
    public string? Name_Org { get; set; }
}

public class UchetStoimostiRow
{
    public string? Num_Doc { get; set; }
    public DateTime? Data_Post { get; set; }
    public int? VidanoPutyovokKolVo { get; set; }
    public decimal? VidanoPutyovokSumma { get; set; }
    public int? PrBPKolVo { get; set; }
    public decimal? PrBPSumma { get; set; }
    public int? Pr45KolVo { get; set; }
    public int? Pr100KolVo { get; set; }
    public decimal? Pr45Summa { get; set; }
    public decimal? Pr100Summa { get; set; }
    public decimal? DoljnoPostupit { get; set; }
}
