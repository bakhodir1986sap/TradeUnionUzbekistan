using KasabaKIAT.Core.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KasabaKIAT.Core.IntegrationTests.Infrastructure;

public class DatabaseFixture : IAsyncLifetime
{
    public SeedData Seed { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        using var db = CreateDbContext();
        await db.Database.EnsureCreatedAsync();

        // Create the uspLogin stored procedure (test-only, plain-text password)
        await db.Database.ExecuteSqlRawAsync(@"
            CREATE OR ALTER PROCEDURE [dbo].[uspLogin]
                @login  NVARCHAR(256),
                @parol  NVARCHAR(256)
            AS
            BEGIN
                SET NOCOUNT ON;
                SELECT TOP 1 id
                FROM   users
                WHERE  user_name      = @login
                  AND  [password]     = @parol
                  AND  [state]        = '1'
                  AND  account_locked  = 0
                  AND  account_expired = 0;
            END");

        using var seedDb = CreateDbContext();
        Seed = await TestDataSeeder.SeedAsync(seedDb);
    }

    public async Task DisposeAsync()
    {
        using var db = CreateDbContext();
        await db.Database.EnsureDeletedAsync();
    }

    public KasabaKIATContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<KasabaKIATContext>()
            .UseSqlServer(CustomWebApplicationFactory.TestConnectionString)
            .Options;
        return new KasabaKIATContext(options);
    }
}
