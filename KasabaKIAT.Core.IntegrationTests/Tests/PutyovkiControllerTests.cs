using FluentAssertions;
using KasabaKIAT.Core.IntegrationTests.Infrastructure;
using KasabaKIAT.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace KasabaKIAT.Core.IntegrationTests.Tests;

public class PutyovkiControllerTests : TestBase
{
    public PutyovkiControllerTests(CustomWebApplicationFactory factory, DatabaseFixture fixture)
        : base(factory, fixture) { }

    private async Task<long> InsertTestPutyovkaAsync(int year = 2025, string status = "1")
    {
        var p = new TbptPutyovki
        {
            DEPARTMENT_CODE = "1",
            SANATORIUM_ID = (int)Fixture.Seed.SanatoriumId,
            USER_ID = Fixture.Seed.AdminUserId,
            PRICE = 100,
            STATUS = status,
            QUANTITY_IN = 10,
            REMAIN_BAL = 10,
            QUANTITY_OUT = 0,
            QUANTITY_REMAIN = 10,
            SMFIL = year
        };
        Db.TbptPutyovkis.Add(p);
        await Db.SaveChangesAsync();
        Db.ChangeTracker.Clear();
        return p.REC_ID;
    }

    [Fact]
    public async Task GetAll_Returns200WithPagedResult()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/putyovki");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("items").ValueKind.Should().Be(JsonValueKind.Array);
        body.GetProperty("total").GetInt32().Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task GetAll_FilterBySanId_ReturnsOnlyMatchingItems()
    {
        await InsertTestPutyovkaAsync();
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync($"/api/putyovki?sanId={Fixture.Seed.SanatoriumId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var items = body.GetProperty("items").EnumerateArray().ToList();
        items.Should().OnlyContain(item =>
            item.GetProperty("sanatoriumId").GetInt64() == Fixture.Seed.SanatoriumId);
    }

    [Fact]
    public async Task GetAll_SoftDeletedRecords_AreExcluded()
    {
        await InsertTestPutyovkaAsync(status: "0");
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/putyovki?status=0");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("items").GetArrayLength().Should().Be(0);
    }

    [Fact]
    public async Task GetById_ExistingPutyovka_Returns200()
    {
        var id = await InsertTestPutyovkaAsync();
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync($"/api/putyovki/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("recId").GetInt64().Should().Be(id);
    }

    [Fact]
    public async Task GetById_NonExistent_Returns404()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/putyovki/999999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithValidRequest_Returns201()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.PostAsJsonAsync("/api/putyovki", new
        {
            DepartmentCode = "TEST",
            SanatoriumId = (int)Fixture.Seed.SanatoriumId,
            Price = 500m,
            QuantityIn = 5,
            RestBeginDate = DateTime.UtcNow.Date,
            RestEndDate = DateTime.UtcNow.Date.AddDays(14),
            Year = 2025
        });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("recId").GetInt64().Should().BeGreaterThan(0);
        body.GetProperty("status").GetString().Should().Be("1");
    }

    [Fact]
    public async Task Create_WithoutToken_Returns401()
    {
        Auth.ClearToken(Client);
        var response = await Client.PostAsJsonAsync("/api/putyovki", new
        {
            DepartmentCode = "TEST",
            SanatoriumId = (int)Fixture.Seed.SanatoriumId
        });
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Update_ExistingPutyovka_Returns200WithChanges()
    {
        var id = await InsertTestPutyovkaAsync();
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.PutAsJsonAsync($"/api/putyovki/{id}", new
        {
            DepartmentCode = "UPDATED",
            SanatoriumId = (int)Fixture.Seed.SanatoriumId,
            Price = 999m,
            QuantityIn = 5
        });
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("price").GetDecimal().Should().Be(999m);
    }

    [Fact]
    public async Task Update_NonExistent_Returns404()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.PutAsJsonAsync("/api/putyovki/999999999", new
        {
            DepartmentCode = "X",
            SanatoriumId = 1,
            Price = 1m,
            QuantityIn = 1
        });
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_ExistingPutyovka_Returns204AndSoftDeletes()
    {
        var id = await InsertTestPutyovkaAsync();
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.DeleteAsync($"/api/putyovki/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var dbRecord = await Db.TbptPutyovkis
            .IgnoreQueryFilters()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.REC_ID == id);
        dbRecord!.STATUS.Should().Be("1");
    }

    [Fact]
    public async Task Delete_NonExistent_Returns404()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.DeleteAsync("/api/putyovki/999999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
