using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveTypes.Handlers;

/// <summary>
/// Handler for deleting a leave type.
/// </summary>
public sealed class DeleteLeaveTypeCommandHandler(
    ILogger<DeleteLeaveTypeCommandHandler> logger,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<DeleteLeaveTypeCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var leaveTypeId = ObjectId.Parse(request.Id);

            var existing = await leaveTypeRepo.GetByIdAsync(leaveTypeId, cancellationToken);
            if (existing is null)
            {
                throw new NotFoundException("Leave type not found");
            }

            await leaveTypeRepo.DeleteAsync(leaveTypeId, cancellationToken);

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
            logger.LogError(e, "Error deleting leave type: {Error}", e.Message);
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
