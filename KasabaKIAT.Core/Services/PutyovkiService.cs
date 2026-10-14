using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs;
using KasabaKIAT.Core.DTOs.Putyovki;
using KasabaKIAT.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Services;

public class PutyovkiService(KasabaKIATContext db, IAuditService audit, ILogger<PutyovkiService> logger) : IPutyovkiService
{
    public async Task<PagedResult<PutyovkaDto>> GetListAsync(
        int page, int pageSize, int? sanId, int? year, string? status, CancellationToken ct = default)
    {
        var query = db.TbptPutyovkis
            .Include(p => p.tblSprav_San)
            .AsQueryable();

        if (sanId.HasValue)
            query = query.Where(p => p.SANATORIUM_ID == sanId.Value);
        if (year.HasValue)
            query = query.Where(p => p.SMFIL == year.Value);
        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.STATUS == status);
        else
            query = query.Where(p => p.STATUS == "1");

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(p => p.MODIFY_DATE)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => ToDto(p))
            .ToListAsync(ct);

        return new PagedResult<PutyovkaDto>(items, total, page, pageSize);
    }

    public async Task<PutyovkaDto?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var p = await db.TbptPutyovkis
            .Include(x => x.tblSprav_San)
            .FirstOrDefaultAsync(x => x.REC_ID == id, ct);
        return p == null ? null : ToDto(p);
    }

    public async Task<PutyovkaDto> CreateAsync(CreatePutyovkaRequest request, long userId, string departmentCode, CancellationToken ct = default)
    {
        var quantityIn = request.QuantityIn ?? 0;
        var price = request.Price ?? 0;
        var entity = new TbptPutyovki
        {
            DEPARTMENT_CODE = departmentCode,
            SANATORIUM_ID = request.SanatoriumId,
            CAMP_ID = request.CampId,
            USER_ID = userId,
            SERIAL_BEGIN = request.SerialBegin?.ToString(),
            SERIAL_END = request.SerialEnd?.ToString(),
            REST_BEGIN_DATE = request.RestBeginDate,
            REST_END_DATE = request.RestEndDate,
            PRICE = price,
            STATUS = "1",
            MODIFY_DATE = DateTime.Now,
            QUANTITY_IN = quantityIn,
            QUANTITY_OUT = 0,
            QUANTITY_REMAIN = quantityIn,
            REMAIN_BAL = quantityIn * price,
            SMFIL = request.Year
        };
        db.TbptPutyovkis.Add(entity);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Putyovka created: RecId={RecId} SanId={SanId} UserId={UserId}", entity.REC_ID, request.SanatoriumId, userId);
        await audit.LogAsync(userId, "PUTYOVKA_CREATE", $"RecId={entity.REC_ID} SanId={request.SanatoriumId}", ct);
        return ToDto(entity);
    }

    public async Task<PutyovkaDto?> UpdateAsync(long id, CreatePutyovkaRequest request, long actorUserId, CancellationToken ct = default)
    {
        var entity = await db.TbptPutyovkis.FindAsync([id], ct);
        if (entity == null) return null;

        if (request.SanatoriumId.HasValue) entity.SANATORIUM_ID = request.SanatoriumId;
        if (request.CampId.HasValue) entity.CAMP_ID = request.CampId;
        if (request.SerialBegin.HasValue) entity.SERIAL_BEGIN = request.SerialBegin.ToString();
        if (request.SerialEnd.HasValue) entity.SERIAL_END = request.SerialEnd.ToString();
        if (request.RestBeginDate.HasValue) entity.REST_BEGIN_DATE = request.RestBeginDate;
        if (request.RestEndDate.HasValue) entity.REST_END_DATE = request.RestEndDate;
        if (request.Price.HasValue) entity.PRICE = request.Price;
        if (request.QuantityIn.HasValue) entity.QUANTITY_IN = request.QuantityIn;
        if (request.Year.HasValue) entity.SMFIL = request.Year;

        var qIn = entity.QUANTITY_IN ?? 0;
        var qOut = entity.QUANTITY_OUT ?? 0;
        entity.QUANTITY_REMAIN = qIn - qOut;
        entity.REMAIN_BAL = entity.QUANTITY_REMAIN * (entity.PRICE ?? 0);
        entity.MODIFY_DATE = DateTime.Now;

        await db.SaveChangesAsync(ct);
        logger.LogInformation("Putyovka updated: RecId={RecId}", id);
        await audit.LogAsync(actorUserId, "PUTYOVKA_UPDATE", $"RecId={id}", ct);
        return ToDto(entity);
    }

    public async Task<bool> DeleteAsync(long id, long actorUserId, CancellationToken ct = default)
    {
        var entity = await db.TbptPutyovkis.FindAsync([id], ct);
        if (entity == null) return false;
        entity.STATUS = "0";
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Putyovka deleted (soft): RecId={RecId}", id);
        await audit.LogAsync(actorUserId, "PUTYOVKA_DELETE", $"RecId={id}", ct);
        return true;
    }

    private static PutyovkaDto ToDto(TbptPutyovki p) => new()
    {
        RecId = p.REC_ID,
        DepartmentCode = p.DEPARTMENT_CODE,
        SanatoriumId = (int?)p.SANATORIUM_ID,
        SanatoriumName = p.tblSprav_San?.Name_San,
        CampId = p.CAMP_ID,
        UserId = p.USER_ID,
        SerialBegin = p.SERIAL_BEGIN != null && int.TryParse(p.SERIAL_BEGIN, out var sb) ? sb : null,
        SerialEnd = p.SERIAL_END != null && int.TryParse(p.SERIAL_END, out var se) ? se : null,
        RestBeginDate = p.REST_BEGIN_DATE,
        RestEndDate = p.REST_END_DATE,
        Price = p.PRICE,
        Status = p.STATUS,
        ModifyDate = p.MODIFY_DATE,
        QuantityIn = p.QUANTITY_IN,
        QuantityOut = p.QUANTITY_OUT,
        QuantityRemain = p.QUANTITY_REMAIN,
        RemainBal = p.REMAIN_BAL,
        Year = p.SMFIL
    };
}
