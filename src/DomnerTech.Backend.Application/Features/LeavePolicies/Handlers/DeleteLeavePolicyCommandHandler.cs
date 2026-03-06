using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeavePolicies.Handlers;

public sealed class DeleteLeavePolicyCommandHandler(
    ILogger<DeleteLeavePolicyCommandHandler> logger,
    ILeavePolicyRepo leavePolicyRepo) : IRequestHandler<DeleteLeavePolicyCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(DeleteLeavePolicyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var policyId = ObjectId.Parse(request.Id);

            var existing = await leavePolicyRepo.GetByIdAsync(policyId, cancellationToken);
            if (existing is null)
            {
                throw new NotFoundException("Leave policy not found");
            }

            await leavePolicyRepo.DeleteAsync(policyId, cancellationToken);

            return new BaseResponse<bool> { Data = true };
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
            logger.LogError(e, "Error deleting leave policy: {Error}", e.Message);
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