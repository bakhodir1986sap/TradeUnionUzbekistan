using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

// ---------------------------------------------------------------------------
// Inline DTOs
// ---------------------------------------------------------------------------

public class OrgSaveDto
{
    public long id { get; set; }
    public string? name_Org { get; set; }
    public string? inn { get; set; }
    public int? id_Ot { get; set; }
    public string? departmentBase { get; set; }
    public string? adress { get; set; }
    public string? rs { get; set; }
    public string? mfo { get; set; }
    public string? bank { get; set; }
    public string? okonx { get; set; }
    public string? telefon { get; set; }
    public int? region { get; set; }
    public string? e_Mail { get; set; }
    public string? owner_CODE { get; set; }
    public string? director { get; set; }
    public string? buxgalter { get; set; }
    public string? doverennoe_Liso { get; set; }
    public string? orig_INN { get; set; }
    public string? to_DepCode { get; set; }
    public int? face_Type { get; set; }
    public string? pas_ser { get; set; }
    public string? pas_Num { get; set; }
    public string? pas_KD { get; set; }
    public DateTime? pas_Data { get; set; }
}

public class FizLicoDto
{
    public long ID { get; set; }
    public string? Name_Face { get; set; }
    public string? inn { get; set; }
    public string? adress { get; set; }
    public string? telefon { get; set; }
    public string? pas_ser { get; set; }
    public string? pas_Num { get; set; }
    public string? pas_kd { get; set; }
    public DateTime? pas_data { get; set; }
    public long? UserId { get; set; }
}

public class FizLicoSaveDto
{
    public string? Name_Face { get; set; }
    public string? inn { get; set; }
    public string? adress { get; set; }
    public string? telefon { get; set; }
    public string? pas_ser { get; set; }
    public string? pas_Num { get; set; }
    public string? pas_kd { get; set; }
    public DateTime? pas_data { get; set; }
}

// ---------------------------------------------------------------------------
// OrganizaciiController — covers /api/organizations (extended) + /api/otrasl
// ---------------------------------------------------------------------------

[ApiController]
[Route("api/organizations")]
[Authorize]
public class OrganizaciiController(KasabaKIATContext db) : ControllerBase
{
    private long? CurrentUserId =>
        long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var uid) ? uid : null;

    // -----------------------------------------------------------------------
    // GET /api/organizations
    // ?search=&inn=&name=&otraslId=&page=
    // Returns { items, count }
    // -----------------------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetOrganizationList(
        [FromQuery] string? search,
        [FromQuery] string? inn,
        [FromQuery] string? name,
        [FromQuery] long? otraslId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = db.TblSpravOrgs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(o =>
                (o.Name_Org != null && o.Name_Org.Contains(search)) ||
                (o.INN != null && o.INN.Contains(search)));

        if (!string.IsNullOrWhiteSpace(inn))
            query = query.Where(o => o.INN != null && o.INN.Contains(inn));

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(o =>
                (o.Name_Org != null && o.Name_Org.Contains(name)));

        if (otraslId.HasValue)
            query = query.Where(o => o.ID_Ot == otraslId);

        var count = await query.CountAsync();

        var items = await query
            .OrderBy(o => o.Name_Org)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new
            {
                id = o.ID,
                name_Org = o.Name_Org,
                inn = o.INN,
                rs = o.RS,
                mfo = o.MFO,
                bank = o.Bank,
                okonx = o.OKONX,
                adress = o.Adress,
                telefon = o.Telefon,
                region = o.Region,
                e_Mail = o.E_Mail,
                owner_CODE = o.OWNER_CODE,
                id_Ot = o.ID_Ot,
                departmentBase = o.DepartmentBase,
                director = o.DIRECTOR,
                buxgalter = o.BUXGALTER
            })
            .ToListAsync();

        return Ok(new { items, count });
    }

    // -----------------------------------------------------------------------
    // GET /api/organizations/{id}
    // -----------------------------------------------------------------------
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetOrganization(long id)
    {
        var item = await db.TblSpravOrgs.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    // -----------------------------------------------------------------------
    // GET /api/organizations/otrasl  — list of industries
    // -----------------------------------------------------------------------
    [HttpGet("otrasl")]
    public async Task<IActionResult> GetOtraslForOrganization()
    {
        var items = await db.TblSpravOtrases
            .OrderBy(o => o.Name_Ot)
            .Select(o => new { id = o.ID, name_Ot = o.Name_Ot })
            .ToListAsync();
        return Ok(items);
    }

    // -----------------------------------------------------------------------
    // GET /api/organizations/otrasl-rasxod
    // Returns industries filtered for rasxod context (currently same list;
    // extend predicate if business logic differs).
    // -----------------------------------------------------------------------
    [HttpGet("otrasl-rasxod")]
    public async Task<IActionResult> GetOtraslForOrganizationRasxod()
    {
        var items = await db.TblSpravOtrases
            .OrderBy(o => o.Name_Ot)
            .Select(o => new { id = o.ID, name_Ot = o.Name_Ot })
            .ToListAsync();
        return Ok(items);
    }

    // -----------------------------------------------------------------------
    // GET /api/organizations/regions
    // -----------------------------------------------------------------------
    [HttpGet("regions")]
    public async Task<IActionResult> GetRegionForOrganization()
    {
        var items = await db.TblSpravRegs
            .OrderBy(r => r.name_ru)
            .Select(r => new { id = r.ID, name_Reg = r.name_ru, code = r.code })
            .ToListAsync();
        return Ok(items);
    }

    // -----------------------------------------------------------------------
    // GET /api/organizations/empty-carcas
    // Returns an empty org shell (field list for the front-end form).
    // -----------------------------------------------------------------------
    [HttpGet("empty-carcas")]
    public IActionResult GetEmptyCarcas()
    {
        return Ok(new OrgSaveDto());
    }

    // -----------------------------------------------------------------------
    // POST /api/organizations
    // -----------------------------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> SaveOrganization([FromBody] OrgSaveDto dto)
    {
        var userId = CurrentUserId;

        var org = new TblSpravOrg
        {
            Name_Org = dto.name_Org ?? string.Empty,
            INN = dto.inn ?? string.Empty,
            ID_Ot = dto.id_Ot,
            DepartmentBase = dto.departmentBase,
            Adress = dto.adress,
            RS = dto.rs,
            MFO = dto.mfo,
            Bank = dto.bank,
            OKONX = dto.okonx,
            Telefon = dto.telefon,
            Region = dto.region,
            E_Mail = dto.e_Mail,
            OWNER_CODE = dto.owner_CODE,
            DIRECTOR = dto.director,
            BUXGALTER = dto.buxgalter,
            DOVERENNOE_LISO = dto.doverennoe_Liso,
            ORIG_INN = dto.orig_INN,
            TO_DEPCODE = dto.to_DepCode,
            FACE_TYPE = dto.face_Type,
            Pas_ser = dto.pas_ser,
            Pas_Num = dto.pas_Num,
            Pas_KD = dto.pas_KD,
            Pas_Data = dto.pas_Data,
            UserId = userId
        };

        db.TblSpravOrgs.Add(org);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrganization), new { id = org.ID }, org);
    }

    // -----------------------------------------------------------------------
    // PUT /api/organizations/{id}
    // -----------------------------------------------------------------------
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateOrganization(long id, [FromBody] OrgSaveDto dto)
    {
        var org = await db.TblSpravOrgs.FindAsync(id);
        if (org == null) return NotFound();

        org.Name_Org = dto.name_Org ?? org.Name_Org;
        org.INN = dto.inn ?? org.INN;
        org.ID_Ot = dto.id_Ot;
        org.DepartmentBase = dto.departmentBase;
        org.Adress = dto.adress;
        org.RS = dto.rs;
        org.MFO = dto.mfo;
        org.Bank = dto.bank;
        org.OKONX = dto.okonx;
        org.Telefon = dto.telefon;
        org.Region = dto.region;
        org.E_Mail = dto.e_Mail;
        org.OWNER_CODE = dto.owner_CODE;
        org.DIRECTOR = dto.director;
        org.BUXGALTER = dto.buxgalter;
        org.DOVERENNOE_LISO = dto.doverennoe_Liso;
        org.ORIG_INN = dto.orig_INN;
        org.TO_DEPCODE = dto.to_DepCode;
        org.FACE_TYPE = dto.face_Type;
        org.Pas_ser = dto.pas_ser;
        org.Pas_Num = dto.pas_Num;
        org.Pas_KD = dto.pas_KD;
        org.Pas_Data = dto.pas_Data;
        org.UserId = CurrentUserId ?? org.UserId;

        db.TblSpravOrgs.Update(org);
        await db.SaveChangesAsync();
        return Ok(org);
    }

    // -----------------------------------------------------------------------
    // DELETE /api/organizations/{id}
    // -----------------------------------------------------------------------
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteOrganization(long id)
    {
        var org = await db.TblSpravOrgs.FindAsync(id);
        if (org == null) return NotFound();
        db.TblSpravOrgs.Remove(org);  // Remove() re-attaches as Deleted
        await db.SaveChangesAsync();
        return NoContent();
    }

    // -----------------------------------------------------------------------
    // GET /api/organizations/by-inn/{inn}
    // -----------------------------------------------------------------------
    [HttpGet("by-inn/{inn}")]
    public async Task<IActionResult> GetByInn(string inn)
    {
        var item = await db.TblSpravOrgs
            .Where(o => o.INN == inn)
            .FirstOrDefaultAsync();
        if (item == null) return NotFound();
        return Ok(item);
    }

    // -----------------------------------------------------------------------
    // GET /api/organizations/yur-liso-data/{id}
    // Returns legal entity data (same as GetById, kept as named endpoint).
    // -----------------------------------------------------------------------
    [HttpGet("yur-liso-data/{id:long}")]
    public async Task<IActionResult> GetYurLisoData(long id)
    {
        var item = await db.TblSpravOrgs
            .Where(o => o.ID == id)
            .Select(o => new
            {
                id = o.ID,
                name_Org = o.Name_Org,
                inn = o.INN,
                rs = o.RS,
                mfo = o.MFO,
                bank = o.Bank,
                okonx = o.OKONX,
                adress = o.Adress,
                telefon = o.Telefon,
                region = o.Region,
                e_Mail = o.E_Mail,
                owner_CODE = o.OWNER_CODE,
                id_Ot = o.ID_Ot,
                departmentBase = o.DepartmentBase,
                director = o.DIRECTOR,
                buxgalter = o.BUXGALTER
            })
            .FirstOrDefaultAsync();

        if (item == null) return NotFound();
        return Ok(item);
    }

    // -----------------------------------------------------------------------
    // POST /api/organizations/change-otrasl-massive
    // Body: { ids: [1,2,3], otraslId: 5 }
    // -----------------------------------------------------------------------
    [HttpPost("change-otrasl-massive")]
    public async Task<IActionResult> ChangeOtraslMassive([FromBody] ChangeOtraslMassiveRequest req)
    {
        if (req.Ids == null || req.Ids.Length == 0)
            return BadRequest("No IDs provided.");

        var orgs = await db.TblSpravOrgs
            .Where(o => req.Ids.Contains(o.ID))
            .ToListAsync();

        foreach (var org in orgs)
        {
            org.ID_Ot = req.OtraslId;
            db.TblSpravOrgs.Update(org);
        }

        await db.SaveChangesAsync();
        return Ok(new { updated = orgs.Count });
    }

    // -----------------------------------------------------------------------
    // GET /api/organizations/otrasl-map/{id}
    // Returns the otrasl (industry) record linked to a given org.
    // -----------------------------------------------------------------------
    [HttpGet("otrasl-map/{orgId:long}")]
    public async Task<IActionResult> OrganizationOtrasl(long orgId)
    {
        var org = await db.TblSpravOrgs
            .Where(o => o.ID == orgId)
            .Select(o => new { o.ID_Ot })
            .FirstOrDefaultAsync();

        if (org == null) return NotFound();
        if (org.ID_Ot == null) return Ok(null);

        var otrasl = await db.TblSpravOtrases
            .Where(o => o.ID == org.ID_Ot)
            .Select(o => new { id = o.ID, name_Ot = o.Name_Ot })
            .FirstOrDefaultAsync();

        return Ok(otrasl);
    }
}

// Helper request record for the massive otrasl change
public class ChangeOtraslMassiveRequest
{
    public long[] Ids { get; set; } = [];
    public int? OtraslId { get; set; }
}

// FizLicaController with full CRUD lives in RasxodPutyovokController.cs
// (it was already defined there; this file only defines the DTOs it uses).

// ---------------------------------------------------------------------------
// SanatoriumsListController — GET /api/sanatoriums
// Thin controller so filters can load sanatorium list independently.
// ---------------------------------------------------------------------------

[ApiController]
[Route("api/sanatoriums")]
[Authorize]
public class SanatoriumsListController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await db.TblSpravSans
            .OrderBy(s => s.Name_San)
            .Select(s => new { s.ID, s.Name_San, s.Num_Kod })
            .ToListAsync();
        return Ok(items);
    }
}

// ---------------------------------------------------------------------------
// OtraslController — GET /api/otrasl
// Shortcut so the PrixodPutevok module can fetch industries without
// going through /api/organizations/otrasl.
// ---------------------------------------------------------------------------

[ApiController]
[Route("api/otrasl")]
[Authorize]
public class OtraslController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await db.TblSpravOtrases
            .OrderBy(o => o.Name_Ot)
            .Select(o => new { id = o.ID, name = o.Name_Ot })
            .ToListAsync();
        return Ok(items);
    }
}

// ---------------------------------------------------------------------------
// RegionController — GET /api/region
// ---------------------------------------------------------------------------

[ApiController]
[Route("api/region")]
[Authorize]
public class RegionController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await db.TblSpravRegs
            .OrderBy(r => r.name_ru)
            .Select(r => new { id = r.ID, name = r.name_ru })
            .ToListAsync();
        return Ok(items);
    }
}

// ---------------------------------------------------------------------------
// SpravochnikController — GET /api/spravochnik/profsoyuz
// ---------------------------------------------------------------------------

[ApiController]
[Route("api/spravochnik")]
[Authorize]
public class SpravochnikController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet("profsoyuz")]
    public async Task<IActionResult> GetProfsoyuz()
    {
        var items = await db.TbkuStrokaProfsoyazs
            .OrderBy(p => p.ProfsoyzNaimenovanie)
            .Select(p => new { id = p.ID, name = p.ProfsoyzNaimenovanie })
            .ToListAsync();
        return Ok(items);
    }
}
