using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Policies.Validators;

public class CreatePolicyCommandValidator : AbstractValidator<CreatePolicyCommand>
{
    public CreatePolicyCommandValidator()
    {
        RuleFor(i => i.Name)
            .NotEmpty()
            .WithMessage("Policy name is required.")
            .MaximumLength(100)
            .WithMessage("Policy name must not exceed 100 characters.");
        RuleFor(i => i.RoleNames)
            .NotEmpty()
            .WithMessage("At least one role is required.")
            .Must(roles => roles.All(r => !string.IsNullOrWhiteSpace(r)))
            .WithMessage("Role names must not be empty.");
    }
}