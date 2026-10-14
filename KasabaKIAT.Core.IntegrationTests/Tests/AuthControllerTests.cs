using FluentAssertions;
using KasabaKIAT.Core.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace KasabaKIAT.Core.IntegrationTests.Tests;

public class AuthControllerTests : TestBase
{
    public AuthControllerTests(CustomWebApplicationFactory factory, DatabaseFixture fixture)
        : base(factory, fixture) { }

    [Fact]
    public async Task Login_WithValidCredentials_Returns200WithTokens()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/login",
            new { Login = "testadmin", Password = "Admin123!" });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        body!.AccessToken.Should().NotBeNullOrEmpty();
        body.RefreshToken.Should().NotBeNullOrEmpty();
        body.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task Login_WithWrongPassword_Returns401()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/login",
            new { Login = "testadmin", Password = "WrongPassword!" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithUnknownUsername_Returns401()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/login",
            new { Login = "nonexistent_user_xyz", Password = "Admin123!" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithEmptyBody_Returns400()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/login", new { });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Refresh_WithValidRefreshToken_Returns200WithNewTokens()
    {
        // Login to get a refresh token
        var loginResp = await Client.PostAsJsonAsync("/api/auth/login",
            new { Login = "testadmin", Password = "Admin123!" });
        var loginBody = await loginResp.Content.ReadFromJsonAsync<LoginResponseDto>();
        var originalRefreshToken = loginBody!.RefreshToken;

        // Use the refresh token
        var response = await Client.PostAsJsonAsync("/api/auth/refresh",
            new { RefreshToken = originalRefreshToken });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        body!.AccessToken.Should().NotBeNullOrEmpty();
        body.RefreshToken.Should().NotBeNullOrEmpty();
        body.RefreshToken.Should().NotBe(originalRefreshToken);

        // Old token should be revoked in DB
        var revokedToken = await Db.ApiRefreshTokens
            .FirstOrDefaultAsync(t => t.token == originalRefreshToken);
        revokedToken!.revoked_at.Should().NotBeNull();
    }

    [Fact]
    public async Task Refresh_WithInvalidToken_Returns401()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/refresh",
            new { RefreshToken = "this-is-not-a-valid-token" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Refresh_WithAlreadyRevokedToken_Returns401()
    {
        // Login
        var loginResp = await Client.PostAsJsonAsync("/api/auth/login",
            new { Login = "testuser", Password = "User123!" });
        var loginBody = await loginResp.Content.ReadFromJsonAsync<LoginResponseDto>();
        var originalToken = loginBody!.RefreshToken;

        // First refresh (rotates the token)
        await Client.PostAsJsonAsync("/api/auth/refresh", new { RefreshToken = originalToken });

        // Second refresh with the original (now revoked) token
        var response = await Client.PostAsJsonAsync("/api/auth/refresh",
            new { RefreshToken = originalToken });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Logout_WithValidToken_Returns204()
    {
        // Login
        var loginResp = await Client.PostAsJsonAsync("/api/auth/login",
            new { Login = "testadmin", Password = "Admin123!" });
        var loginBody = await loginResp.Content.ReadFromJsonAsync<LoginResponseDto>();

        Auth.SetAdminToken(Client, Fixture.Seed.AdminUserId);
        var response = await Client.PostAsJsonAsync("/api/auth/logout",
            new { RefreshToken = loginBody!.RefreshToken });

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var revokedToken = await Db.ApiRefreshTokens
            .FirstOrDefaultAsync(t => t.token == loginBody.RefreshToken);
        revokedToken!.revoked_at.Should().NotBeNull();
    }

    [Fact]
    public async Task Logout_WithoutBearer_Returns401()
    {
        Auth.ClearToken(Client);
        var response = await Client.PostAsJsonAsync("/api/auth/logout",
            new { RefreshToken = "some-token" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private record LoginResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);
}
