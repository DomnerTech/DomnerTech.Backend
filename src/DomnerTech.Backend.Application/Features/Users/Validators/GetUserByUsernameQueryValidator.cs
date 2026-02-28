using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Users.Validators;

public sealed class GetUserByUsernameQueryValidator : AbstractValidator<GetUserByUsernameQuery>
{
    public GetUserByUsernameQueryValidator()
    {
        RuleFor(i => i.Username)
            .NotNull()
            .NotEmpty()
            .WithMessage("Username is required");
    }
}