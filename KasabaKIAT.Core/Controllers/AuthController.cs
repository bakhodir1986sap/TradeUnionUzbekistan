using KasabaKIAT.Core.DTOs.Auth;
using KasabaKIAT.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request);
        if (result == null)
            return Unauthorized(new { error = "Invalid credentials" });
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var result = await authService.RefreshAsync(request.RefreshToken);
        if (result == null)
            return Unauthorized(new { error = "Invalid or expired refresh token" });
        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
    {
        await authService.LogoutAsync(request.RefreshToken);
        return NoContent();
    }
}
