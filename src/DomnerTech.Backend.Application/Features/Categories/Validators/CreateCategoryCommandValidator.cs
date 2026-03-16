using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Categories.Validators;

/// <summary>
/// Validator for CreateCategoryCommand.
/// </summary>
public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Category name is required")
            .Must(name => name.ContainsKey("en") && !string.IsNullOrWhiteSpace(name["en"]))
            .WithMessage("Category name in English is required");

        RuleFor(x => x.Dto.Slug)
            .NotEmpty()
            .WithMessage("Slug is required")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug must be lowercase alphanumeric with hyphens only");

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Display order must be greater than or equal to 0");
    }
}
