using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

// ── DTOs ─────────────────────────────────────────────────────────────────────

public class FizLisoListDto
{
    public long   Id          { get; set; }
    public string? Name_Org   { get; set; }
    public string? Inn        { get; set; }
    public string? Adress     { get; set; }
    public string? Telefon    { get; set; }
    public string? Pas_Ser    { get; set; }
    public string? Pas_Num    { get; set; }
    public string? Pas_KD     { get; set; }
    public DateTime? Pas_Data { get; set; }
    public string? OtraslName { get; set; }
    public string? RegionName { get; set; }
    public int?   OtraslId    { get; set; }
    public int?   RegionId    { get; set; }
}

public class FizLisoSaveDto
{
    public string?   Name_Org  { get; set; }
    public string?   Inn       { get; set; }
    public string?   Adress    { get; set; }
    public string?   Telefon   { get; set; }
    public string?   E_Mail    { get; set; }
    public string?   Pas_Ser   { get; set; }
    public string?   Pas_Num   { get; set; }
    public string?   Pas_KD    { get; set; }
    public DateTime? Pas_Data  { get; set; }
    public int?      OtraslId  { get; set; }
    public int?      RegionId  { get; set; }
}

// ── Controller ────────────────────────────────────────────────────────────────

[ApiController]
[Route("api/fizliso")]
[Authorize]
public class FizLisoController(KasabaKIATContext db) : ControllerBase
{
    private long? CurrentUserId =>
        long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var uid) ? uid : null;

    private string? DepartmentBase =>
        User.FindFirstValue("DepartmentBase");

    // GET /api/fizliso?name=&inn=&seria=&nomer=
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] string? name,
        [FromQuery] string? inn,
        [FromQuery] string? seria,
        [FromQuery] string? nomer,
        [FromQuery] string? search)
    {
        var depBase = DepartmentBase;

        var q = db.TblSpravOrgs.Where(o => o.FACE_TYPE == 1);

        if (!string.IsNullOrWhiteSpace(depBase))
            q = q.Where(o => o.DepartmentBase == depBase || o.DepartmentBase != null && o.DepartmentBase.Contains(depBase));

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(o =>
                (o.Name_Org != null && o.Name_Org.Contains(search)) ||
                (o.INN     != null && o.INN.Contains(search))       ||
                (o.Pas_ser != null && o.Pas_ser.Contains(search))   ||
                (o.Pas_Num != null && o.Pas_Num.Contains(search)));

        if (!string.IsNullOrWhiteSpace(name))
            q = q.Where(o => o.Name_Org != null && o.Name_Org.Contains(name));

        if (!string.IsNullOrWhiteSpace(inn))
            q = q.Where(o => o.INN != null && o.INN.Contains(inn));

        if (!string.IsNullOrWhiteSpace(seria))
            q = q.Where(o => o.Pas_ser != null && o.Pas_ser.Contains(seria));

        if (!string.IsNullOrWhiteSpace(nomer))
            q = q.Where(o => o.Pas_Num != null && o.Pas_Num.Contains(nomer));

        var otraslList = await db.TblSpravOtrases.Select(ot => new { ot.ID, ot.Name_Ot }).ToListAsync();
        var regionList = await db.TblSpravRegs.Select(r => new { r.ID, r.name_ru }).ToListAsync();

        var items = await q
            .OrderBy(o => o.Name_Org)
            .Take(200)
            .Select(o => new FizLisoListDto
            {
                Id       = o.ID,
                Name_Org = o.Name_Org,
                Inn      = o.INN,
                Adress   = o.Adress,
                Telefon  = o.Telefon,
                Pas_Ser  = o.Pas_ser,
                Pas_Num  = o.Pas_Num,
                Pas_KD   = o.Pas_KD,
                Pas_Data = o.Pas_Data,
                OtraslId = o.ID_Ot,
                RegionId = o.Region,
            })
            .ToListAsync();

        foreach (var item in items)
        {
            item.OtraslName = otraslList.FirstOrDefault(o => o.ID == item.OtraslId)?.Name_Ot;
            item.RegionName = regionList.FirstOrDefault(r => r.ID == item.RegionId)?.name_ru;
        }

        return Ok(items);
    }

    // GET /api/fizliso/{id}
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var o = await db.TblSpravOrgs.FindAsync(id);
        if (o == null || o.FACE_TYPE != 1) return NotFound();

        var otraslName = o.ID_Ot.HasValue
            ? (await db.TblSpravOtrases.FindAsync((long)o.ID_Ot.Value))?.Name_Ot
            : null;
        var regionName = o.Region.HasValue
            ? (await db.TblSpravRegs.FindAsync((long)o.Region.Value))?.name_ru
            : null;

        return Ok(new FizLisoListDto
        {
            Id        = o.ID,
            Name_Org  = o.Name_Org,
            Inn       = o.INN,
            Adress    = o.Adress,
            Telefon   = o.Telefon,
            Pas_Ser   = o.Pas_ser,
            Pas_Num   = o.Pas_Num,
            Pas_KD    = o.Pas_KD,
            Pas_Data  = o.Pas_Data,
            OtraslId  = o.ID_Ot,
            RegionId  = o.Region,
            OtraslName = otraslName,
            RegionName = regionName,
        });
    }

    // POST /api/fizliso
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FizLisoSaveDto dto)
    {
        var org = new TblSpravOrg
        {
            Name_Org       = dto.Name_Org ?? string.Empty,
            INN            = dto.Inn ?? string.Empty,
            Adress         = dto.Adress,
            Telefon        = dto.Telefon,
            E_Mail         = dto.E_Mail,
            Pas_ser        = dto.Pas_Ser,
            Pas_Num        = dto.Pas_Num,
            Pas_KD         = dto.Pas_KD,
            Pas_Data       = dto.Pas_Data,
            ID_Ot          = dto.OtraslId,
            Region         = dto.RegionId,
            DepartmentBase = DepartmentBase,
            UserId         = CurrentUserId,
            IS_PRAVSOYUZ   = false,
            FACE_TYPE      = 1,
            PRAVORGLEVEL   = 4,
            DT_OST         = 0,
            KT_OST         = 0,
            DB_BAL         = 0,
            CT_BAL         = 0,
            TIMESTAMP_CH   = DateTime.Now,
        };

        db.TblSpravOrgs.Add(org);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = org.ID }, new { id = org.ID });
    }

    // PUT /api/fizliso/{id}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] FizLisoSaveDto dto)
    {
        var org = await db.TblSpravOrgs.FindAsync(id);
        if (org == null || org.FACE_TYPE != 1) return NotFound();

        org.Name_Org     = dto.Name_Org ?? org.Name_Org;
        org.INN          = dto.Inn ?? org.INN;
        org.Adress       = dto.Adress;
        org.Telefon      = dto.Telefon;
        org.E_Mail       = dto.E_Mail;
        org.Pas_ser      = dto.Pas_Ser;
        org.Pas_Num      = dto.Pas_Num;
        org.Pas_KD       = dto.Pas_KD;
        org.Pas_Data     = dto.Pas_Data;
        org.ID_Ot        = dto.OtraslId;
        org.Region       = dto.RegionId;
        org.TIMESTAMP_CH = DateTime.Now;

        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/fizliso/{id}
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var org = await db.TblSpravOrgs.FindAsync(id);
        if (org == null || org.FACE_TYPE != 1) return NotFound();

        db.TblSpravOrgs.Remove(org);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
