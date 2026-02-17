using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Auth.Validators;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(i => i.Username)
            .NotEmpty()
            .NotNull()
            .WithMessage("Username is required.");
        RuleFor(i => i.Pwd)
            .NotEmpty()
            .NotNull()
            .WithMessage("Password is required.");
    }
}