using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveApprovals.Handlers;

public sealed class RejectLeaveCommandHandler(
    ILogger<RejectLeaveCommandHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    ILeaveApprovalRepo leaveApprovalRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<RejectLeaveCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(RejectLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var leaveRequestId = ObjectId.Parse(r.LeaveRequestId);
            
            var approverId = httpContextAccessor.HttpContext?.GetClaim(ClaimConstant.EmpId).ToObjectId() ?? ObjectId.Empty;

            if (approverId == ObjectId.Empty)
            {
                throw new UnauthorizedException();
            }

            var leaveRequest = await leaveRequestRepo.GetByIdAsync(leaveRequestId, cancellationToken);
            if (leaveRequest == null)
            {
                throw new NotFoundException("Leave request not found");
            }

            if (leaveRequest.Status != LeaveRequestStatus.Pending)
            {
                throw new ValidationException("Leave request is not in pending status");
            }

            var currentApproval = await leaveApprovalRepo.GetCurrentPendingApprovalAsync(leaveRequestId, cancellationToken);
            
            if (currentApproval == null)
            {
                currentApproval = new LeaveApprovalEntity
                {
                    Id = ObjectId.GenerateNewId(),
                    CompanyId = leaveRequest.CompanyId,
                    LeaveRequestId = leaveRequestId,
                    Level = ApprovalLevel.Manager,
                    ApproverId = approverId,
                    Status = LeaveRequestStatus.Pending,
                    SequenceOrder = 1,
                    IsFinalApproval = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await leaveApprovalRepo.CreateAsync(currentApproval, cancellationToken);
            }

            if (currentApproval.ApproverId != approverId)
            {
                throw new ValidationException("You are not authorized to reject this request");
            }

            currentApproval.Status = LeaveRequestStatus.Rejected;
            currentApproval.Comments = r.Comments;
            currentApproval.ActionDate = DateTime.UtcNow;
            currentApproval.UpdatedAt = DateTime.UtcNow;
            await leaveApprovalRepo.UpdateAsync(currentApproval, cancellationToken);

            leaveRequest.Status = LeaveRequestStatus.Rejected;
            leaveRequest.RejectionReason = r.RejectionReason;
            leaveRequest.UpdatedAt = DateTime.UtcNow;
            await leaveRequestRepo.UpdateAsync(leaveRequest, cancellationToken);

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
        catch (ValidationException)
        {
            throw;
        }
        catch (UnauthorizedException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error rejecting leave: {Error}", e.Message);
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