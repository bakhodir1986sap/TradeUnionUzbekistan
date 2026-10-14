namespace KasabaKIAT.Core.DTOs.Putyovki;

public class PutyovkaDto
{
    public long RecId { get; set; }
    public string? DepartmentCode { get; set; }
    public int? SanatoriumId { get; set; }
    public string? SanatoriumName { get; set; }
    public long? CampId { get; set; }
    public long? UserId { get; set; }
    public int? SerialBegin { get; set; }
    public int? SerialEnd { get; set; }
    public DateTime? RestBeginDate { get; set; }
    public DateTime? RestEndDate { get; set; }
    public decimal? Price { get; set; }
    public string? Status { get; set; }
    public DateTime? ModifyDate { get; set; }
    public int? QuantityIn { get; set; }
    public int? QuantityOut { get; set; }
    public int? QuantityRemain { get; set; }
    public decimal? RemainBal { get; set; }
    public int? Year { get; set; }
}
