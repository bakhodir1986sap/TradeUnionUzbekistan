using FluentAssertions;
using KasabaKIAT.Core.DTOs.Putyovki;
using KasabaKIAT.Core.Models;
using KasabaKIAT.Core.Services;
using KasabaKIAT.Core.UnitTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace KasabaKIAT.Core.UnitTests.Services;

public class PutyovkiServiceTests
{
    private static PutyovkiService CreateSut(out Data.KasabaKIATContext db)
    {
        db = InMemoryDbContextFactory.Create();
        return new PutyovkiService(db, Substitute.For<IAuditService>(), Substitute.For<ILogger<PutyovkiService>>());
    }

    // ── CreateAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_SetsQuantityRemainEqualToQuantityIn()
    {
        var sut = CreateSut(out _);
        var request = new CreatePutyovkaRequest { QuantityIn = 50, Price = 100m };

        var dto = await sut.CreateAsync(request, userId: 1, departmentCode: "DEPT01");

        dto.QuantityRemain.Should().Be(50);
        dto.QuantityIn.Should().Be(50);
        dto.QuantityOut.Should().Be(0);
    }

    [Fact]
    public async Task CreateAsync_SetsRemainBalToQuantityInTimesPrice()
    {
        var sut = CreateSut(out _);
        var request = new CreatePutyovkaRequest { QuantityIn = 10, Price = 250m };

        var dto = await sut.CreateAsync(request, userId: 1, departmentCode: "DEPT01");

        dto.RemainBal.Should().Be(2500m); // 10 * 250
    }

    [Fact]
    public async Task CreateAsync_SetsStatusTo1()
    {
        var sut = CreateSut(out _);

        var dto = await sut.CreateAsync(new CreatePutyovkaRequest(), userId: 1, departmentCode: "DEPT01");

        dto.Status.Should().Be("1");
    }

    [Fact]
    public async Task CreateAsync_NullQuantityAndPrice_DefaultsToZeroBalance()
    {
        var sut = CreateSut(out _);

        var dto = await sut.CreateAsync(
            new CreatePutyovkaRequest { QuantityIn = null, Price = null },
            userId: 1, departmentCode: "DEPT01");

        dto.QuantityRemain.Should().Be(0);
        dto.RemainBal.Should().Be(0m);
    }

    // ── UpdateAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_RecalculatesQuantityRemainAsInMinusOut()
    {
        var sut = CreateSut(out var db);
        db.TbptPutyovkis.Add(new TbptPutyovki
        {
            REC_ID = 1, QUANTITY_IN = 100, QUANTITY_OUT = 30, PRICE = 50m, STATUS = "1"
        });
        await db.SaveChangesAsync();

        // Update QuantityIn to 80; OUT stays 30 → REMAIN = 50, BAL = 50 * 50 = 2500
        var dto = await sut.UpdateAsync(1, new CreatePutyovkaRequest { QuantityIn = 80 }, actorUserId: 1);

        dto!.QuantityRemain.Should().Be(50);
        dto.RemainBal.Should().Be(2500m);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentRecord_ReturnsNull()
    {
        var sut = CreateSut(out _);

        var result = await sut.UpdateAsync(999, new CreatePutyovkaRequest(), actorUserId: 1);

        result.Should().BeNull();
    }

    // ── DeleteAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_ExistingRecord_SetsStatusToZero()
    {
        var sut = CreateSut(out var db);
        db.TbptPutyovkis.Add(new TbptPutyovki { REC_ID = 10, STATUS = "1" });
        await db.SaveChangesAsync();

        var result = await sut.DeleteAsync(10, actorUserId: 1);

        result.Should().BeTrue();

        // Bypass query filter (STATUS != "0") to see the soft-deleted row
        var entity = await db.TbptPutyovkis.IgnoreQueryFilters()
                              .FirstOrDefaultAsync(p => p.REC_ID == 10);
        entity!.STATUS.Should().Be("0");
    }

    [Fact]
    public async Task DeleteAsync_NonExistentRecord_ReturnsFalse()
    {
        var sut = CreateSut(out _);

        var result = await sut.DeleteAsync(999, actorUserId: 1);

        result.Should().BeFalse();
    }
}
