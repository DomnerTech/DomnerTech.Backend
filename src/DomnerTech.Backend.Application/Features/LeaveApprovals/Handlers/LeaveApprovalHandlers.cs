using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveApprovals;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveApprovals.Handlers;

public sealed class ApproveLeaveCommandHandler(
    ILogger<ApproveLeaveCommandHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    ILeaveApprovalRepo leaveApprovalRepo,
    ILeaveBalanceRepo leaveBalanceRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<ApproveLeaveCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(ApproveLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var leaveRequestId = ObjectId.Parse(r.LeaveRequestId);
            
            var approverId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "EmployeeId")?.Value.ToObjectId() ?? ObjectId.Empty;

            if (approverId == ObjectId.Empty)
            {
                throw new UnauthorizedException();
            }

            // Get leave request
            var leaveRequest = await leaveRequestRepo.GetByIdAsync(leaveRequestId, cancellationToken);
            if (leaveRequest == null)
            {
                throw new NotFoundException("Leave request not found");
            }

            if (leaveRequest.Status != LeaveRequestStatus.Pending)
            {
                throw new ValidationException("Leave request is not in pending status");
            }

            // Get current pending approval
            var currentApproval = await leaveApprovalRepo.GetCurrentPendingApprovalAsync(leaveRequestId, cancellationToken);
            
            if (currentApproval == null)
            {
                // Create first approval if none exists
                currentApproval = new Domain.Entities.LeaveApprovalEntity
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
                throw new ValidationException("You are not authorized to approve this request");
            }

            // Approve current step
            currentApproval.Status = LeaveRequestStatus.Approved;
            currentApproval.Comments = r.Comments;
            currentApproval.ActionDate = DateTime.UtcNow;
            currentApproval.UpdatedAt = DateTime.UtcNow;
            await leaveApprovalRepo.UpdateAsync(currentApproval, cancellationToken);

            // If final approval, update leave request and deduct balance
            if (currentApproval.IsFinalApproval)
            {
                leaveRequest.Status = LeaveRequestStatus.Approved;
                leaveRequest.ApprovedBy = approverId;
                leaveRequest.ApprovedAt = DateTime.UtcNow;
                leaveRequest.UpdatedAt = DateTime.UtcNow;
                await leaveRequestRepo.UpdateAsync(leaveRequest, cancellationToken);

                // Deduct from leave balance
                var year = leaveRequest.Period.StartDate.Year;
                var balance = await leaveBalanceRepo.GetByEmployeeAndTypeAsync(
                    leaveRequest.EmployeeId, 
                    leaveRequest.LeaveTypeId, 
                    year, 
                    cancellationToken);

                if (balance != null)
                {
                    balance.Allowance.UsedDays += leaveRequest.TotalDays;
                    balance.UpdatedAt = DateTime.UtcNow;
                    await leaveBalanceRepo.UpdateAsync(balance, cancellationToken);
                }
            }

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
            logger.LogError(e, "Error approving leave: {Error}", e.Message);
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
            
            var approverId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "EmployeeId")?.Value.ToObjectId() ?? ObjectId.Empty;

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
                currentApproval = new Domain.Entities.LeaveApprovalEntity
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

public sealed class GetPendingApprovalsQueryHandler(
    ILogger<GetPendingApprovalsQueryHandler> logger,
    ILeaveApprovalRepo leaveApprovalRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetPendingApprovalsQuery, BaseResponse<List<LeaveApprovalDto>>>
{
    public async Task<BaseResponse<List<LeaveApprovalDto>>> Handle(GetPendingApprovalsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var approverId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "EmployeeId")?.Value.ToObjectId() ?? ObjectId.Empty;

            if (approverId == ObjectId.Empty)
            {
                throw new UnauthorizedException();
            }

            var entities = await leaveApprovalRepo.GetPendingByApproverAsync(approverId, cancellationToken);

            var dtos = entities.Select(e => new LeaveApprovalDto
            {
                Id = e.Id.ToString(),
                LeaveRequestId = e.LeaveRequestId.ToString(),
                Level = e.Level.ToString(),
                ApproverId = e.ApproverId.ToString(),
                Status = e.Status.ToString(),
                Comments = e.Comments,
                ActionDate = e.ActionDate,
                SequenceOrder = e.SequenceOrder,
                IsFinalApproval = e.IsFinalApproval
            }).ToList();

            return new BaseResponse<List<LeaveApprovalDto>> { Data = dtos };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (UnauthorizedException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting pending approvals: {Error}", e.Message);
        }

        return new BaseResponse<List<LeaveApprovalDto>>
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

public sealed class GetLeaveApprovalHistoryQueryHandler(
    ILogger<GetLeaveApprovalHistoryQueryHandler> logger,
    ILeaveApprovalRepo leaveApprovalRepo) : IRequestHandler<GetLeaveApprovalHistoryQuery, BaseResponse<List<LeaveApprovalDto>>>
{
    public async Task<BaseResponse<List<LeaveApprovalDto>>> Handle(GetLeaveApprovalHistoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var leaveRequestId = ObjectId.Parse(request.LeaveRequestId);
            var entities = await leaveApprovalRepo.GetByLeaveRequestAsync(leaveRequestId, cancellationToken);

            var dtos = entities.Select(e => new LeaveApprovalDto
            {
                Id = e.Id.ToString(),
                LeaveRequestId = e.LeaveRequestId.ToString(),
                Level = e.Level.ToString(),
                ApproverId = e.ApproverId.ToString(),
                Status = e.Status.ToString(),
                Comments = e.Comments,
                ActionDate = e.ActionDate,
                SequenceOrder = e.SequenceOrder,
                IsFinalApproval = e.IsFinalApproval
            }).ToList();

            return new BaseResponse<List<LeaveApprovalDto>> { Data = dtos };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting approval history: {Error}", e.Message);
        }

        return new BaseResponse<List<LeaveApprovalDto>>
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
