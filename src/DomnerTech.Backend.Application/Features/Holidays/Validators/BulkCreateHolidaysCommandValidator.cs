using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Holidays.Validators;

/// <summary>
/// Validator for BulkCreateHolidaysCommand.
/// </summary>
public sealed class BulkCreateHolidaysCommandValidator : AbstractValidator<BulkCreateHolidaysCommand>
{
    public BulkCreateHolidaysCommandValidator()
    {
        RuleFor(x => x.Dto.Holidays)
            .NotEmpty()
            .WithMessage("At least one holiday must be provided")
            .Must(h => h.Count <= 100)
            .WithMessage("Maximum 100 holidays can be created at once");

        RuleForEach(x => x.Dto.Holidays).ChildRules(holiday =>
        {
            holiday.RuleFor(x => x.Name)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Leave.HolidayNameReq)
                .MaximumLength(100);

            holiday.RuleFor(x => x.Date)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Leave.HolidayDateReq);
        });
    }
}
