using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Users.Validators;

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersQueryValidator()
    {
        RuleFor(i => i.PageSize)
            .LessThanOrEqualTo(100)
            .WithErrorCode(ErrorCodes.MaxPageSize);
    }
}