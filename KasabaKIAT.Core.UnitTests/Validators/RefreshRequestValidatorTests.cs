using FluentAssertions;
using KasabaKIAT.Core.DTOs.Auth;
using KasabaKIAT.Core.Validators;
using Xunit;

namespace KasabaKIAT.Core.UnitTests.Validators;

public class RefreshRequestValidatorTests
{
    private readonly RefreshRequestValidator _validator = new();

    [Fact]
    public void Valid_Token_PassesValidation()
    {
        _validator.Validate(new RefreshRequest { RefreshToken = "valid-token" }).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Empty_Token_FailsValidation()
    {
        _validator.Validate(new RefreshRequest { RefreshToken = "" }).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Token_Over500Chars_FailsValidation()
    {
        var result = _validator.Validate(new RefreshRequest { RefreshToken = new string('t', 501) });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "RefreshToken");
    }

    [Fact]
    public void Token_At500Chars_PassesValidation()
    {
        _validator.Validate(new RefreshRequest { RefreshToken = new string('t', 500) }).IsValid.Should().BeTrue();
    }
}
