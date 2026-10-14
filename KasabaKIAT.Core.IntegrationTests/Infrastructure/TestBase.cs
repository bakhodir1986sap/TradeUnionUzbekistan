using KasabaKIAT.Core.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace KasabaKIAT.Core.IntegrationTests.Infrastructure;

[Collection("IntegrationTests")]
public abstract class TestBase : IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly DatabaseFixture Fixture;
    protected HttpClient Client { get; private set; } = null!;
    protected KasabaKIATContext Db { get; private set; } = null!;
    protected AuthHelper Auth { get; private set; } = null!;

    protected TestBase(CustomWebApplicationFactory factory, DatabaseFixture fixture)
    {
        Factory = factory;
        Fixture = fixture;
    }

    public Task InitializeAsync()
    {
        Client = Factory.CreateClient();
        Db = Fixture.CreateDbContext();
        var config = Factory.Services.GetRequiredService<Microsoft.Extensions.Configuration.IConfiguration>();
        Auth = new AuthHelper(config);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await Db.DisposeAsync();
        Client.Dispose();
    }
}

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection
    : ICollectionFixture<CustomWebApplicationFactory>,
      ICollectionFixture<DatabaseFixture>
{ }
