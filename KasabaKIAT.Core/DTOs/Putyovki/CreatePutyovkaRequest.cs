namespace KasabaKIAT.Core.DTOs.Putyovki;

public class CreatePutyovkaRequest
{
    public string? DepartmentCode { get; set; }
    public int? SanatoriumId { get; set; }
    public long? CampId { get; set; }
    public int? SerialBegin { get; set; }
    public int? SerialEnd { get; set; }
    public DateTime? RestBeginDate { get; set; }
    public DateTime? RestEndDate { get; set; }
    public decimal? Price { get; set; }
    public int? QuantityIn { get; set; }
    public int? Year { get; set; }
}
