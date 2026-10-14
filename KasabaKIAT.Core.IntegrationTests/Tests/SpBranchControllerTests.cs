using FluentAssertions;
using KasabaKIAT.Core.IntegrationTests.Infrastructure;
using KasabaKIAT.Core.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace KasabaKIAT.Core.IntegrationTests.Tests;

/// <summary>
/// Template for all reference table controllers. Pattern repeats for SpCountry, TCamp, etc.
/// </summary>
public class SpBranchControllerTests : TestBase
{
    public SpBranchControllerTests(CustomWebApplicationFactory factory, DatabaseFixture fixture)
        : base(factory, fixture) { }

    private async Task<long> InsertBranchAsync(string name = "Temp Branch")
    {
        var item = new SpBranch { name_ru = name, name_uz = name, name_uzl = name };
        Db.SpBranches.Add(item);
        await Db.SaveChangesAsync();
        Db.ChangeTracker.Clear();
        return item.id;
    }

    [Fact]
    public async Task GetAll_WithToken_Returns200WithList()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/sp-branch");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement[]>();
        body!.Length.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {
        Auth.ClearToken(Client);
        var response = await Client.GetAsync("/api/sp-branch");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetById_ExistingItem_Returns200()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync($"/api/sp-branch/{Fixture.Seed.BranchId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("id").GetInt64().Should().Be(Fixture.Seed.BranchId);
    }

    [Fact]
    public async Task GetById_NonExistentItem_Returns404()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/sp-branch/999999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithValidBody_Returns201()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var branchName = "New Branch " + Guid.NewGuid().ToString("N")[..6];
        var response = await Client.PostAsJsonAsync("/api/sp-branch", new
        {
            name_ru = branchName,
            name_uz = branchName,
            name_uzl = branchName
        });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("id").GetInt64().Should().BeGreaterThan(0);
        body.GetProperty("name_ru").GetString().Should().Be(branchName);

        // Cleanup
        var id = body.GetProperty("id").GetInt64();
        await Client.DeleteAsync($"/api/sp-branch/{id}");
    }

    [Fact]
    public async Task Update_WithValidBody_Returns204()
    {
        var id = await InsertBranchAsync("Branch To Update");
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var newName = "Updated Branch " + Guid.NewGuid().ToString("N")[..6];
        var response = await Client.PutAsJsonAsync($"/api/sp-branch/{id}", new
        {
            id,
            name_ru = newName,
            name_uz = newName,
            name_uzl = newName
        });
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var dbItem = await Db.SpBranches.FindAsync(id);
        dbItem!.name_ru.Should().Be(newName);

        // Cleanup
        Db.SpBranches.Remove(dbItem);
        await Db.SaveChangesAsync();
    }

    [Fact]
    public async Task Update_IdMismatch_Returns400()
    {
        var id = await InsertBranchAsync("Mismatch Branch");
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.PutAsJsonAsync($"/api/sp-branch/{id}", new
        {
            id = id + 1,  // deliberate mismatch
            name_ru = "x",
            name_uz = "x"
        });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Cleanup
        var dbItem = await Db.SpBranches.FindAsync(id);
        if (dbItem != null) { Db.SpBranches.Remove(dbItem); await Db.SaveChangesAsync(); }
    }

    [Fact]
    public async Task Delete_ExistingItem_Returns204AndRemovesFromDb()
    {
        var id = await InsertBranchAsync("Branch To Delete");
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.DeleteAsync($"/api/sp-branch/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var dbItem = await Db.SpBranches.FindAsync(id);
        dbItem.Should().BeNull();
    }

    [Fact]
    public async Task Delete_NonExistentItem_Returns404()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.DeleteAsync("/api/sp-branch/999999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
