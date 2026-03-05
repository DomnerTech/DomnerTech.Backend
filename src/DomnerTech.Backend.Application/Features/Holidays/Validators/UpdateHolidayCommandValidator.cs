using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Holidays.Validators;

/// <summary>
/// Validator for UpdateHolidayCommand.
/// </summary>
public sealed class UpdateHolidayCommandValidator : AbstractValidator<UpdateHolidayCommand>
{
    public UpdateHolidayCommandValidator()
    {
        RuleFor(x => x.Dto.Id)
            .NotEmpty()
            .WithMessage("Holiday ID is required")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid holiday ID format");

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
