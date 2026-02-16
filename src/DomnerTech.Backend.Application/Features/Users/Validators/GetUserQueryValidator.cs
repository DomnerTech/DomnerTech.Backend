using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Users.Validators;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(i => i.UserId)
            .NotNull()
            .NotEmpty()
            .WithMessage("UserId is required");
    }
}