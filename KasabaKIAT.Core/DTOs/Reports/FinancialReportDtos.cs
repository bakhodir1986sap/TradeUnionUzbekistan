namespace KasabaKIAT.Core.DTOs.Reports;

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

public class PostupleniyaEspOtrasliRow
{
    public string? TUMAN_SHAXAR { get; set; }
    public string? CLINN { get; set; }
    public decimal? POSTUPLENIYA { get; set; }
    public decimal? Obshiy01 { get; set; }
    public decimal? FEDERATION_AMT { get; set; }
    public decimal? OBLSOVPROF_AMT { get; set; }
    public string? BRANCH_NAME { get; set; }
    public string? NAME_ORG { get; set; }
}

public class PostupleniyaSvodnoyeRow
{
    public long? PAYMENT_ID { get; set; }
    public decimal? PROFSOUZ_AMT { get; set; }
    public decimal? FEDERATION_AMT { get; set; }
    public decimal? OBLSOVPROF_AMT { get; set; }
    public string? OBL_DEPARTMENT_BASE { get; set; }
    public long? PROVODKA_ID { get; set; }
    public string? REMARK { get; set; }
    public string? INN { get; set; }
    public string? SOATO { get; set; }
    public string? SOATO_NAME { get; set; }
    public DateTime? OPERDATE { get; set; }
    public string? OKONX_ID { get; set; }
    public string? OKONX_NAME { get; set; }
    public string? BRANCH_CODE { get; set; }
    public string? BRANCH_NAME { get; set; }
}

public class OborotkaRegionamRow
{
    public string? K_TYPE { get; set; }
    public string? OPERDAY { get; set; }
    public long? ID { get; set; }
    public string? BANKID { get; set; }
    public string? KLSDOX { get; set; }
    public string? DOCNUMB { get; set; }
    public DateTime? DOCDATE { get; set; }
    public string? DOCTYPE { get; set; }
    public decimal? SUMPAY { get; set; }
    public string? CLMFO { get; set; }
    public string? CLACC { get; set; }
    public string? CLINN { get; set; }
    public string? CLNAME { get; set; }
    public string? COMFO { get; set; }
    public string? COACC { get; set; }
    public string? COINN { get; set; }
    public string? CONAME { get; set; }
    public string? PURPOSE { get; set; }
    public string? CHAPTER { get; set; }
    public string? OKONH { get; set; }
    public string? FILE_ID { get; set; }
    public string? OTCHISLEN { get; set; }
    public string? GRPID { get; set; }
    public string? NameReg { get; set; }
    public string? CodeReg { get; set; }
}

public class BalansPredpriyatiyRow
{
    public string? Name_Org { get; set; }
    public decimal? DebitNachalo { get; set; }
    public decimal? KreditNachalo { get; set; }
    public decimal? DebitOborot { get; set; }
    public decimal? KreditOborot { get; set; }
    public decimal? DebitKones { get; set; }
    public decimal? KreditKones { get; set; }
}

public class OrganizasiyaRow
{
    public long? ID_Ot { get; set; }
    public string? Name_Org { get; set; }
    public string? INN { get; set; }
    public string? RS { get; set; }
    public string? MFO { get; set; }
    public string? Bank { get; set; }
    public string? OKONX { get; set; }
    public string? Adress { get; set; }
    public string? Telefon { get; set; }
    public string? Region { get; set; }
    public decimal? DT_OST { get; set; }
    public decimal? KT_OST { get; set; }
    public string? E_Mail { get; set; }
    public string? DB_BAL { get; set; }
    public string? CT_BAL { get; set; }
    public string? Branch { get; set; }
    public string? NameReg { get; set; }
}

public class JurOrderRow
{
    public DateTime? Data_Post { get; set; }
    public string? Num_Doc { get; set; }
    public long? ID { get; set; }
    public string? INN { get; set; }
    public string? Name_Org { get; set; }
    public string? DebitSchet { get; set; }
    public string? CreditSchet { get; set; }
    public int? Kolvo { get; set; }
    public decimal? Summa { get; set; }
}

public class JurOrderResult
{
    public List<JurOrderRow> Summary { get; set; } = [];
    public List<JurOrderRow> Details { get; set; } = [];
}

public class JurOrderOtchetiRow
{
    public string? DEBIT_ACCOUNT { get; set; }
    public string? CREDIT_ACCOUNT { get; set; }
    public decimal? AMOUNT { get; set; }
}
