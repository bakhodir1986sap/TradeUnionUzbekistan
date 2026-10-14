using FluentValidation;
using KasabaKIAT.Core.DTOs.Users;

namespace KasabaKIAT.Core.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Login).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.RegionId).MaximumLength(50).When(x => x.RegionId != null);
        RuleFor(x => x.Language).MaximumLength(10).When(x => x.Language != null);
        RuleFor(x => x.UserType).MaximumLength(50).When(x => x.UserType != null);
    }
}
