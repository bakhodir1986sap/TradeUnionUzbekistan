namespace KasabaKIAT.Core.DTOs.Reports;

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
    public DateTime? DataPokupkiPutyovki { get; set; }
    public string? NomerResheniyaOVidache { get; set; }
    public string? FIO { get; set; }
    public string? MestoRaboti { get; set; }
    public DateTime? DataPlateja { get; set; }
    public decimal? SymmaPlateja { get; set; }
    public string? NomerPlatejnogoPorucheniya { get; set; }
    public string? NomerTalona { get; set; }
    public DateTime? DataTalona { get; set; }
}

public class Forma14Row
{
    public string? Col_0 { get; set; }
    public string? Col_1 { get; set; }
    public int? OstatokKol_2 { get; set; }
    public decimal? OstatokSymma_3 { get; set; }
    public int? PrixodKol_4 { get; set; }
    public decimal? PrixodSumma_5 { get; set; }
    public string? Col_6 { get; set; }
    public string? Col_7 { get; set; }
    public int? RasxodKol_8 { get; set; }
    public decimal? RasxodSumma_9 { get; set; }
    public int? RasxodKol_10 { get; set; }
    public decimal? RasxodSumma_11 { get; set; }
    public string? Col_12 { get; set; }
    public string? Col_13 { get; set; }
    public string? Col_14 { get; set; }
    public string? Col_15 { get; set; }
    public string? Col_16 { get; set; }
    public string? Col_17 { get; set; }
    public string? Col_18 { get; set; }
    public string? Col_19 { get; set; }
    public int? RasxodKol_20 { get; set; }
    public decimal? RasxodSumma_21 { get; set; }
    public string? Col_22 { get; set; }
    public string? Col_23 { get; set; }
    public string? Col_24 { get; set; }
}

public class OperativkaPutyovkiRow
{
    public int? SanatID { get; set; }
    public string? SanatoryName { get; set; }
    public int? OstatokKol { get; set; }
    public decimal? OstatokSymma { get; set; }
    public int? PrixodKol { get; set; }
    public decimal? PrixodSumma { get; set; }
    public int? RasxodKol { get; set; }
    public decimal? RasxodSumma { get; set; }
    public int? VozvratKol { get; set; }
    public decimal? VozvRatSumma { get; set; }
    public int? OstatokKolKones { get; set; }
    public decimal? OstatokSymmaKones { get; set; }
}

public class OperativkaSubRow
{
    public DateTime? Data_Post { get; set; }
    public string? Num_Doc { get; set; }
    public string? Num_Put { get; set; }
    public string? Name_Org { get; set; }
    public decimal? Symma_Put { get; set; }
}

public class OperativkaRasxodRow
{
    public DateTime? Data_Post { get; set; }
    public string? Num_Doc { get; set; }
    public string? Num_Put { get; set; }
    public string? Name_Org { get; set; }
    public decimal? Symma { get; set; }
}

public class OperativkaVozvratRow
{
    public DateTime? Data_Post { get; set; }
    public string? Num_Doc { get; set; }
    public string? Num_Put { get; set; }
    public string? Name_Org { get; set; }
    public decimal? SymmaP { get; set; }
}

public class SvodPutyovkamRow
{
    public string? Name_San { get; set; }
    public string? Name_Ot { get; set; }
    public string? Name_Org { get; set; }
    public decimal? Symma { get; set; }
    public string? Priz { get; set; }
}

public class SvodniyOtchetPutyovkiRow
{
    public string? Name_Ot { get; set; }
    public string? Name_Org { get; set; }
    public string? Sanat { get; set; }
    public int? RasxodPutyovka { get; set; }
    public string? Num_Doc { get; set; }
    public decimal? RasxodSumma { get; set; }
    public int? VozvratPutyovka { get; set; }
    public decimal? VozvratSumma { get; set; }
    public int? FamiliarPutyovka { get; set; }
    public decimal? FamiliarSumma { get; set; }
    public int? Ostatok_Putyoka { get; set; }
    public decimal? Ostatok_Summa { get; set; }
    public string? FamilyName { get; set; }
}

public class ZaprosPutyovkiRow
{
    public string? MATCH_DEPARTMENTBASE { get; set; }
    public string? ProfsoyzNaimenovanie { get; set; }
    public long? ID { get; set; }
    public string? Name_Org { get; set; }
    public string? Name_San { get; set; }
    public string? StatusPutyovki { get; set; }
    public string? Otpravitel_Postavshik { get; set; }
    public DateTime? DataPrixoda { get; set; }
    public string? NomerSchetFakturi { get; set; }
    public string? PrixodDoverennost { get; set; }
    public string? PeriodPrixoda { get; set; }
    public string? NazvaniyaSanatoriya { get; set; }
    public string? N_Putyovki { get; set; }
    public string? SrokPutyovki { get; set; }
    public decimal? Stoimost { get; set; }
    public string? StatusPrioda { get; set; }
    public DateTime? DataVidachi { get; set; }
    public string? NomerNakladnoy { get; set; }
    public string? PoluchatelPokupatel { get; set; }
    public string? RasxodDoverennost { get; set; }
    public string? PeriodVidachi { get; set; }
    public string? StatusVidachi { get; set; }
    public string? FamiliyaVladelsa { get; set; }
    public string? ImyaVladelsa { get; set; }
    public string? OtchstvoVladelsa { get; set; }
}

public class KnigaVladelsevRow
{
    public long? ID { get; set; }
    public string? Name_San { get; set; }
    public string? Num_Put { get; set; }
    public decimal? Symma { get; set; }
    public string? FamiliyaVladelsa { get; set; }
    public string? ImyaVladelsa { get; set; }
    public string? OtchstvoVladelsa { get; set; }
    public string? MestoJitelstva { get; set; }
    public string? MestoRaboti { get; set; }
    public string? Doljnost { get; set; }
    public string? SeriyaPassporta { get; set; }
    public string? NomerPassporta { get; set; }
    public string? KemVidan { get; set; }
    public DateTime? DataVidachi { get; set; }
    public string? Pol { get; set; }
    public string? MedisinaNomer { get; set; }
    public decimal? Procent { get; set; }
    public DateTime? MedisinaDataVidachi { get; set; }
    public string? UplataNomer { get; set; }
    public DateTime? UplataDataDoc { get; set; }
    public decimal? UplataSumma { get; set; }
    public string? ObratniyTalon { get; set; }
    public DateTime? ObratniyTalonData { get; set; }
}

public class TekushiyeOstatokRow
{
    public string? Name_San { get; set; }
    public string? Num_Put { get; set; }
    public DateTime? DataSan { get; set; }
    public decimal? Symma_Put { get; set; }
}

public class OstatokNizhestvuyushixRow
{
    public string? INN { get; set; }
    public string? Name_Org { get; set; }
    public string? Name_San { get; set; }
    public string? Num_Put { get; set; }
}

public class PoiskPutyovkiRow
{
    public string? ProfsoyzNaimenovanie { get; set; }
    public string? Name_San { get; set; }
    public string? Num_Put { get; set; }
    public decimal? Symma_Put { get; set; }
    public string? Operasiya { get; set; }
}
