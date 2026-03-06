using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Reports.Validators;

/// <summary>
/// Validator for GetLeaveUsageReportQuery.
/// </summary>
public sealed class GetLeaveUsageReportQueryValidator : AbstractValidator<GetLeaveUsageReportQuery>
{
    public GetLeaveUsageReportQueryValidator()
    {
        RuleFor(x => x.Year)
            .GreaterThan(2000)
            .WithMessage("Year must be greater than 2000")
            .LessThanOrEqualTo(DateTime.Now.Year + 1)
            .WithMessage("Year cannot be more than one year in the future");
    }
}
