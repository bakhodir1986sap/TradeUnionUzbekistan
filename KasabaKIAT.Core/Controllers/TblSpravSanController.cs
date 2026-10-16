using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

public class SanSaveDto
{
    public string? name_San { get; set; }
    public string? num_Kod { get; set; }
    public int? kurortSanID { get; set; }
}

[ApiController]
[Route("api/tbl-sprav-san")]
[Authorize]
public class TblSpravSanController(KasabaKIATContext db) : ControllerBase
{
    private long? CurrentUserId =>
        long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var uid) ? uid : null;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await db.TblSpravSans
            .OrderBy(s => s.Name_San)
            .Select(s => new
            {
                id = s.ID,
                name_San = s.Name_San,
                num_Kod = s.Num_Kod,
                kurortSanID = s.KurortSanID,
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await db.TblSpravSans.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(new
        {
            id = item.ID,
            name_San = item.Name_San,
            num_Kod = item.Num_Kod,
            kurortSanID = item.KurortSanID,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SanSaveDto dto)
    {
        var item = new TblSpravSan
        {
            Name_San = dto.name_San,
            Num_Kod = dto.num_Kod,
            KurortSanID = dto.kurortSanID,
            UserId = CurrentUserId,
        };
        db.TblSpravSans.Add(item);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = item.ID }, new
        {
            id = item.ID,
            name_San = item.Name_San,
            num_Kod = item.Num_Kod,
            kurortSanID = item.KurortSanID,
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SanSaveDto dto)
    {
        var item = await db.TblSpravSans.FindAsync(id);
        if (item == null) return NotFound();

        item.Name_San = dto.name_San;
        item.Num_Kod = dto.num_Kod;
        item.KurortSanID = dto.kurortSanID;
        item.UserId = CurrentUserId ?? item.UserId;

        db.TblSpravSans.Update(item);
        await db.SaveChangesAsync();
        return Ok(new
        {
            id = item.ID,
            name_San = item.Name_San,
            num_Kod = item.Num_Kod,
            kurortSanID = item.KurortSanID,
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.TblSpravSans.FindAsync(id);
        if (item == null) return NotFound();
        db.TblSpravSans.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
