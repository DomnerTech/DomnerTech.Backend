using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Holidays.Handlers;

/// <summary>
/// Handler for deleting a holiday.
/// </summary>
public sealed class DeleteHolidayCommandHandler(
    ILogger<DeleteHolidayCommandHandler> logger,
    IHolidayRepo holidayRepo) : IRequestHandler<DeleteHolidayCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var holidayId = ObjectId.Parse(request.Id);

            var existing = await holidayRepo.GetByIdAsync(holidayId, cancellationToken);
            if (existing is null)
            {
                throw new NotFoundException("Holiday not found");
            }

            await holidayRepo.DeleteAsync(holidayId, cancellationToken);

            return new BaseResponse<bool>
            {
                Data = true
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting holiday: {Error}", e.Message);
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
