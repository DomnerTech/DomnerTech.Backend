using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Products.Validators;

/// <summary>
/// Validator for UpdateProductCommand.
/// </summary>
public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Dto.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product name is required")
            .Must(name => name.ContainsKey("en") && !string.IsNullOrWhiteSpace(name["en"]))
            .WithMessage("Product name in English is required");

        RuleFor(x => x.Dto.CategoryId)
            .NotEmpty()
            .WithMessage("Category ID is required");

        RuleFor(x => x.Dto.Prices)
            .NotNull()
            .NotEmpty()
            .WithMessage("At least one price is required")
            .Must(prices => prices.Any(p => p.PriceType == Domain.Enums.PriceType.Retail))
            .WithMessage("Retail price is required");

        RuleForEach(x => x.Dto.Prices)
            .ChildRules(price =>
            {
                price.RuleFor(p => p.Amount)
                    .GreaterThan(0)
                    .WithMessage("Price amount must be greater than 0");
            });

        RuleFor(x => x.Dto.CostPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Dto.CostPrice.HasValue)
            .WithMessage("Cost price must be greater than or equal to 0");

        RuleFor(x => x.Dto.TaxRate)
            .InclusiveBetween(0, 100)
            .When(x => x.Dto.TaxRate.HasValue)
            .WithMessage("Tax rate must be between 0 and 100");
    }
}
