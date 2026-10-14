using System.ComponentModel.DataAnnotations;

namespace KasabaKIAT.Core.DTOs.Putyovki;

public class CreatePutyovkaRequest
{
    [MaxLength(50)]
    public string? DepartmentCode { get; set; }
    public int? SanatoriumId { get; set; }
    public long? CampId { get; set; }
    public int? SerialBegin { get; set; }
    public int? SerialEnd { get; set; }
    public DateTime? RestBeginDate { get; set; }
    public DateTime? RestEndDate { get; set; }
    [Range(0, 10_000_000)]
    public decimal? Price { get; set; }
    [Range(0, 10_000)]
    public int? QuantityIn { get; set; }
    [Range(2000, 2100)]
    public int? Year { get; set; }
}
