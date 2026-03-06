using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeavePolicies;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.LeavePolicies.Handlers;

public sealed class GetActivePoliciesQueryHandler(
    ILogger<GetActivePoliciesQueryHandler> logger,
    ILeavePolicyRepo leavePolicyRepo) : IRequestHandler<GetActivePoliciesQuery, BaseResponse<List<LeavePolicyDto>>>
{
    public async Task<BaseResponse<List<LeavePolicyDto>>> Handle(GetActivePoliciesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await leavePolicyRepo.GetAllActiveAsync(cancellationToken);
            return new BaseResponse<List<LeavePolicyDto>>
            {
                Data = [.. entities.Select(e => e.ToDto())]
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting active policies: {Error}", e.Message);
        }

        return new BaseResponse<List<LeavePolicyDto>>
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