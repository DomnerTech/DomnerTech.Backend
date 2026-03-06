using DomnerTech.Backend.Application.Errors;
using FluentValidation;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Notifications.Validators;

/// <summary>
/// Validator for MarkNotificationAsReadCommand.
/// </summary>
public sealed class MarkNotificationAsReadCommandValidator : AbstractValidator<MarkNotificationAsReadCommand>
{
    public MarkNotificationAsReadCommandValidator()
    {
        RuleFor(x => x.NotificationId)
            .NotEmpty()
            .WithMessage("Notification ID is required")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid notification ID format");
    }
}
