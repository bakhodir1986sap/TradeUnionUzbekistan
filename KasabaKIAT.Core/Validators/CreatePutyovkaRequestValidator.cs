using FluentValidation;
using KasabaKIAT.Core.DTOs.Putyovki;

namespace KasabaKIAT.Core.Validators;

public class CreatePutyovkaRequestValidator : AbstractValidator<CreatePutyovkaRequest>
{
    public CreatePutyovkaRequestValidator()
    {
        RuleFor(x => x.DepartmentCode).MaximumLength(50).When(x => x.DepartmentCode != null);
        RuleFor(x => x.Price).InclusiveBetween(0, 10_000_000).When(x => x.Price.HasValue);
        RuleFor(x => x.QuantityIn).InclusiveBetween(0, 10_000).When(x => x.QuantityIn.HasValue);
        RuleFor(x => x.Year).InclusiveBetween(2000, 2100).When(x => x.Year.HasValue);
        RuleFor(x => x.RestEndDate).GreaterThanOrEqualTo(x => x.RestBeginDate)
            .When(x => x.RestBeginDate.HasValue && x.RestEndDate.HasValue)
            .WithMessage("RestEndDate must be on or after RestBeginDate");
    }
}
