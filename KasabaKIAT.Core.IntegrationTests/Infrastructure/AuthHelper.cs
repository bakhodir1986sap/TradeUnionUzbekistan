using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace KasabaKIAT.Core.IntegrationTests.Infrastructure;

public class AuthHelper
{
    private readonly IConfiguration _config;

    public AuthHelper(IConfiguration config) => _config = config;

    /// <summary>Generates a JWT token directly (bypasses uspLogin). Use for all non-auth tests.</summary>
    public string GenerateToken(long userId, string role, long bkutId = 1, bool isSuperAdmin = false)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiryMinutes = int.Parse(_config["Jwt:AccessTokenExpiryMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Role, role),
            new("bkut_id", bkutId.ToString()),
            new("is_super_admin", isSuperAdmin.ToString().ToLower())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public void SetAdminToken(HttpClient client, long userId, long bkutId = 1)
    {
        var token = GenerateToken(userId, "admin", bkutId);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void SetUserToken(HttpClient client, long userId, long bkutId = 1)
    {
        var token = GenerateToken(userId, "user", bkutId);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearToken(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = null;
    }
}
