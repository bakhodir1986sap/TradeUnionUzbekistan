using FluentValidation;
using KasabaKIAT.Core.DTOs.Auth;

namespace KasabaKIAT.Core.Validators;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().MaximumLength(500);
    }
}
