using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KasabaKIAT.Core.Controllers;

[ApiController]
[Route("api/grafik-zaezdov")]
[Authorize]
public class GrafikZaezdovController(KasabaKIATContext db) : ControllerBase
{
    // GET /api/grafik-zaezdov?sanatoriumId=1&placeTypeId=2
    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int? sanatoriumId = null,
        [FromQuery] long? campId = null,
        [FromQuery] long? placeTypeId = null)
    {
        if (sanatoriumId.HasValue)
        {
            var sanItems = await db.TbptZaezdis
                .Where(e => e.SANATORY_ID == sanatoriumId
                    && (!placeTypeId.HasValue || e.PLACE_TYPE_ID == placeTypeId))
                .Join(db.TblSpravSans, e => e.SANATORY_ID, s => s.ID, (e, s) => new
                {
                    id = e.REC_ID,
                    zaezdBegin = e.ZAEZD_BEGIN,
                    zaezdEnd = e.ZAEZD_END,
                    sanatoriumName = s.Name_San,
                    sanatoriumId = s.ID,
                    placeTypeId = e.PLACE_TYPE_ID,
                    price = e.PRICE,
                    quantity = e.QUANTITY
                })
                .ToListAsync();
            return Ok(sanItems);
        }

        if (campId.HasValue)
        {
            var campItems = await db.TbptZaezdis
                .Where(e => e.CAMP_ID == campId
                    && (!placeTypeId.HasValue || e.PLACE_TYPE_ID == placeTypeId))
                .Join(db.TCamps, e => e.CAMP_ID, c => c.id, (e, c) => new
                {
                    id = e.REC_ID,
                    zaezdBegin = e.ZAEZD_BEGIN,
                    zaezdEnd = e.ZAEZD_END,
                    campName = c.camps_name,
                    campId = c.id,
                    placeTypeId = e.PLACE_TYPE_ID,
                    price = e.PRICE,
                    quantity = e.QUANTITY
                })
                .ToListAsync();
            return Ok(campItems);
        }

        var all = await db.TbptZaezdis
            .Select(e => new
            {
                id = e.REC_ID,
                zaezdBegin = e.ZAEZD_BEGIN,
                zaezdEnd = e.ZAEZD_END,
                sanatoriumId = e.SANATORY_ID,
                campId = e.CAMP_ID,
                placeTypeId = e.PLACE_TYPE_ID,
                price = e.PRICE,
                quantity = e.QUANTITY
            })
            .ToListAsync();
        return Ok(all);
    }

    // POST /api/grafik-zaezdov
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GrafikZaezdovRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        long.TryParse(userIdClaim, out var userId);

        var entry = new TbptZaezdi
        {
            USER_ID = userId,
            ZAEZD_BEGIN = request.ZaezdBegin,
            ZAEZD_END = request.ZaezdEnd,
            PLACE_TYPE_ID = request.PlaceTypeId,
            PRICE = request.Price,
            QUANTITY = request.Quantity,
            SANATORY_ID = request.SanatoriumId,
            CAMP_ID = request.CampId
        };
        db.TbptZaezdis.Add(entry);
        await db.SaveChangesAsync();
        return Ok(new { id = entry.REC_ID });
    }

    // PUT /api/grafik-zaezdov/{id}
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] GrafikZaezdovRequest request)
    {
        var entry = await db.TbptZaezdis.FindAsync(id);
        if (entry == null) return NotFound();

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        long.TryParse(userIdClaim, out var userId);

        entry.USER_ID = userId;
        entry.ZAEZD_BEGIN = request.ZaezdBegin;
        entry.ZAEZD_END = request.ZaezdEnd;
        entry.PLACE_TYPE_ID = request.PlaceTypeId;
        entry.PRICE = request.Price;
        entry.QUANTITY = request.Quantity;
        entry.SANATORY_ID = request.SanatoriumId;
        entry.CAMP_ID = request.CampId;

        await db.SaveChangesAsync();
        return Ok(new { id = entry.REC_ID });
    }

    // DELETE /api/grafik-zaezdov/{id}
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var entry = await db.TbptZaezdis.FindAsync(id);
        if (entry == null) return NotFound();
        db.TbptZaezdis.Remove(entry);
        await db.SaveChangesAsync();
        return NoContent();
    }
}

public record GrafikZaezdovRequest(
    DateTime? ZaezdBegin,
    DateTime? ZaezdEnd,
    long? PlaceTypeId,
    decimal? Price,
    int? Quantity,
    int? SanatoriumId,
    long? CampId
);
