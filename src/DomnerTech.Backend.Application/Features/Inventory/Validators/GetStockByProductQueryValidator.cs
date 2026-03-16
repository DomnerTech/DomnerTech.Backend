using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Inventory.Validators;

/// <summary>
/// Validator for GetStockByProductQuery.
/// </summary>
public sealed class GetStockByProductQueryValidator : AbstractValidator<GetStockByProductQuery>
{
    public GetStockByProductQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
}
