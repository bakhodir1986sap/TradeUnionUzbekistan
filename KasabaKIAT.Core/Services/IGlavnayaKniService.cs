using KasabaKIAT.Core.DTOs.Reports;

namespace KasabaKIAT.Core.Services;

public interface IGlavnayaKniService
{
    Task<GlavnayaKnigaResult> GetGlavnayaKnigaAsync(int year, int kvartal, string departmentCode, bool isSvodniy, CancellationToken ct = default);
}
