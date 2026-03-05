using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Holidays.Validators;

/// <summary>
/// Validator for CreateHolidayCommand.
/// </summary>
public sealed class CreateHolidayCommandValidator : AbstractValidator<CreateHolidayCommand>
{
    public CreateHolidayCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.HolidayNameReq)
            .MaximumLength(100);

        RuleFor(x => x.Dto.Date)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Leave.HolidayDateReq);

        RuleFor(x => x.Dto.Type)
            .IsInEnum()
            .WithMessage("Invalid holiday type");

        When(x => !string.IsNullOrEmpty(x.Dto.CountryCode), () =>
        {
            RuleFor(x => x.Dto.CountryCode)
                .Length(2)
                .WithMessage("Country code must be 2 characters (ISO 3166-1 alpha-2)");
        });
    }
}
