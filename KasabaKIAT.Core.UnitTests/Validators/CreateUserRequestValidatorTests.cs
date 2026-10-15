using FluentAssertions;
using KasabaKIAT.Core.DTOs.Users;
using KasabaKIAT.Core.Validators;
using Xunit;

namespace KasabaKIAT.Core.UnitTests.Validators;

public class CreateUserRequestValidatorTests
{
    private readonly CreateUserRequestValidator _validator = new();

    private static CreateUserRequest ValidRequest() => new()
    {
        Login = "testuser",
        Password = "SecurePass1"
    };

    [Fact]
    public void Valid_MinimalRequest_PassesValidation()
    {
        _validator.Validate(ValidRequest()).IsValid.Should().BeTrue();
    }

    [Fact]
    public void EmptyLogin_FailsValidation()
    {
        var req = ValidRequest();
        req.Login = "";
        _validator.Validate(req).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Password_ShorterThan8Chars_FailsValidation()
    {
        var req = ValidRequest();
        req.Password = "short";
        var result = _validator.Validate(req);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Password");
    }

    [Fact]
    public void Password_AtExactly8Chars_PassesValidation()
    {
        var req = ValidRequest();
        req.Password = "Pass1234";
        _validator.Validate(req).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Email_WhenSupplied_MustBeValidAddress()
    {
        var req = ValidRequest();
        req.Email = "not-an-email";
        var result = _validator.Validate(req);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Email_WhenNull_IsSkippedByValidation()
    {
        var req = ValidRequest();
        req.Email = null;
        _validator.Validate(req).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Email_WhenEmpty_IsSkippedByValidation()
    {
        var req = ValidRequest();
        req.Email = string.Empty;
        _validator.Validate(req).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Language_WhenOver10Chars_FailsValidation()
    {
        var req = ValidRequest();
        req.Language = "toolonglanguagecode";
        var result = _validator.Validate(req);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Language");
    }

    [Fact]
    public void RegionId_WhenOver50Chars_FailsValidation()
    {
        var req = ValidRequest();
        req.RegionId = new string('r', 51);
        var result = _validator.Validate(req);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "RegionId");
    }

    [Fact]
    public void UserType_WhenOver50Chars_FailsValidation()
    {
        var req = ValidRequest();
        req.UserType = new string('u', 51);
        var result = _validator.Validate(req);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "UserType");
    }
}
