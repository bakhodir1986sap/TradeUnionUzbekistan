using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.DTOs.Reference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/reference")]
[Authorize]
public class ReferenceController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet("sanatoriums")]
    public async Task<IActionResult> GetSanatoriums()
    {
        var items = await db.TblSpravSans
            .Select(s => new ReferenceItemDto
            {
                Code = s.ID.ToString(),
                NameRu = s.Name_San,
                NameUz = s.Name_San,
                NameUzl = s.Name_San
            })
            .ToListAsync();
        return Ok(items);
    }

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
            .Select(r => new ReferenceItemDto
            {
                Code = r.code ?? r.ID.ToString(),
                NameRu = r.name_ru,
                NameUz = r.name_uz,
                NameUzl = r.name_uzl,
                CreateDate = r.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("branches")]
    public async Task<IActionResult> GetBranches()
    {
        var items = await db.SpBranches
            .Select(b => new ReferenceItemDto
            {
                Code = b.id.ToString(),
                NameRu = b.name_ru,
                NameUz = b.name_uz,
                NameUzl = b.name_uzl,
                CreateDate = b.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries()
    {
        var items = await db.SpCountries
            .Select(c => new ReferenceItemDto
            {
                Code = c.id.ToString(),
                NameRu = c.name_ru,
                NameUz = c.name_uz,
                NameUzl = c.name_uzl,
                CreateDate = c.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("positions")]
    public async Task<IActionResult> GetPositions()
    {
        var items = await db.SpPositions
            .Select(p => new ReferenceItemDto
            {
                Code = p.id.ToString(),
                NameRu = p.name_ru,
                NameUz = p.name_uz,
                NameUzl = p.name_uzl,
                CreateDate = p.s_create
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
            .Select(c => new { id = c.id, name = c.camps_name })
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
            .Select(n => new ReferenceItemDto
            {
                Code = n.code,
                NameRu = n.name_ru,
                NameUz = n.name_uz,
                NameUzl = n.name_uzl,
                CreateDate = n.s_create
            })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("degrees")]
    public async Task<IActionResult> GetDegrees()
    {
        var items = await db.SpDegrees
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
