using FluentAssertions;
using KasabaKIAT.Core.Models;
using KasabaKIAT.Core.Services;
using KasabaKIAT.Core.UnitTests.Infrastructure;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace KasabaKIAT.Core.UnitTests.Services;

public class RoleServiceTests
{
    private static RoleService CreateSut(out Data.KasabaKIATContext db)
    {
        db = InMemoryDbContextFactory.Create();
        return new RoleService(db, Substitute.For<IAuditService>(), Substitute.For<ILogger<RoleService>>());
    }

    [Fact]
    public async Task CreateAsync_PersistsRoleWithStatusOne()
    {
        var sut = CreateSut(out var db);

        var dto = await sut.CreateAsync("Accountant", actorUserId: 1);

        dto.Id.Should().BeGreaterThan(0);
        dto.Name.Should().Be("Accountant");
        var stored = await db.Roles.FindAsync(dto.Id);
        stored!.s_status.Should().Be("1");
    }

    [Fact]
    public async Task UpdateAsync_ExistingRole_ChangesName()
    {
        var sut = CreateSut(out var db);
        db.Roles.Add(new Role { id = 5, name = "OldName", version = 0 });
        await db.SaveChangesAsync();

        var dto = await sut.UpdateAsync(5, "NewName", actorUserId: 1);

        dto!.Name.Should().Be("NewName");
        (await db.Roles.FindAsync(5L))!.name.Should().Be("NewName");
    }

    [Fact]
    public async Task UpdateAsync_NonExistentRole_ReturnsNull()
    {
        var sut = CreateSut(out _);

        var result = await sut.UpdateAsync(999, "X", actorUserId: 1);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ExistingRole_RemovesFromDb()
    {
        var sut = CreateSut(out var db);
        db.Roles.Add(new Role { id = 3, name = "ToDelete", version = 0 });
        await db.SaveChangesAsync();

        var result = await sut.DeleteAsync(3, actorUserId: 1);

        result.Should().BeTrue();
        (await db.Roles.FindAsync(3L)).Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_NonExistentRole_ReturnsFalse()
    {
        var sut = CreateSut(out _);

        var result = await sut.DeleteAsync(999, actorUserId: 1);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllAsync_ProjectsPermissionsCorrectly()
    {
        var sut = CreateSut(out var db);
        var func = new Functionality { id = 1, name = "MANAGE_USERS" };
        var role = new Role { id = 10, name = "Admin", version = 0 };
        role.role_permissions.Add(new RolePermission
        {
            id = 1, role_id = 10, functionality_id = 1, functionality = func, action = "READ"
        });
        db.Functionalities.Add(func);
        db.Roles.Add(role);
        await db.SaveChangesAsync();

        var roles = await sut.GetAllAsync();

        roles.Should().ContainSingle(r => r.Id == 10);
        roles[0].Permissions.Should().ContainSingle(p =>
            p.FunctionalityName == "MANAGE_USERS" && p.Action == "READ");
    }
}
