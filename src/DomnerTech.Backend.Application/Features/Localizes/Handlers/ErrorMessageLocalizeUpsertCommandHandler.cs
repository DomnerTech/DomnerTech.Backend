using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Caching;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Enums;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Localizes.Handlers;

public sealed class ErrorMessageLocalizeUpsertCommandHandler(
    ILogger<ErrorMessageLocalizeUpsertCommandHandler> logger,
    IErrorMessageLocalizeRepo localizeRepo) : IRequestHandler<ErrorMessageLocalizeUpsertCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(ErrorMessageLocalizeUpsertCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var languageSupport = Enum.GetValues<LanguageSupportType>();
            var msg = languageSupport
                .Where(languageSupportType => request.Messages.ContainsKey(languageSupportType.ToName(true)))
                .ToDictionary(languageSupportType => languageSupportType.ToName(true),
                    languageSupportType => request.Messages[languageSupportType.ToName(true)]);

            if (msg.Count == 0) return new BaseResponse<bool>
            {
                Status = new ResponseStatus
                {
                    ErrorCode = ErrorCodes.Validation,
                    StatusCode = StatusCodes.Status400BadRequest
                }
            };

            var errorMsgExisted = await localizeRepo.GetByKeyAsync(request.Key, cancellationToken);

            if (errorMsgExisted != null)
            {
                errorMsgExisted.Messages = msg;
                errorMsgExisted.UpdatedAt = DateTime.UtcNow;
                await localizeRepo.UpdateAsync(errorMsgExisted, cancellationToken);
            }
            else
            {
                await localizeRepo.CreateAsync(new ErrorMessageLocalizeEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    Key = request.Key,
                    Messages = msg,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }, cancellationToken);
            }
            return new BaseResponse<bool>
            {
                Data = true
            };
        }
        catch (OperationCanceledException)
        {
            // Preserve cooperative cancellation semantics
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error upsert error message localize: {Error}", e.Message);
        }

        return new BaseResponse<bool>
        {
            Data = false,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}