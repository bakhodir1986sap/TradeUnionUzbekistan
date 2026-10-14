using KasabaKIAT.Core.DTOs.Putyovki;

namespace KasabaKIAT.Core.Services;

public interface IPutyovkiService
{
    Task<(List<PutyovkaDto> Items, int Total)> GetListAsync(int page, int pageSize, long? bkutId, int? sanId, int? year, string? status);
    Task<PutyovkaDto?> GetByIdAsync(long id);
    Task<PutyovkaDto> CreateAsync(CreatePutyovkaRequest request, long userId, string departmentCode);
    Task<PutyovkaDto?> UpdateAsync(long id, CreatePutyovkaRequest request, long actorUserId);
    Task<bool> DeleteAsync(long id, long actorUserId);
}
