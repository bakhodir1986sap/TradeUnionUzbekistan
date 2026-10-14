using FluentAssertions;
using KasabaKIAT.Core.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace KasabaKIAT.Core.IntegrationTests.Tests;

public class ReferenceControllerTests : TestBase
{
    public ReferenceControllerTests(CustomWebApplicationFactory factory, DatabaseFixture fixture)
        : base(factory, fixture) { }

    [Fact]
    public async Task GetSanatoriums_WithToken_Returns200WithArray()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/reference/sanatoriums");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement[]>();
        body.Should().NotBeNull();
        body!.Length.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task GetSanatoriums_WithoutToken_Returns401()
    {
        Auth.ClearToken(Client);
        var response = await Client.GetAsync("/api/reference/sanatoriums");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetBranches_WithToken_Returns200()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/reference/branches");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement[]>();
        body!.Length.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task GetRegions_WithToken_Returns200()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/reference/regions");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement[]>();
        body.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCountries_WithToken_Returns200()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/reference/countries");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement[]>();
        body.Should().NotBeNull();
    }

    [Fact]
    public async Task GetYears_WithToken_Returns200WithEightItems()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/reference/years");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement[]>();
        body!.Length.Should().Be(8);
    }

    [Fact]
    public async Task GetKvartalList_WithToken_Returns200WithFourItems()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/reference/kvartal-list");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement[]>();
        body!.Length.Should().Be(4);
    }

    [Fact]
    public async Task GetPol_WithToken_Returns200WithTwoItems()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/reference/pol");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement[]>();
        body!.Length.Should().Be(2);
    }

    [Fact]
    public async Task GetMfo_WithToken_Returns200()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/reference/mfo");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement[]>();
        body.Should().NotBeNull();
    }

    [Fact]
    public async Task GetDictionary_WithToken_Returns200()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/reference/dictionary/TEST");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<JsonElement[]>();
        body.Should().NotBeNull();
    }

    [Fact]
    public async Task GetMfoByCode_NonExistentCode_Returns404()
    {
        Auth.SetUserToken(Client, Fixture.Seed.RegularUserId);
        var response = await Client.GetAsync("/api/reference/mfo/ZZZNONSENSE999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
