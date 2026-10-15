using FluentAssertions;
using KasabaKIAT.Core.DTOs.Auth;
using KasabaKIAT.Core.Validators;
using Xunit;

namespace KasabaKIAT.Core.UnitTests.Validators;

public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator = new();

    [Fact]
    public void Valid_LoginAndPassword_PassesValidation()
    {
        var result = _validator.Validate(new LoginRequest { Login = "user1", Password = "secret" });
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "secret")]
    [InlineData("user1", "")]
    [InlineData("", "")]
    public void Empty_LoginOrPassword_FailsValidation(string login, string password)
    {
        var result = _validator.Validate(new LoginRequest { Login = login, Password = password });
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Login_ExceedingMaxLength_FailsValidation()
    {
        var result = _validator.Validate(new LoginRequest { Login = new string('a', 101), Password = "x" });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Login");
    }

    [Fact]
    public void Password_ExceedingMaxLength_FailsValidation()
    {
        var result = _validator.Validate(new LoginRequest { Login = "u", Password = new string('x', 101) });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Password");
    }

    [Fact]
    public void Login_AtExactMaxLength_PassesValidation()
    {
        var result = _validator.Validate(new LoginRequest { Login = new string('a', 100), Password = "x" });
        result.IsValid.Should().BeTrue();
    }
}
