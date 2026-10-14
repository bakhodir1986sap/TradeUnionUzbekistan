using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/platejnoe-poruchenie")]
[Authorize]
public class PlatejnoyePorucheniyeController(KasabaKIATContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var depBase = User.FindFirstValue("bkut_id");

        var items = await db.TbbuPlatejkas
            .Where(p => p.DEPARTMENT_CODE == depBase)
            .OrderByDescending(p => p.PLAT_DATE)
            .Select(p => new
            {
                recId = p.REC_ID,
                platDate = p.PLAT_DATE,
                platNum = p.PLAT_NUM,
                platelName = p.PLATELNAME,
                platelInn = p.PLATELINN,
                platelRaschet = p.PLATELRASCHET,
                platelBankName = p.PLATELBANKNAME,
                platelBankMfo = p.PLATELBANKMFO,
                poluchName = p.POLUCHNAME,
                poluchInn = p.POLUCHINN,
                poluchRaschet = p.POLUCHRASCHET,
                poluchBankName = p.POLUCHBANKNAME,
                poluchBankMfo = p.POLUCHBANKMFO,
                summa = p.SUMMA,
                comment = p.COMMENT,
                rukovoditel = p.RUKOVODITEL,
                glavbux = p.GLAVBUX
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await db.TbbuPlatejkas.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TbbuPlatejka platejka)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var depBase = User.FindFirstValue("bkut_id");

        platejka.USR_ID = userId;
        platejka.DEPARTMENT_CODE = depBase;
        platejka.MODIFY_DATE = DateTime.Now;

        db.TbbuPlatejkas.Add(platejka);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = platejka.REC_ID }, platejka);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] TbbuPlatejka platejka)
    {
        if (id != platejka.REC_ID) return BadRequest();
        platejka.MODIFY_DATE = DateTime.Now;
        db.Entry(platejka).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await db.TbbuPlatejkas.FindAsync(id);
        if (item == null) return NotFound();
        db.TbbuPlatejkas.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
