using FluentAssertions;
using KasabaKIAT.Core.Services;
using KasabaKIAT.Core.UnitTests.Infrastructure;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace KasabaKIAT.Core.UnitTests.Services;

public class AuditServiceTests
{
    [Fact]
    public async Task LogAsync_WritesUserActionToDatabase()
    {
        var db = InMemoryDbContextFactory.Create();
        var sut = new AuditService(db, Substitute.For<ILogger<AuditService>>());

        await sut.LogAsync(userId: 42, action: "TEST_ACTION", description: "desc");

        db.UserActions.Should().ContainSingle(a =>
            a.user_id == 42 &&
            a.action_name == "TEST_ACTION" &&
            a.description == "desc");
    }

    [Fact]
    public async Task LogAsync_WhenDbIsDisposed_DoesNotPropagateException()
    {
        // Disposing the context forces SaveChangesAsync to throw ObjectDisposedException.
        // AuditService must swallow it — audit failures must never crash the caller.
        var db = InMemoryDbContextFactory.Create();
        await db.DisposeAsync();

        var sut = new AuditService(db, Substitute.For<ILogger<AuditService>>());

        var act = () => sut.LogAsync(1, "ANY_ACTION");
        await act.Should().NotThrowAsync();
    }
}
