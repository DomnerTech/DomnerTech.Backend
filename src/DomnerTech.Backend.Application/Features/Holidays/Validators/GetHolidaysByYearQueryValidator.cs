using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Holidays.Validators;

/// <summary>
/// Validator for GetHolidaysByYearQuery.
/// </summary>
public sealed class GetHolidaysByYearQueryValidator : AbstractValidator<GetHolidaysByYearQuery>
{
    public GetHolidaysByYearQueryValidator()
    {
        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(2000)
            .WithMessage("Year must be 2000 or later")
            .LessThanOrEqualTo(2100)
            .WithMessage("Year must be 2100 or earlier");
    }
}
