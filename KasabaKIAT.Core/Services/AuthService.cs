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

public class AuthService(KasabaKIATContext db, IConfiguration config, IAuditService audit, ILogger<AuthService> logger) : IAuthService
{
    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var userId = await CallUspLoginAsync(request.Login, request.Password, ct);
        if (userId <= 0)
        {
            logger.LogWarning("Login failed for {Login}", request.Login);
            return null;
        }

        var user = await db.Users
            .Include(u => u.role)
            .FirstOrDefaultAsync(u => u.id == userId, ct);

        if (user == null) return null;

        var depBase = await ResolveDepBaseAsync(user.bkut_id, ct);
        var (accessToken, expiresAt) = GenerateAccessToken(user, depBase);
        var refreshToken = await StoreRefreshTokenAsync(userId, ct);

        await audit.LogAsync(userId, "LOGIN", $"User {user.user_name} logged in", ct);

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

    public async Task<LoginResponse?> RefreshAsync(string refreshToken, CancellationToken ct = default)
    {
        var connStr = config.GetConnectionString("DefaultConnection")!;
        using var con = new SqlConnection(connStr);
        await con.OpenAsync(ct);

        var now = DateTime.UtcNow;
        long userId;
        using (var cmd = new SqlCommand(
            "SELECT user_id FROM api_refresh_tokens WHERE token = @token AND expires_at > @now AND revoked_at IS NULL",
            con))
        {
            cmd.Parameters.AddWithValue("@token", refreshToken);
            cmd.Parameters.AddWithValue("@now", now);
            var result = await cmd.ExecuteScalarAsync(ct);
            if (result == null || result == DBNull.Value) return null;
            userId = Convert.ToInt64(result);
        }

        await RevokeTokenAsync(refreshToken, con, ct);

        var user = await db.Users.Include(u => u.role).FirstOrDefaultAsync(u => u.id == userId, ct);
        if (user == null) return null;

        var depBase = await ResolveDepBaseAsync(user.bkut_id, ct);
        var (accessToken, expiresAt) = GenerateAccessToken(user, depBase);
        var newRefreshToken = await StoreRefreshTokenAsync(userId, ct);

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

    public async Task<bool> LogoutAsync(string refreshToken, CancellationToken ct = default)
    {
        var connStr = config.GetConnectionString("DefaultConnection")!;
        using var con = new SqlConnection(connStr);
        await con.OpenAsync(ct);

        long userId = 0;
        using (var cmd = new SqlCommand(
            "SELECT user_id FROM api_refresh_tokens WHERE token = @token AND revoked_at IS NULL",
            con))
        {
            cmd.Parameters.AddWithValue("@token", refreshToken);
            var result = await cmd.ExecuteScalarAsync(ct);
            if (result != null && result != DBNull.Value)
                userId = Convert.ToInt64(result);
        }

        await RevokeTokenAsync(refreshToken, con, ct);

        if (userId > 0)
            await audit.LogAsync(userId, "LOGOUT", null, ct);

        return true;
    }

    private async Task<long> CallUspLoginAsync(string login, string parol, CancellationToken ct)
    {
        var connStr = config.GetConnectionString("DefaultConnection")!;
        using var con = new SqlConnection(connStr);
        await con.OpenAsync(ct);
        using var cmd = new SqlCommand("uspLogin", con) { CommandType = System.Data.CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@login", login);
        cmd.Parameters.AddWithValue("@parol", parol);
        var result = await cmd.ExecuteScalarAsync(ct);
        return result == null || result == DBNull.Value ? 0 : Convert.ToInt64(result);
    }

    private async Task<string> ResolveDepBaseAsync(long bkutId, CancellationToken ct)
    {
        var dep = await db.TDepartmentBases
            .FirstOrDefaultAsync(d => d.bkut_id == bkutId, ct);
        return dep?.DEPARTMENT_CODE ?? bkutId.ToString();
    }

    private (string token, DateTime expiresAt) GenerateAccessToken(Models.User user, string depBase)
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
            new("dep_base", depBase),
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

    private async Task<string> StoreRefreshTokenAsync(long userId, CancellationToken ct)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var expiryDays = int.Parse(config["Jwt:RefreshTokenExpiryDays"] ?? "7");
        var expiresAt = DateTime.UtcNow.AddDays(expiryDays);

        var connStr = config.GetConnectionString("DefaultConnection")!;
        using var con = new SqlConnection(connStr);
        await con.OpenAsync(ct);

        // Revoke oldest tokens when the user already has 5 or more active ones
        using (var cleanupCmd = new SqlCommand(
            """
            UPDATE api_refresh_tokens
            SET revoked_at = GETUTCDATE()
            WHERE user_id = @uid
              AND revoked_at IS NULL
              AND expires_at > GETUTCDATE()
              AND id NOT IN (
                  SELECT TOP 4 id
                  FROM api_refresh_tokens
                  WHERE user_id = @uid AND revoked_at IS NULL AND expires_at > GETUTCDATE()
                  ORDER BY id DESC
              )
            """, con))
        {
            cleanupCmd.Parameters.AddWithValue("@uid", userId);
            await cleanupCmd.ExecuteNonQueryAsync(ct);
        }

        using var cmd = new SqlCommand(
            "INSERT INTO api_refresh_tokens (user_id, token, expires_at) VALUES (@uid, @token, @exp)",
            con);
        cmd.Parameters.AddWithValue("@uid", userId);
        cmd.Parameters.AddWithValue("@token", token);
        cmd.Parameters.AddWithValue("@exp", expiresAt);
        await cmd.ExecuteNonQueryAsync(ct);
        return token;
    }

    private static async Task RevokeTokenAsync(string token, SqlConnection con, CancellationToken ct)
    {
        using var cmd = new SqlCommand(
            "UPDATE api_refresh_tokens SET revoked_at = GETUTCDATE() WHERE token = @token",
            con);
        cmd.Parameters.AddWithValue("@token", token);
        await cmd.ExecuteNonQueryAsync(ct);
    }
}
