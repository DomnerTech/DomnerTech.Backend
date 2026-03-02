using DomnerTech.Backend.Application.Errors;
using FluentValidation;

namespace DomnerTech.Backend.Application.Features.Localizes.Validators;

public sealed class ErrorMessageLocalizeUpsertCommandValidator : AbstractValidator<ErrorMessageLocalizeUpsertCommand>
{
    public ErrorMessageLocalizeUpsertCommandValidator()
    {
        RuleFor(i => i.Key)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Localize.ErrorMessage.KeyRequired);
        RuleFor(i => i.Messages)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Localize.ErrorMessage.MessagesRequired)
            .Must(messages => messages.Keys.All(lang => !string.IsNullOrWhiteSpace(lang)))
            .WithErrorCode(ErrorCodes.Localize.ErrorMessage.MessagesLangRequired)
            .Must(messages => messages.Values.All(msg => !string.IsNullOrWhiteSpace(msg)))
            .WithErrorCode(ErrorCodes.Localize.ErrorMessage.MessagesValueRequired);
    }
}