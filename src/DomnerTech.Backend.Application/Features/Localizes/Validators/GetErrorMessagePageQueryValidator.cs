using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Localizes.Validators;

public class GetErrorMessagePageQueryValidator : AbstractValidator<GetErrorMessagePageQuery>
{
    public GetErrorMessagePageQueryValidator()
    {
        RuleFor(i => i.PageSize)
            .LessThanOrEqualTo(100)
            .WithErrorCode(ErrorCodes.MaxPageSize);
    }
}