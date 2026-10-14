using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/dictionary")]
[Authorize]
public class DictionaryController(KasabaKIATContext db) : ControllerBase
{
    // GET /api/dictionary?typeCode=NaznacheniyaPlateja
    [HttpGet]
    public async Task<IActionResult> GetByType([FromQuery] string typeCode)
    {
        if (string.IsNullOrEmpty(typeCode))
            return BadRequest(new { message = "typeCode is required" });

        var items = await db.TbcsDictionaries
            .Join(db.TbcsDictionaryTypes,
                d => d.DICTIONARY_TYPE_ID,
                dt => dt.REC_ID,
                (d, dt) => new { d, dt })
            .Where(x => x.dt.CODE == typeCode)
            .Select(x => new
            {
                id = x.d.REC_ID,
                nameUz = x.d.NAME_UZ,
                nameRu = x.d.NAME_RU,
                nameLt = x.d.NAME_LT,
                code = x.d.CODE,
                status = x.d.STATUS
            })
            .ToListAsync();
        return Ok(items);
    }

    // GET /api/dictionary/{id}
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.TbcsDictionaries.FindAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    // POST /api/dictionary
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DictionaryRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int.TryParse(userIdClaim, out var userId);

        var dictType = await db.TbcsDictionaryTypes
            .FirstOrDefaultAsync(dt => dt.CODE == request.TypeCode);
        if (dictType == null)
            return BadRequest(new { message = $"Dictionary type '{request.TypeCode}' not found" });

        var entry = new TbcsDictionary
        {
            NAME_RU = request.NameRu,
            NAME_UZ = request.NameUz,
            NAME_LT = request.NameLt,
            STATUS = true,
            DICTIONARY_TYPE_ID = dictType.REC_ID,
            USER_ID = userId
        };
        db.TbcsDictionaries.Add(entry);
        await db.SaveChangesAsync();
        entry.CODE = entry.REC_ID.ToString();
        await db.SaveChangesAsync();
        return Ok(new { id = entry.REC_ID });
    }

    // PUT /api/dictionary/{id}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] DictionaryRequest request)
    {
        var entry = await db.TbcsDictionaries.FindAsync(id);
        if (entry == null) return NotFound();

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int.TryParse(userIdClaim, out var userId);

        entry.NAME_RU = request.NameRu;
        entry.NAME_UZ = request.NameUz;
        entry.NAME_LT = request.NameLt;
        entry.USER_ID = userId;
        entry.STATUS = true;

        await db.SaveChangesAsync();
        return Ok(new { id = entry.REC_ID });
    }

    // DELETE /api/dictionary/{id}
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var entry = await db.TbcsDictionaries.FindAsync(id);
        if (entry == null) return NotFound();
        db.TbcsDictionaries.Remove(entry);
        await db.SaveChangesAsync();
        return NoContent();
    }
}

public record DictionaryRequest(
    string TypeCode,
    string? NameRu,
    string? NameUz,
    string? NameLt
);
