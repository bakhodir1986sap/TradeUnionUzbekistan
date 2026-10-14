using KasabaKIAT.Core.DTOs;
using KasabaKIAT.Core.DTOs.Putyovki;

namespace KasabaKIAT.Core.Services;

public interface IPutyovkiService
{
    Task<PagedResult<PutyovkaDto>> GetListAsync(int page, int pageSize, int? sanId, int? year, string? status, CancellationToken ct = default);
    Task<PutyovkaDto?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<PutyovkaDto> CreateAsync(CreatePutyovkaRequest request, long userId, string departmentCode, CancellationToken ct = default);
    Task<PutyovkaDto?> UpdateAsync(long id, CreatePutyovkaRequest request, long actorUserId, CancellationToken ct = default);
    Task<bool> DeleteAsync(long id, long actorUserId, CancellationToken ct = default);
}
