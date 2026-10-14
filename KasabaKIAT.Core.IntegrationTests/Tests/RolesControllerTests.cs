using FluentAssertions;
using KasabaKIAT.Core.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace KasabaKIAT.Core.IntegrationTests.Tests;

public class RolesControllerTests : TestBase
{
    public RolesControllerTests(CustomWebApplicationFactory factory, DatabaseFixture fixture)
        : base(factory, fixture) { }

    [Fact]
    public async Task GetAll_AsAdmin_Returns200WithList()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.GetAsync("/api/roles");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement[]>();
        body!.Length.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetAll_AsNonAdmin_Returns403()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/roles");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {
        Auth.ClearToken(Client);
        var response = await Client.GetAsync("/api/roles");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetById_ExistingRole_Returns200()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.GetAsync($"/api/roles/{Fixture.Seed.AdminRoleId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("id").GetInt64().Should().Be(Fixture.Seed.AdminRoleId);
        body.GetProperty("name").GetString().Should().Be("admin");
    }

    [Fact]
    public async Task GetById_NonExistentRole_Returns404()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.GetAsync("/api/roles/999999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_AsAdmin_Returns201()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var roleName = "testrole_" + Guid.NewGuid().ToString("N")[..8];
        var response = await Client.PostAsJsonAsync("/api/roles", new { Name = roleName });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("name").GetString().Should().Be(roleName);

        // Cleanup
        var roleId = body.GetProperty("id").GetInt64();
        await Client.DeleteAsync($"/api/roles/{roleId}");
    }

    [Fact]
    public async Task Create_AsNonAdmin_Returns403()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.PostAsJsonAsync("/api/roles", new { Name = "testrole" });
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_AsAdmin_Returns200WithNewName()
    {
        // Create a transient role to update
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var createResp = await Client.PostAsJsonAsync("/api/roles", new { Name = "update_test_role" });
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var roleId = created.GetProperty("id").GetInt64();

        var newName = "updated_role_" + Guid.NewGuid().ToString("N")[..8];
        var response = await Client.PutAsJsonAsync($"/api/roles/{roleId}", new { Name = newName });
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("name").GetString().Should().Be(newName);

        // Cleanup
        await Client.DeleteAsync($"/api/roles/{roleId}");
    }

    [Fact]
    public async Task Update_NonExistentRole_Returns404()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.PutAsJsonAsync("/api/roles/999999999", new { Name = "x" });
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_AsAdmin_Returns204AndRemovesFromDb()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var createResp = await Client.PostAsJsonAsync("/api/roles", new { Name = "delete_test_role" });
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var roleId = created.GetProperty("id").GetInt64();

        var response = await Client.DeleteAsync($"/api/roles/{roleId}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var dbRole = await Db.Roles.FindAsync(roleId);
        dbRole.Should().BeNull();
    }

    [Fact]
    public async Task Delete_AsNonAdmin_Returns403()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.DeleteAsync($"/api/roles/{Fixture.Seed.UserRoleId}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
