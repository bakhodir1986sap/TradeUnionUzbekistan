using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs.Auth;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KasabaKIAT.Core.Services;

public class AuthService(KasabaKIATContext db, IConfiguration config) : IAuthService
{
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var userId = await CallUspLoginAsync(request.Login, request.Password);
        if (userId <= 0) return null;

        var user = await db.Users
            .Include(u => u.role)
            .FirstOrDefaultAsync(u => u.id == userId);

        if (user == null) return null;

        var (accessToken, expiresAt) = GenerateAccessToken(user);
        var refreshToken = await StoreRefreshTokenAsync(userId);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            User = new UserInfo
            {
                Id = user.id,
                Email = user.email,
                UserName = user.user_name,
                Role = user.role?.name,
                BkutId = user.bkut_id,
                IsSuperAdmin = user.is_super_admin ?? false
            }
        };
    }

    public async Task<LoginResponse?> RefreshAsync(string refreshToken)
    {
        var connStr = config.GetConnectionString("DefaultConnection")!;
        using var con = new SqlConnection(connStr);
        await con.OpenAsync();

        var now = DateTime.UtcNow;
        long userId;
        using (var cmd = new SqlCommand(
            "SELECT user_id FROM api_refresh_tokens WHERE token = @token AND expires_at > @now AND revoked_at IS NULL",
            con))
        {
            cmd.Parameters.AddWithValue("@token", refreshToken);
            cmd.Parameters.AddWithValue("@now", now);
            var result = await cmd.ExecuteScalarAsync();
            if (result == null || result == DBNull.Value) return null;
            userId = Convert.ToInt64(result);
        }

        await RevokeTokenAsync(refreshToken, con);

        var user = await db.Users.Include(u => u.role).FirstOrDefaultAsync(u => u.id == userId);
        if (user == null) return null;

        var (accessToken, expiresAt) = GenerateAccessToken(user);
        var newRefreshToken = await StoreRefreshTokenAsync(userId);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = expiresAt,
            User = new UserInfo
            {
                Id = user.id,
                Email = user.email,
                UserName = user.user_name,
                Role = user.role?.name,
                BkutId = user.bkut_id,
                IsSuperAdmin = user.is_super_admin ?? false
            }
        };
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var connStr = config.GetConnectionString("DefaultConnection")!;
        using var con = new SqlConnection(connStr);
        await con.OpenAsync();
        await RevokeTokenAsync(refreshToken, con);
        return true;
    }

    private async Task<long> CallUspLoginAsync(string login, string parol)
    {
        var connStr = config.GetConnectionString("DefaultConnection")!;
        using var con = new SqlConnection(connStr);
        await con.OpenAsync();
        using var cmd = new SqlCommand("uspLogin", con) { CommandType = System.Data.CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@login", login);
        cmd.Parameters.AddWithValue("@parol", parol);
        var result = await cmd.ExecuteScalarAsync();
        return result == null || result == DBNull.Value ? 0 : Convert.ToInt64(result);
    }

    private (string token, DateTime expiresAt) GenerateAccessToken(Models.User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiryMinutes = int.Parse(config["Jwt:AccessTokenExpiryMinutes"] ?? "60");
        var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.id.ToString()),
            new(ClaimTypes.NameIdentifier, user.id.ToString()),
            new(ClaimTypes.Email, user.email ?? string.Empty),
            new(ClaimTypes.Name, user.user_name ?? string.Empty),
            new("bkut_id", user.bkut_id.ToString()),
            new("is_super_admin", (user.is_super_admin ?? false).ToString())
        };

        if (user.role != null)
            claims.Add(new Claim(ClaimTypes.Role, user.role.name ?? string.Empty));

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    private async Task<string> StoreRefreshTokenAsync(long userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var expiryDays = int.Parse(config["Jwt:RefreshTokenExpiryDays"] ?? "7");
        var expiresAt = DateTime.UtcNow.AddDays(expiryDays);

        var connStr = config.GetConnectionString("DefaultConnection")!;
        using var con = new SqlConnection(connStr);
        await con.OpenAsync();
        using var cmd = new SqlCommand(
            "INSERT INTO api_refresh_tokens (user_id, token, expires_at) VALUES (@uid, @token, @exp)",
            con);
        cmd.Parameters.AddWithValue("@uid", userId);
        cmd.Parameters.AddWithValue("@token", token);
        cmd.Parameters.AddWithValue("@exp", expiresAt);
        await cmd.ExecuteNonQueryAsync();
        return token;
    }

    private static async Task RevokeTokenAsync(string token, SqlConnection con)
    {
        using var cmd = new SqlCommand(
            "UPDATE api_refresh_tokens SET revoked_at = GETUTCDATE() WHERE token = @token",
            con);
        cmd.Parameters.AddWithValue("@token", token);
        await cmd.ExecuteNonQueryAsync();
    }
}
