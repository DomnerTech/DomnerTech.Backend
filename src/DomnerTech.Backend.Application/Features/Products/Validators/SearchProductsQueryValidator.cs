using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Products.Validators;

/// <summary>
/// Validator for SearchProductsQuery.
/// </summary>
public sealed class SearchProductsQueryValidator : AbstractValidator<SearchProductsQuery>
{
    public SearchProductsQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .NotEmpty()
            .WithMessage("Search term is required")
            .MinimumLength(2)
            .WithMessage("Search term must be at least 2 characters");
    }
}
