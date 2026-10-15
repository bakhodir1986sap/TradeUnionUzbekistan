using FluentAssertions;
using KasabaKIAT.Core.DTOs.Users;
using KasabaKIAT.Core.Models;
using KasabaKIAT.Core.Services;
using KasabaKIAT.Core.UnitTests.Infrastructure;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace KasabaKIAT.Core.UnitTests.Services;

public class UserServiceTests
{
    private static UserService CreateSut(out Data.KasabaKIATContext db)
    {
        db = InMemoryDbContextFactory.Create();
        return new UserService(db, Substitute.For<IAuditService>(), Substitute.For<ILogger<UserService>>());
    }

    // ── GetUsersAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetUsersAsync_ReturnsPagedResult_WithCorrectTotalAndItems()
    {
        var sut = CreateSut(out var db);
        db.Users.AddRange(
            new User { id = 1, user_name = "alice" },
            new User { id = 2, user_name = "bob" },
            new User { id = 3, user_name = "carol" });
        await db.SaveChangesAsync();

        var result = await sut.GetUsersAsync(page: 1, pageSize: 2, search: null);

        result.Total.Should().Be(3);
        result.Items.Should().HaveCount(2);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(2);
    }

    [Fact]
    public async Task GetUsersAsync_SecondPage_ReturnsCorrectSlice()
    {
        var sut = CreateSut(out var db);
        for (int i = 1; i <= 5; i++)
            db.Users.Add(new User { id = i, user_name = $"user{i}" });
        await db.SaveChangesAsync();

        var result = await sut.GetUsersAsync(page: 2, pageSize: 2, search: null);

        result.Items.Should().HaveCount(2);
        result.Items.Select(u => u.Id).Should().BeEquivalentTo(new long[] { 3, 4 });
    }

    [Fact]
    public async Task GetUsersAsync_WithSearch_FiltersOnUserNameAndEmail()
    {
        var sut = CreateSut(out var db);
        db.Users.AddRange(
            new User { id = 1, user_name = "alice", email = "a@x.com" },
            new User { id = 2, user_name = "bob", email = "b@x.com" },
            new User { id = 3, user_name = "charlie", email = "alice@y.com" });
        await db.SaveChangesAsync();

        var result = await sut.GetUsersAsync(1, 10, search: "alice");

        result.Total.Should().Be(2);
        result.Items.Should().NotContain(u => u.UserName == "bob");
    }

    // ── CreateAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_StoresHashedPassword_NotPlaintext()
    {
        var sut = CreateSut(out var db);

        var dto = await sut.CreateAsync(new CreateUserRequest { Login = "newuser", Password = "plaintext123" });

        var stored = await db.Users.FindAsync(dto.Id);
        stored!.password.Should().NotBe("plaintext123");
        BCrypt.Net.BCrypt.Verify("plaintext123", stored.password).Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_SetsDefaultStateAndFlags()
    {
        var sut = CreateSut(out var db);

        var dto = await sut.CreateAsync(new CreateUserRequest { Login = "u", Password = "Pass12345" });

        var stored = await db.Users.FindAsync(dto.Id);
        stored!.state.Should().Be("1");
        stored.account_locked.Should().BeFalse();
        stored.account_expired.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_CallsAuditLogWithUserCreateAction()
    {
        var db = InMemoryDbContextFactory.Create();
        var audit = Substitute.For<IAuditService>();
        var sut = new UserService(db, audit, Substitute.For<ILogger<UserService>>());

        var dto = await sut.CreateAsync(new CreateUserRequest { Login = "audituser", Password = "Passw0rd!" });

        await audit.Received(1).LogAsync(dto.Id, "USER_CREATE", Arg.Any<string?>(), Arg.Any<CancellationToken>());
    }

    // ── UpdateAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_PartialUpdate_OnlyChangesSuppliedFields()
    {
        var sut = CreateSut(out var db);
        db.Users.Add(new User { id = 7, user_name = "original", email = "orig@x.com" });
        await db.SaveChangesAsync();

        var result = await sut.UpdateAsync(7, new UpdateUserRequest { Email = "new@x.com" });

        result.Should().NotBeNull();
        result!.Email.Should().Be("new@x.com");
        result.UserName.Should().Be("original");
    }

    [Fact]
    public async Task UpdateAsync_NonExistentUser_ReturnsNull()
    {
        var sut = CreateSut(out _);

        var result = await sut.UpdateAsync(999, new UpdateUserRequest { Email = "x@x.com" });

        result.Should().BeNull();
    }

    // ── DeactivateAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task DeactivateAsync_ExistingUser_SetsStateToZero()
    {
        var sut = CreateSut(out var db);
        db.Users.Add(new User { id = 42, user_name = "victim", state = "1" });
        await db.SaveChangesAsync();

        var result = await sut.DeactivateAsync(42);

        result.Should().BeTrue();
        var user = await db.Users.FindAsync(42L);
        user!.state.Should().Be("0");
    }

    [Fact]
    public async Task DeactivateAsync_NonExistentUser_ReturnsFalse()
    {
        var sut = CreateSut(out _);

        var result = await sut.DeactivateAsync(999);

        result.Should().BeFalse();
    }
}
