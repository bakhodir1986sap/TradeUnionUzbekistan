namespace KasabaKIAT.Core.DTOs.Reports;

public class GkDebitRow
{
    public string? DebitAccount { get; set; }
    public string? JO { get; set; }
    public decimal? SUMMA { get; set; }
}

public class GkCreditRow
{
    public string? CreditAccount { get; set; }
    public string? JO { get; set; }
    public decimal? SUMMA { get; set; }
}

public class GlavnayaKnigaResult
{
    public List<string> Rows { get; set; } = [];
    public List<string> Cols { get; set; } = [];
    public List<GlavnayaKnigaTable> Data { get; set; } = [];
}

public class GlavnayaKnigaTable
{
    public string? field0010 { get; set; }
    public decimal? field0011 { get; set; }
    public decimal? field0012 { get; set; }
    public decimal? field0013 { get; set; }
    public decimal? field0014 { get; set; }
    public decimal? field0015 { get; set; }
    public decimal? field0016 { get; set; }
    public decimal? field0017 { get; set; }
    public decimal? field0018 { get; set; }
    public decimal? field0019 { get; set; }
    public decimal? field0020 { get; set; }
    public decimal? field0021 { get; set; }
    public decimal? field0022 { get; set; }
    public decimal? field0023 { get; set; }
    public string? field0024 { get; set; }
    public decimal? field0025 { get; set; }
    public decimal? field0026 { get; set; }
    public decimal? field0027 { get; set; }
    public decimal? field0028 { get; set; }
    public decimal? field0029 { get; set; }
    public decimal? field0030 { get; set; }
    public decimal? field0031 { get; set; }
    public decimal? field0032 { get; set; }
    public decimal? field0033 { get; set; }
    public decimal? field0034 { get; set; }
    public decimal? field0035 { get; set; }
    public decimal? field0036 { get; set; }
    public decimal? field0037 { get; set; }
    public decimal? field0038 { get; set; }
    public decimal? field0039 { get; set; }
    public string? field0040 { get; set; }
}
