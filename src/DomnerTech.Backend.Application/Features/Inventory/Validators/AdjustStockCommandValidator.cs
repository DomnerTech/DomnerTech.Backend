using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Inventory.Validators;

/// <summary>
/// Validator for AdjustStockCommand.
/// </summary>
public sealed class AdjustStockCommandValidator : AbstractValidator<AdjustStockCommand>
{
    public AdjustStockCommandValidator()
    {
        RuleFor(x => x.Dto.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Dto.WarehouseId)
            .NotEmpty()
            .WithMessage("Warehouse ID is required");

        RuleFor(x => x.Dto.Quantity)
            .NotEqual(0)
            .WithMessage("Quantity cannot be zero");

        RuleFor(x => x.Dto.Reason)
            .IsInEnum()
            .WithMessage("Invalid adjustment reason");
    }
}
