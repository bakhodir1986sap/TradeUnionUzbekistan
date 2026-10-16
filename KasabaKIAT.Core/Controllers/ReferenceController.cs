using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs.Reference;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/reference")]
[Authorize]
[OutputCache(PolicyName = "reference")]
public class ReferenceController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet("sanatorium-types")]
    public async Task<IActionResult> GetSanatoriumTypes()
    {
        var items = await db.SpSanatoriumTypes
            .Select(s => new ReferenceItemDto
            {
                Code = s.id.ToString(),
                NameRu = s.name_ru,
                NameUz = s.name_uz,
                NameUzl = s.name_uzl,
                CreateDate = s.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("regions")]
    public async Task<IActionResult> GetRegions()
    {
        var items = await db.TblSpravRegs
            .Select(r => new
            {
                id = r.ID,
                code = r.code ?? r.ID.ToString(),
                nameRu = r.name_ru,
                nameUz = r.name_uz,
                nameUzl = r.name_uzl,
                createDate = r.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("branches")]
    public async Task<IActionResult> GetBranches()
    {
        var items = await db.SpBranches
            .Select(b => new
            {
                id = b.id,
                code = b.id.ToString(),
                nameRu = b.name_ru,
                nameUz = b.name_uz,
                nameUzl = b.name_uzl,
                createDate = b.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries()
    {
        var items = await db.SpCountries
            .Select(c => new
            {
                id = c.id,
                code = c.id.ToString(),
                nameRu = c.name_ru,
                nameUz = c.name_uz,
                nameUzl = c.name_uzl,
                createDate = c.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("positions")]
    public async Task<IActionResult> GetPositions()
    {
        var items = await db.SpPositions
            .Select(p => new
            {
                id = p.id,
                code = p.id.ToString(),
                nameRu = p.name_ru,
                nameUz = p.name_uz,
                nameUzl = p.name_uzl,
                createDate = p.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("departments")]
    public async Task<IActionResult> GetDepartments()
    {
        var items = await db.TDepartmentBases
            .Select(d => new ReferenceItemDto
            {
                Code = d.DEPARTMENT_CODE ?? d.id.ToString(),
                NameRu = d.name_ru,
                NameUz = d.name_uz,
                NameUzl = d.name_uzl,
                CreateDate = d.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("years")]
    public IActionResult GetYears()
    {
        var currentYear = DateTime.Now.Year;
        var years = Enumerable.Range(currentYear - 5, 8)
            .Select(y => new { value = y, label = y.ToString() })
            .ToList();
        return Ok(years);
    }

    [HttpGet("camps")]
    public async Task<IActionResult> GetCamps()
    {
        var items = await db.TCamps
            .Select(c => new { id = c.id, code = (string?)null, nameRu = c.camps_name, nameUz = c.camps_name })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("kvartals")]
    public async Task<IActionResult> GetKvartals()
    {
        var items = await db.TblSpravKvs
            .Select(k => new { id = k.ID, name = k.Name_Kv })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("soato")]
    public async Task<IActionResult> GetSoato()
    {
        var items = await db.SpSoatos
            .Select(s => new ReferenceItemDto
            {
                Code = s.code,
                NameRu = s.name_ru,
                NameUz = s.name_uz,
                NameUzl = s.name_uzl
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("nations")]
    public async Task<IActionResult> GetNations()
    {
        var items = await db.SpNations
            .Select(n => new
            {
                id = n.id,
                code = n.code,
                nameRu = n.name_ru,
                nameUz = n.name_uz,
                nameUzl = n.name_uzl,
                createDate = n.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("degrees")]
    public async Task<IActionResult> GetDegrees()
    {
        var items = await db.SpDegrees
            .Select(d => new
            {
                id = d.id,
                code = d.code,
                nameRu = d.name_ru,
                nameUz = d.name_uz,
                nameUzl = d.name_uzl,
                createDate = d.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("contract-types")]
    public async Task<IActionResult> GetContractTypes()
    {
        var items = await db.SpContractTypes
            .Select(c => new ReferenceItemDto
            {
                Code = c.code,
                NameRu = c.name_ru,
                NameUz = c.name_uz,
                NameUzl = c.name_uzl,
                CreateDate = c.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("department-types")]
    public async Task<IActionResult> GetDepartmentTypes()
    {
        var items = await db.SpDepartmentTypes
            .Select(d => new ReferenceItemDto
            {
                Code = d.code,
                NameRu = d.name_ru,
                NameUz = d.name_uz,
                NameUzl = d.name_uzl,
                CreateDate = d.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("sections")]
    public async Task<IActionResult> GetSections()
    {
        var items = await db.SpSections
            .Select(s => new
            {
                id = s.id,
                code = s.code,
                nameRu = s.name_ru,
                nameUz = s.name_uz,
                nameUzl = s.name_uzl,
                createDate = s.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("sanatorium-profils")]
    public async Task<IActionResult> GetSanatoriumProfils()
    {
        var items = await db.SpSanatoriumProfils
            .Select(s => new ReferenceItemDto
            {
                Code = s.code,
                NameRu = s.name_ru,
                NameUz = s.name_uz,
                NameUzl = s.name_uzl,
                CreateDate = s.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("okonx")]
    public async Task<IActionResult> GetOkonx()
    {
        var items = await db.SpOkonxes
            .Select(o => new ReferenceItemDto
            {
                Code = o.code,
                NameRu = o.name_ru,
                NameUz = o.name_uz,
                NameUzl = o.name_uzl,
                CreateDate = o.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("opf")]
    public async Task<IActionResult> GetOpf()
    {
        var items = await db.SpOpfs
            .Select(o => new ReferenceItemDto
            {
                Code = o.code,
                NameRu = o.name_ru,
                NameUz = o.name_uz,
                NameUzl = o.name_uzl,
                CreateDate = o.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("employees")]
    public async Task<IActionResult> GetEmployees()
    {
        var items = await db.SpEmployees
            .Select(e => new ReferenceItemDto
            {
                Code = e.code,
                NameRu = e.name_ru,
                NameUz = e.name_uz,
                NameUzl = e.name_uzl,
                CreateDate = e.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("otras")]
    public async Task<IActionResult> GetOtras()
    {
        var items = await db.TblSpravOtrases
            .Select(o => new { id = o.ID, name = o.Name_Ot })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("villages")]
    public async Task<IActionResult> GetVillages()
    {
        var items = await db.TblSpravVils
            .Select(v => new { id = v.ID, name = v.Name_Vil, code = v.CodeVil })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("mfo")]
    public async Task<IActionResult> GetMfo()
    {
        var items = await db.TblSpravMfos
            .Select(m => new { id = m.ID, code = m.Code_MFO, name = m.Name_MFO })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("mfo/{code}")]
    public async Task<IActionResult> GetMfoByCode(string code)
    {
        var item = await db.TblSpravMfos
            .Where(m => m.Code_MFO == code)
            .Select(m => new { id = m.ID, code = m.Code_MFO, name = m.Name_MFO })
            .FirstOrDefaultAsync();
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    [HttpGet("face")]
    public async Task<IActionResult> GetFace()
    {
        var items = await db.TblSpravFaces
            .Select(f => new { id = f.ID, name = f.Name_Face })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("os")]
    public async Task<IActionResult> GetOs()
    {
        var items = await db.TblSpravOses
            .Select(o => new { id = o.ID, name = o.Name_Os })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("pr")]
    public async Task<IActionResult> GetPr()
    {
        var items = await db.TblSpravPrs
            .Select(p => new { id = p.ID, name = p.Name_Pr, value = p.Value })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("normativdoc")]
    public async Task<IActionResult> GetNormativDoc()
    {
        var items = await db.TNormativDocs
            .Select(n => new { id = n.id, nameUz = n.name_uz, nameRu = n.name_ru })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("sanatorium-list")]
    public async Task<IActionResult> GetSanatoriumList()
    {
        var items = await db.TSanatoriums
            .Select(s => new { id = s.id, nameUz = s.name_uz, nameRu = s.name_ru, code = s.code })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("dictionary/{typeCode}")]
    public async Task<IActionResult> GetDictionary(string typeCode)
    {
        var items = await db.TbcsDictionaries
            .Join(db.TbcsDictionaryTypes,
                d => d.DICTIONARY_TYPE_ID,
                dt => dt.REC_ID,
                (d, dt) => new { d, dt })
            .Where(x => x.dt.CODE == typeCode)
            .Select(x => new { recId = x.d.REC_ID, nameUz = x.d.NAME_UZ, nameRu = x.d.NAME_RU })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("pol")]
    public IActionResult GetPol()
    {
        var items = new[]
        {
            new { code = 0, name = "Женщины" },
            new { code = 1, name = "Мужчины" }
        };
        return Ok(items);
    }

    [HttpGet("kvartal-list")]
    public IActionResult GetKvartalList()
    {
        var items = new[]
        {
            new { value = 1 },
            new { value = 2 },
            new { value = 3 },
            new { value = 4 }
        };
        return Ok(items);
    }
}

// ---------------------------------------------------------------------------
// ReferenceCrudController — POST/PUT/DELETE for reference tables
// Route: api/reference/{endpoint}/{id}
// ---------------------------------------------------------------------------

public class RefSaveDto
{
    public string? code { get; set; }
    public string? nameRu { get; set; }
    public string? nameUz { get; set; }
    public string? nameUzl { get; set; }
}

[ApiController]
[Route("api/reference")]
[Authorize]
public class ReferenceCrudController(KasabaKIATContext db, IOutputCacheStore cache) : ControllerBase
{
    private async Task InvalidateCache(CancellationToken ct = default)
        => await cache.EvictByTagAsync("reference", ct);

    // ── regions ──────────────────────────────────────────────────────────────

    [HttpPost("regions")]
    public async Task<IActionResult> CreateRegion([FromBody] RefSaveDto dto)
    {
        var item = new TblSpravReg { name_ru = dto.nameRu, name_uz = dto.nameUz, name_uzl = dto.nameUzl, code = dto.code, s_create = DateTime.Now };
        db.TblSpravRegs.Add(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.ID, code = item.code, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpPut("regions/{id:long}")]
    public async Task<IActionResult> UpdateRegion(long id, [FromBody] RefSaveDto dto)
    {
        var item = await db.TblSpravRegs.FindAsync(id);
        if (item == null) return NotFound();
        item.name_ru = dto.nameRu; item.name_uz = dto.nameUz; item.name_uzl = dto.nameUzl; item.code = dto.code;
        db.TblSpravRegs.Update(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.ID, code = item.code, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpDelete("regions/{id:long}")]
    public async Task<IActionResult> DeleteRegion(long id)
    {
        var item = await db.TblSpravRegs.FindAsync(id);
        if (item == null) return NotFound();
        db.TblSpravRegs.Remove(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return NoContent();
    }

    // ── branches ─────────────────────────────────────────────────────────────

    [HttpPost("branches")]
    public async Task<IActionResult> CreateBranch([FromBody] RefSaveDto dto)
    {
        var item = new SpBranch { name_ru = dto.nameRu, name_uz = dto.nameUz, name_uzl = dto.nameUzl, s_create = DateTime.Now };
        db.SpBranches.Add(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpPut("branches/{id:long}")]
    public async Task<IActionResult> UpdateBranch(long id, [FromBody] RefSaveDto dto)
    {
        var item = await db.SpBranches.FindAsync(id);
        if (item == null) return NotFound();
        item.name_ru = dto.nameRu; item.name_uz = dto.nameUz; item.name_uzl = dto.nameUzl;
        db.SpBranches.Update(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpDelete("branches/{id:long}")]
    public async Task<IActionResult> DeleteBranch(long id)
    {
        var item = await db.SpBranches.FindAsync(id);
        if (item == null) return NotFound();
        db.SpBranches.Remove(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return NoContent();
    }

    // ── countries ────────────────────────────────────────────────────────────

    [HttpPost("countries")]
    public async Task<IActionResult> CreateCountry([FromBody] RefSaveDto dto)
    {
        var item = new SpCountry { name_ru = dto.nameRu, name_uz = dto.nameUz, name_uzl = dto.nameUzl, s_create = DateTime.Now };
        db.SpCountries.Add(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpPut("countries/{id:long}")]
    public async Task<IActionResult> UpdateCountry(long id, [FromBody] RefSaveDto dto)
    {
        var item = await db.SpCountries.FindAsync(id);
        if (item == null) return NotFound();
        item.name_ru = dto.nameRu; item.name_uz = dto.nameUz; item.name_uzl = dto.nameUzl;
        db.SpCountries.Update(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpDelete("countries/{id:long}")]
    public async Task<IActionResult> DeleteCountry(long id)
    {
        var item = await db.SpCountries.FindAsync(id);
        if (item == null) return NotFound();
        db.SpCountries.Remove(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return NoContent();
    }

    // ── positions ────────────────────────────────────────────────────────────

    [HttpPost("positions")]
    public async Task<IActionResult> CreatePosition([FromBody] RefSaveDto dto)
    {
        var item = new SpPosition { name_ru = dto.nameRu, name_uz = dto.nameUz, name_uzl = dto.nameUzl, s_create = DateTime.Now };
        db.SpPositions.Add(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpPut("positions/{id:long}")]
    public async Task<IActionResult> UpdatePosition(long id, [FromBody] RefSaveDto dto)
    {
        var item = await db.SpPositions.FindAsync(id);
        if (item == null) return NotFound();
        item.name_ru = dto.nameRu; item.name_uz = dto.nameUz; item.name_uzl = dto.nameUzl;
        db.SpPositions.Update(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpDelete("positions/{id:long}")]
    public async Task<IActionResult> DeletePosition(long id)
    {
        var item = await db.SpPositions.FindAsync(id);
        if (item == null) return NotFound();
        db.SpPositions.Remove(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return NoContent();
    }

    // ── nations ──────────────────────────────────────────────────────────────

    [HttpPost("nations")]
    public async Task<IActionResult> CreateNation([FromBody] RefSaveDto dto)
    {
        var item = new SpNation { code = dto.code, name_ru = dto.nameRu, name_uz = dto.nameUz, name_uzl = dto.nameUzl, s_create = DateTime.Now };
        db.SpNations.Add(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, code = item.code, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpPut("nations/{id:long}")]
    public async Task<IActionResult> UpdateNation(long id, [FromBody] RefSaveDto dto)
    {
        var item = await db.SpNations.FindAsync(id);
        if (item == null) return NotFound();
        item.code = dto.code; item.name_ru = dto.nameRu; item.name_uz = dto.nameUz; item.name_uzl = dto.nameUzl;
        db.SpNations.Update(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, code = item.code, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpDelete("nations/{id:long}")]
    public async Task<IActionResult> DeleteNation(long id)
    {
        var item = await db.SpNations.FindAsync(id);
        if (item == null) return NotFound();
        db.SpNations.Remove(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return NoContent();
    }

    // ── degrees ──────────────────────────────────────────────────────────────

    [HttpPost("degrees")]
    public async Task<IActionResult> CreateDegree([FromBody] RefSaveDto dto)
    {
        var item = new SpDegree { code = dto.code, name_ru = dto.nameRu, name_uz = dto.nameUz, name_uzl = dto.nameUzl, s_create = DateTime.Now };
        db.SpDegrees.Add(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, code = item.code, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpPut("degrees/{id:long}")]
    public async Task<IActionResult> UpdateDegree(long id, [FromBody] RefSaveDto dto)
    {
        var item = await db.SpDegrees.FindAsync(id);
        if (item == null) return NotFound();
        item.code = dto.code; item.name_ru = dto.nameRu; item.name_uz = dto.nameUz; item.name_uzl = dto.nameUzl;
        db.SpDegrees.Update(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, code = item.code, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpDelete("degrees/{id:long}")]
    public async Task<IActionResult> DeleteDegree(long id)
    {
        var item = await db.SpDegrees.FindAsync(id);
        if (item == null) return NotFound();
        db.SpDegrees.Remove(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return NoContent();
    }

    // ── sections ─────────────────────────────────────────────────────────────

    [HttpPost("sections")]
    public async Task<IActionResult> CreateSection([FromBody] RefSaveDto dto)
    {
        var item = new SpSection { code = dto.code, name_ru = dto.nameRu, name_uz = dto.nameUz, name_uzl = dto.nameUzl, s_create = DateTime.Now };
        db.SpSections.Add(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, code = item.code, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpPut("sections/{id:long}")]
    public async Task<IActionResult> UpdateSection(long id, [FromBody] RefSaveDto dto)
    {
        var item = await db.SpSections.FindAsync(id);
        if (item == null) return NotFound();
        item.code = dto.code; item.name_ru = dto.nameRu; item.name_uz = dto.nameUz; item.name_uzl = dto.nameUzl;
        db.SpSections.Update(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, code = item.code, nameRu = item.name_ru, nameUz = item.name_uz, nameUzl = item.name_uzl });
    }

    [HttpDelete("sections/{id:long}")]
    public async Task<IActionResult> DeleteSection(long id)
    {
        var item = await db.SpSections.FindAsync(id);
        if (item == null) return NotFound();
        db.SpSections.Remove(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return NoContent();
    }

    // ── camps ────────────────────────────────────────────────────────────────

    [HttpPost("camps")]
    public async Task<IActionResult> CreateCamp([FromBody] RefSaveDto dto)
    {
        var item = new TCamp { camps_name = dto.nameRu, s_create = DateTime.Now };
        db.TCamps.Add(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, nameRu = item.camps_name, nameUz = item.camps_name });
    }

    [HttpPut("camps/{id:long}")]
    public async Task<IActionResult> UpdateCamp(long id, [FromBody] RefSaveDto dto)
    {
        var item = await db.TCamps.FindAsync(id);
        if (item == null) return NotFound();
        item.camps_name = dto.nameRu;
        db.TCamps.Update(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return Ok(new { id = item.id, nameRu = item.camps_name, nameUz = item.camps_name });
    }

    [HttpDelete("camps/{id:long}")]
    public async Task<IActionResult> DeleteCamp(long id)
    {
        var item = await db.TCamps.FindAsync(id);
        if (item == null) return NotFound();
        db.TCamps.Remove(item);
        await db.SaveChangesAsync();
        await InvalidateCache();
        return NoContent();
    }
}
