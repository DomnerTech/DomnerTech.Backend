using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeavePolicies;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeavePolicies.Handlers;

public sealed class GetPolicyByLeaveTypeQueryHandler(
    ILogger<GetPolicyByLeaveTypeQueryHandler> logger,
    ILeavePolicyRepo leavePolicyRepo) : IRequestHandler<GetPolicyByLeaveTypeQuery, BaseResponse<LeavePolicyDto>>
{
    public async Task<BaseResponse<LeavePolicyDto>> Handle(GetPolicyByLeaveTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var leaveTypeId = ObjectId.Parse(request.LeaveTypeId);
            var entity = await leavePolicyRepo.GetByLeaveTypeIdAsync(leaveTypeId, cancellationToken) 
                         ?? await leavePolicyRepo.GetDefaultPolicyAsync(cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException("No policy found for this leave type");
            }

            return new BaseResponse<LeavePolicyDto>
            {
                Data = entity.ToDto()
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
            logger.LogError(e, "Error getting policy by leave type: {Error}", e.Message);
        }

        return new BaseResponse<LeavePolicyDto>
        {
            Data = null!,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
