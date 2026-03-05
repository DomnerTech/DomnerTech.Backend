using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Holidays.Validators;

/// <summary>
/// Validator for DeleteHolidayCommand.
/// </summary>
public sealed class DeleteHolidayCommandValidator : AbstractValidator<DeleteHolidayCommand>
{
    public DeleteHolidayCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Holiday ID is required")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid holiday ID format");
    }
}
