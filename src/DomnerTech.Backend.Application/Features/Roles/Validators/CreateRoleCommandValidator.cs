using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Roles.Validators;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(i => i.Name)
            .NotEmpty()
            .MaximumLength(250)
            .WithMessage("Role name cannot be greater than 250 chars");
    }
}