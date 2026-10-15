using FluentAssertions;
using KasabaKIAT.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace KasabaKIAT.Core.UnitTests.Exceptions;

public class GlobalExceptionHandlerTests
{
    private static GlobalExceptionHandler CreateSut()
        => new(Substitute.For<ILogger<GlobalExceptionHandler>>());

    private static DefaultHttpContext CreateHttpContext()
    {
        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();
        return ctx;
    }

    [Theory]
    [InlineData(typeof(NotFoundException), 404)]
    [InlineData(typeof(ConflictException), 409)]
    [InlineData(typeof(UnauthorizedAccessException), 403)]
    public async Task KnownExceptions_MapToCorrectStatusCode(Type exceptionType, int expectedStatus)
    {
        var sut = CreateSut();
        var ctx = CreateHttpContext();
        var ex = (Exception)Activator.CreateInstance(exceptionType, "test")!;

        var handled = await sut.TryHandleAsync(ctx, ex, CancellationToken.None);

        handled.Should().BeTrue();
        ctx.Response.StatusCode.Should().Be(expectedStatus);
    }

    [Fact]
    public async Task UnknownException_MapsTo500()
    {
        var sut = CreateSut();
        var ctx = CreateHttpContext();

        var handled = await sut.TryHandleAsync(ctx, new InvalidOperationException("boom"), CancellationToken.None);

        handled.Should().BeTrue();
        ctx.Response.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task TryHandleAsync_AlwaysReturnsTrue()
    {
        var sut = CreateSut();
        var ctx = CreateHttpContext();

        var result = await sut.TryHandleAsync(ctx, new Exception("any"), CancellationToken.None);

        result.Should().BeTrue();
    }
}
