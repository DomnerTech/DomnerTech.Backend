using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveTypes;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.LeaveTypes.Handlers;

/// <summary>
/// Handler for getting all active leave types.
/// </summary>
public sealed class GetAllLeaveTypesQueryHandler(
    ILogger<GetAllLeaveTypesQueryHandler> logger,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetAllLeaveTypesQuery, BaseResponse<IEnumerable<LeaveTypeDto>>>
{
    public async Task<BaseResponse<IEnumerable<LeaveTypeDto>>> Handle(GetAllLeaveTypesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await leaveTypeRepo.GetAllActiveAsync(cancellationToken);
            return new BaseResponse<IEnumerable<LeaveTypeDto>>
            {
                Data = entities
                    .Select(e => e.ToDto())
                    .OrderBy(x => x.DisplayOrder)
                    .ThenBy(x => x.Name)
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting leave types: {Error}", e.Message);
        }

        return new BaseResponse<IEnumerable<LeaveTypeDto>>
        {
            Data = [],
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
