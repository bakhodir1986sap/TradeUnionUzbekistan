using KasabaKIAT.Core.DTOs.Reports;

namespace KasabaKIAT.Core.Services;

public interface IFinOtchetService
{
    Task<TotalOutput> GetFinOtchetAsync(int year, int kvartal, string departmentCode, bool isSvodniy, CancellationToken ct = default);
}
