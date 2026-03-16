using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Warehouses.Validators;

/// <summary>
/// Validator for CreateWarehouseCommand.
/// </summary>
public sealed class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithMessage("Warehouse name is required")
            .MaximumLength(200)
            .WithMessage("Warehouse name cannot exceed 200 characters");

        RuleFor(x => x.Dto.Code)
            .NotEmpty()
            .WithMessage("Warehouse code is required")
            .Matches("^[A-Z0-9-]+$")
            .WithMessage("Warehouse code must be uppercase alphanumeric with hyphens")
            .MaximumLength(50)
            .WithMessage("Warehouse code cannot exceed 50 characters");

        RuleFor(x => x.Dto.Type)
            .NotEmpty()
            .WithMessage("Warehouse type is required");

        RuleFor(x => x.Dto.Address)
            .NotEmpty()
            .WithMessage("Address is required");

        RuleFor(x => x.Dto.City)
            .NotEmpty()
            .WithMessage("City is required");

        RuleFor(x => x.Dto.Country)
            .NotEmpty()
            .WithMessage("Country is required");

        RuleFor(x => x.Dto.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.Email))
            .WithMessage("Invalid email address");
    }
}
