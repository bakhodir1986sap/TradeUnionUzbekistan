using KasabaKIAT.Core.DTOs.Putyovki;
using KasabaKIAT.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/putyovki")]
[Authorize]
public class PutyovkiController(IPutyovkiService putyovkiService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? sanId = null,
        [FromQuery] int? year = null,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        var result = await putyovkiService.GetListAsync(page, pageSize, sanId, year, status, ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var item = await putyovkiService.GetByIdAsync(id, ct);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePutyovkaRequest request, CancellationToken ct)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var deptCode = User.FindFirstValue("bkut_id") ?? "0";
        var item = await putyovkiService.CreateAsync(request, userId, deptCode, ct);
        return CreatedAtAction(nameof(GetById), new { id = item.RecId }, item);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] CreatePutyovkaRequest request, CancellationToken ct)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        long.TryParse(userIdClaim, out var actorId);
        var item = await putyovkiService.UpdateAsync(id, request, actorId, ct);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        long.TryParse(userIdClaim, out var actorId);
        var result = await putyovkiService.DeleteAsync(id, actorId, ct);
        return result ? NoContent() : NotFound();
    }
}
