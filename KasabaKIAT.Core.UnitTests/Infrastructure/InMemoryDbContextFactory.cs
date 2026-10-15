using KasabaKIAT.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.UnitTests.Infrastructure;

internal static class InMemoryDbContextFactory
{
    // Each call gets a unique database name so tests are fully isolated from each other.
    // httpContextAccessor: null means CurrentBkutId returns null, so the TbptPutyovkis
    // query filter reduces to STATUS != "0" only — no tenancy filtering in unit tests.
    public static KasabaKIATContext Create(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<KasabaKIATContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        return new KasabaKIATContext(options, httpContextAccessor: null);
    }
}
