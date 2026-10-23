using KasabaKIAT.Core.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace KasabaKIAT.Core.Services;

public class DbContextRagService(KasabaKIATContext db) : IDbContextRagService
{
    public async Task<string> BuildContextBlockAsync(string userQuestion, long userId, CancellationToken ct)
    {
        var sb = new StringBuilder();
        var q = userQuestion.ToLowerInvariant();

        var totalPutyovki = await db.TbptPutyovkis
            .Where(p => p.STATUS != "0")
            .CountAsync(ct);
        sb.AppendLine($"Всего активных путевок в системе: {totalPutyovki}");

        if (q.Contains("путевк") || q.Contains("putyovk") || q.Contains("voucher"))
        {
            var recent = await db.TbptPutyovkis
                .Where(p => p.STATUS != "0")
                .OrderByDescending(p => p.MODIFY_DATE)
                .Take(20)
                .Select(p => new
                {
                    p.SERIAL_BEGIN,
                    p.SERIAL_END,
                    p.REST_BEGIN_DATE,
                    p.REST_END_DATE,
                    p.PRICE,
                    p.QUANTITY_IN,
                    p.QUANTITY_REMAIN,
                    San = p.tblSprav_San != null ? p.tblSprav_San.Name_San : null
                })
                .ToListAsync(ct);

            sb.AppendLine("\nПоследние путевки:");
            foreach (var p in recent)
                sb.AppendLine($"  Серия {p.SERIAL_BEGIN}-{p.SERIAL_END}, санаторий: {p.San}, " +
                              $"дата: {p.REST_BEGIN_DATE:dd.MM.yyyy}-{p.REST_END_DATE:dd.MM.yyyy}, " +
                              $"цена: {p.PRICE}, кол-во: {p.QUANTITY_IN}, остаток: {p.QUANTITY_REMAIN}");
        }

        if (q.Contains("санатор") || q.Contains("sanator"))
        {
            var sans = await db.TblSpravSans
                .OrderBy(s => s.Name_San)
                .Take(50)
                .Select(s => new { s.ID, s.Name_San, s.Num_Kod })
                .ToListAsync(ct);

            sb.AppendLine("\nСписок санаториев:");
            foreach (var s in sans)
                sb.AppendLine($"  ID {s.ID}: {s.Name_San} (код {s.Num_Kod})");
        }

        if (q.Contains("план") || q.Contains("plan"))
        {
            var currentYear = DateTime.Now.Year.ToString();
            var plans = await db.TbptYearlyPlanRegions
                .Where(p => p.YEAR == currentYear)
                .Take(30)
                .ToListAsync(ct);

            if (plans.Count > 0)
            {
                sb.AppendLine($"\nГодовой план на {currentYear}: {plans.Count} записей");
            }
        }

        var result = sb.ToString();
        if (result.Length > 3000)
            result = result[..3000];

        return result;
    }
}
