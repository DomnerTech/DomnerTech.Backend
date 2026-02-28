using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Roles.Validators;

public sealed class UpsertUserRoleCommandValidator : AbstractValidator<UpsertUserRoleCommand>
{
    public UpsertUserRoleCommandValidator()
    {
        RuleFor(i => i.UserId)
            .NotNull()
            .NotEmpty()
            .WithMessage("UserId is required.");
        RuleFor(i => i.RoleName)
            .NotNull()
            .NotEmpty()
            .WithMessage("RoleName is required.");
    }
}