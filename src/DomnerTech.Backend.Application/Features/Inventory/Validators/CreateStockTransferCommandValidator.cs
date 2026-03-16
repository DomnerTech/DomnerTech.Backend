using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Inventory.Validators;

/// <summary>
/// Validator for CreateStockTransferCommand.
/// </summary>
public sealed class CreateStockTransferCommandValidator : AbstractValidator<CreateStockTransferCommand>
{
    public CreateStockTransferCommandValidator()
    {
        RuleFor(x => x.Dto.FromWarehouseId)
            .NotEmpty()
            .WithMessage("Source warehouse ID is required");

        RuleFor(x => x.Dto.ToWarehouseId)
            .NotEmpty()
            .WithMessage("Destination warehouse ID is required");

        RuleFor(x => x.Dto.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Dto.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.Dto)
            .Must(dto => dto.FromWarehouseId != dto.ToWarehouseId)
            .WithMessage("Source and destination warehouses must be different");
    }
}
