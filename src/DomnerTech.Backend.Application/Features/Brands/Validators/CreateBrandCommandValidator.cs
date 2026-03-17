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

        RuleFor(x => x.Dto.LogoUrl)
            .Must(url => string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .When(x => !string.IsNullOrWhiteSpace(x.Dto.LogoUrl))
            .WithMessage("Logo URL must be a valid URL");
    }
}
