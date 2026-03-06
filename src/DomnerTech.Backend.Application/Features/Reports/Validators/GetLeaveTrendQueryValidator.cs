using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Reports.Validators;

/// <summary>
/// Validator for GetLeaveTrendQuery.
/// </summary>
public sealed class GetLeaveTrendQueryValidator : AbstractValidator<GetLeaveTrendQuery>
{
    public GetLeaveTrendQueryValidator()
    {
        RuleFor(x => x.Year)
            .GreaterThan(2000)
            .WithMessage("Year must be greater than 2000")
            .LessThanOrEqualTo(DateTime.Now.Year + 1)
            .WithMessage("Year cannot be more than one year in the future");
    }
}
