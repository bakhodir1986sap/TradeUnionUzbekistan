using FluentAssertions;
using KasabaKIAT.Core.DTOs.Putyovki;
using KasabaKIAT.Core.Validators;
using Xunit;

namespace KasabaKIAT.Core.UnitTests.Validators;

public class CreatePutyovkaRequestValidatorTests
{
    private readonly CreatePutyovkaRequestValidator _validator = new();

    [Fact]
    public void EmptyRequest_PassesValidation()
    {
        // All rules use When(field.HasValue), so a fully-null request is valid
        _validator.Validate(new CreatePutyovkaRequest()).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(10_000_001)]
    public void Price_OutOfRange_FailsValidation(int price)
    {
        var result = _validator.Validate(new CreatePutyovkaRequest { Price = price });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Price");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10_000_000)]
    public void Price_AtBoundary_PassesValidation(int price)
    {
        _validator.Validate(new CreatePutyovkaRequest { Price = price }).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(10_001)]
    public void QuantityIn_OutOfRange_FailsValidation(int qty)
    {
        var result = _validator.Validate(new CreatePutyovkaRequest { QuantityIn = qty });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "QuantityIn");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10_000)]
    public void QuantityIn_AtBoundary_PassesValidation(int qty)
    {
        _validator.Validate(new CreatePutyovkaRequest { QuantityIn = qty }).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1999)]
    [InlineData(2101)]
    public void Year_OutOfRange_FailsValidation(int year)
    {
        var result = _validator.Validate(new CreatePutyovkaRequest { Year = year });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Year");
    }

    [Theory]
    [InlineData(2000)]
    [InlineData(2100)]
    public void Year_AtBoundary_PassesValidation(int year)
    {
        _validator.Validate(new CreatePutyovkaRequest { Year = year }).IsValid.Should().BeTrue();
    }

    [Fact]
    public void RestEndDate_BeforeRestBeginDate_FailsValidation()
    {
        var req = new CreatePutyovkaRequest
        {
            RestBeginDate = new DateTime(2025, 7, 10),
            RestEndDate = new DateTime(2025, 7, 9)
        };
        var result = _validator.Validate(req);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e =>
            e.PropertyName == "RestEndDate" &&
            e.ErrorMessage == "RestEndDate must be on or after RestBeginDate");
    }

    [Fact]
    public void RestEndDate_EqualToRestBeginDate_PassesValidation()
    {
        var date = new DateTime(2025, 7, 10);
        _validator.Validate(new CreatePutyovkaRequest { RestBeginDate = date, RestEndDate = date })
                  .IsValid.Should().BeTrue();
    }

    [Fact]
    public void RestEndDate_AfterRestBeginDate_PassesValidation()
    {
        var req = new CreatePutyovkaRequest
        {
            RestBeginDate = new DateTime(2025, 7, 10),
            RestEndDate = new DateTime(2025, 7, 20)
        };
        _validator.Validate(req).IsValid.Should().BeTrue();
    }

    [Fact]
    public void RestEndDate_WhenOnlyBeginDateSupplied_DoesNotError()
    {
        _validator.Validate(new CreatePutyovkaRequest { RestBeginDate = new DateTime(2025, 7, 10) })
                  .IsValid.Should().BeTrue();
    }
}
