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

public sealed class GetLeavePolicyByIdQueryHandler(
    ILogger<GetLeavePolicyByIdQueryHandler> logger,
    ILeavePolicyRepo leavePolicyRepo) : IRequestHandler<GetLeavePolicyByIdQuery, BaseResponse<LeavePolicyDto>>
{
    public async Task<BaseResponse<LeavePolicyDto>> Handle(GetLeavePolicyByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var policyId = ObjectId.Parse(request.Id);
            var entity = await leavePolicyRepo.GetByIdAsync(policyId, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException("Leave policy not found");
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
            logger.LogError(e, "Error getting leave policy: {Error}", e.Message);
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