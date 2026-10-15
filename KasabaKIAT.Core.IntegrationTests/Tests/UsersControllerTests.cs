using FluentAssertions;
using KasabaKIAT.Core.IntegrationTests.Infrastructure;
using KasabaKIAT.Core.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace KasabaKIAT.Core.IntegrationTests.Tests;

public class UsersControllerTests : TestBase
{
    public UsersControllerTests(CustomWebApplicationFactory factory, DatabaseFixture fixture)
        : base(factory, fixture) { }

    [Fact]
    public async Task GetAll_AsAnyUser_Returns200WithPagedResult()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("items").GetArrayLength().Should().BeGreaterThanOrEqualTo(2);
        body.GetProperty("total").GetInt32().Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetAll_WithSearch_FiltersResults()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/users?search=testadmin");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("items").GetArrayLength().Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task GetAll_WithoutToken_Returns401()
    {
        Auth.ClearToken(Client);
        var response = await Client.GetAsync("/api/users");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetMe_ReturnsCurrentUser()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/users/me");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("id").GetInt64().Should().Be(Fixture.Seed.RegularUserId);
    }

    [Fact]
    public async Task GetById_ExistingUser_Returns200()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync($"/api/users/{Fixture.Seed.AdminUserId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("id").GetInt64().Should().Be(Fixture.Seed.AdminUserId);
    }

    [Fact]
    public async Task GetById_NonExistentUser_Returns404()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/users/999999999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_AsAdmin_Returns201WithLocation()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.PostAsJsonAsync("/api/users", new
        {
            Login = "newuser_test_" + Guid.NewGuid().ToString("N")[..8],
            Password = "TestPass123!",
            Email = "newuser@test.com",
            RoleId = Fixture.Seed.UserRoleId
        });
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("id").GetInt64().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Create_AsNonAdmin_Returns403()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.PostAsJsonAsync("/api/users", new
        {
            Login = "someuser",
            Password = "TestPass123!",
            Email = "some@test.com"
        });
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_WithInvalidBody_Returns400()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.PostAsJsonAsync("/api/users", new { });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_AsAdmin_Returns200WithUpdatedData()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var newEmail = "updated_" + Guid.NewGuid().ToString("N")[..8] + "@test.com";
        var response = await Client.PutAsJsonAsync($"/api/users/{Fixture.Seed.RegularUserId}", new
        {
            Email = newEmail
        });
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        body.GetProperty("email").GetString().Should().Be(newEmail);
    }

    [Fact]
    public async Task Update_AsNonAdmin_Returns403()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.PutAsJsonAsync($"/api/users/{Fixture.Seed.RegularUserId}",
            new { Email = "x@x.com" });
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Update_NonExistentUser_Returns404()
    {
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.PutAsJsonAsync("/api/users/999999999", new { Email = "x@x.com" });
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Deactivate_AsAdmin_Returns204AndSetsStateToZero()
    {
        // Create a temp user to deactivate
        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var createResp = await Client.PostAsJsonAsync("/api/users", new
        {
            Login = "deactivate_test_" + Guid.NewGuid().ToString("N")[..8],
            Password = "TestPass123!",
            Email = "deact@test.com",
            RoleId = Fixture.Seed.UserRoleId
        });
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var newUserId = created.GetProperty("id").GetInt64();

        var response = await Client.DeleteAsync($"/api/users/{newUserId}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var dbUser = await Db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.id == newUserId);
        dbUser!.state.Should().Be("0");
    }

    [Fact]
    public async Task Deactivate_AsNonAdmin_Returns403()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.DeleteAsync($"/api/users/{Fixture.Seed.AdminUserId}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
