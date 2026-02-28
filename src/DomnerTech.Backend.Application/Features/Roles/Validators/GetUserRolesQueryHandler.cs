using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Roles.Validators;

public sealed class GetUserRolesQueryHandler : AbstractValidator<GetUserRolesQuery>
{
    public GetUserRolesQueryHandler()
    {
        RuleFor(i => i.UserId)
            .NotNull()
            .NotEmpty()
            .WithMessage("UserId is required.");
    }
}