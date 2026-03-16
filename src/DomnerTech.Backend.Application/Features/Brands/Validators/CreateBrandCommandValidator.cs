using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Brands.Validators;

/// <summary>
/// Validator for CreateBrandCommand.
/// </summary>
public sealed class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithMessage("Brand name is required")
            .MaximumLength(200)
            .WithMessage("Brand name cannot exceed 200 characters");

        RuleFor(x => x.Dto.Slug)
            .NotEmpty()
            .WithMessage("Slug is required")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug must be lowercase alphanumeric with hyphens only")
            .MaximumLength(200)
            .WithMessage("Slug cannot exceed 200 characters");

        RuleFor(x => x.Dto.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Display order must be greater than or equal to 0");

        RuleFor(x => x.Dto.WebsiteUrl)
            .Must(url => string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.WebsiteUrl))
            .WithMessage("Website URL must be a valid URL");

        RuleFor(x => x.Dto.LogoUrl)
            .Must(url => string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.LogoUrl))
            .WithMessage("Logo URL must be a valid URL");
    }
}
