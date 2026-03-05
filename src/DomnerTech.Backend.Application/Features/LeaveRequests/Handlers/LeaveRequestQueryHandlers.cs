using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveRequests.Handlers;

public sealed class UpdateLeaveRequestCommandHandler(
    ILogger<UpdateLeaveRequestCommandHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    ILeaveValidationService leaveValidationService) : IRequestHandler<UpdateLeaveRequestCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var requestId = ObjectId.Parse(r.Id);

            var existing = await leaveRequestRepo.GetByIdAsync(requestId, cancellationToken);
            if (existing is null)
            {
                throw new NotFoundException("Leave request not found");
            }

            if (existing.Status != LeaveRequestStatus.Pending)
            {
                throw new ValidationException("Only pending leave requests can be updated");
            }

            var isHalfDay = r.RequestType != LeaveRequestType.FullDay;
            var totalDays = await leaveValidationService.CalculateLeaveDaysAsync(
                existing.LeaveTypeId, r.StartDate, r.EndDate, isHalfDay, cancellationToken);

            var (isValid, errors) = await leaveValidationService.ValidateLeaveRequestAsync(
                existing.EmployeeId, existing.LeaveTypeId, r.StartDate, r.EndDate, totalDays, requestId, cancellationToken);

            if (!isValid)
            {
                throw new ValidationException(string.Join(", ", errors));
            }

            existing.Period.StartDate = r.StartDate.Date;
            existing.Period.EndDate = r.EndDate.Date;
            existing.RequestType = r.RequestType;
            existing.TotalDays = totalDays;
            existing.Reason = r.Reason;
            existing.Notes = r.Notes;
            existing.DocumentUrls = r.DocumentUrls;
            existing.UpdatedAt = DateTime.UtcNow;

            await leaveRequestRepo.UpdateAsync(existing, cancellationToken);

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
        catch (Exception e)
        {
            logger.LogError(e, "Error updating leave request: {Error}", e.Message);
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

public sealed class CancelLeaveRequestCommandHandler(
    ILogger<CancelLeaveRequestCommandHandler> logger,
    ILeaveRequestRepo leaveRequestRepo) : IRequestHandler<CancelLeaveRequestCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var requestId = ObjectId.Parse(r.Id);

            var existing = await leaveRequestRepo.GetByIdAsync(requestId, cancellationToken);
            if (existing is null)
            {
                throw new NotFoundException("Leave request not found");
            }

            if (existing.Status == LeaveRequestStatus.Cancelled)
            {
                throw new ValidationException("Leave request is already cancelled");
            }

            if (existing.Status == LeaveRequestStatus.Rejected)
            {
                throw new ValidationException("Cannot cancel a rejected leave request");
            }

            existing.Status = LeaveRequestStatus.Cancelled;
            existing.CancellationReason = r.CancellationReason;
            existing.CancelledAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;

            await leaveRequestRepo.UpdateAsync(existing, cancellationToken);

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
        catch (Exception e)
        {
            logger.LogError(e, "Error cancelling leave request: {Error}", e.Message);
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

public sealed class GetLeaveRequestByIdQueryHandler(
    ILogger<GetLeaveRequestByIdQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    ILeaveTypeRepo leaveTypeRepo,
    IEmployeeRepo employeeRepo) : IRequestHandler<GetLeaveRequestByIdQuery, BaseResponse<LeaveRequestDetailDto>>
{
    public async Task<BaseResponse<LeaveRequestDetailDto>> Handle(GetLeaveRequestByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var requestId = ObjectId.Parse(request.Id);
            var entity = await leaveRequestRepo.GetByIdAsync(requestId, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException("Leave request not found");
            }

            var leaveType = await leaveTypeRepo.GetByIdAsync(entity.LeaveTypeId, cancellationToken);
            var employee = await employeeRepo.GetByIdAsync(entity.EmployeeId, cancellationToken);

            var dto = new LeaveRequestDetailDto
            {
                Id = entity.Id.ToString(),
                EmployeeId = entity.EmployeeId.ToString(),
                EmployeeName = employee != null ? $"{employee.FirstName} {employee.LastName}" : null,
                LeaveTypeId = entity.LeaveTypeId.ToString(),
                LeaveTypeName = leaveType?.Name,
                StartDate = entity.Period.StartDate,
                EndDate = entity.Period.EndDate,
                RequestType = entity.RequestType,
                TotalDays = entity.TotalDays,
                Reason = entity.Reason,
                Notes = entity.Notes,
                DocumentUrls = entity.DocumentUrls,
                Status = entity.Status,
                SubmittedAt = entity.SubmittedAt,
                CurrentApprovalLevel = entity.CurrentApprovalLevel,
                ApprovedBy = entity.ApprovedBy?.ToString(),
                ApprovedAt = entity.ApprovedAt,
                RejectionReason = entity.RejectionReason,
                CancellationReason = entity.CancellationReason,
                CancelledAt = entity.CancelledAt,
                CreatedAt = entity.CreatedAt
            };

            return new BaseResponse<LeaveRequestDetailDto> { Data = dto };
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
            logger.LogError(e, "Error getting leave request: {Error}", e.Message);
        }

        return new BaseResponse<LeaveRequestDetailDto>
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

public sealed class GetMyLeaveRequestsQueryHandler(
    ILogger<GetMyLeaveRequestsQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetMyLeaveRequestsQuery, BaseResponse<List<LeaveRequestDto>>>
{
    public async Task<BaseResponse<List<LeaveRequestDto>>> Handle(GetMyLeaveRequestsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "EmployeeId")?.Value;

            if (string.IsNullOrEmpty(employeeId) || !ObjectId.TryParse(employeeId, out var empId))
            {
                throw new UnauthorizedException();
            }

            var entities = await leaveRequestRepo.GetByEmployeeAsync(empId, cancellationToken);

            var dtos = entities.Select(e => new LeaveRequestDto
            {
                Id = e.Id.ToString(),
                EmployeeId = e.EmployeeId.ToString(),
                LeaveTypeId = e.LeaveTypeId.ToString(),
                StartDate = e.Period.StartDate,
                EndDate = e.Period.EndDate,
                RequestType = e.RequestType,
                TotalDays = e.TotalDays,
                Reason = e.Reason,
                Notes = e.Notes,
                DocumentUrls = e.DocumentUrls,
                Status = e.Status,
                SubmittedAt = e.SubmittedAt,
                CurrentApprovalLevel = e.CurrentApprovalLevel,
                ApprovedBy = e.ApprovedBy?.ToString(),
                ApprovedAt = e.ApprovedAt,
                RejectionReason = e.RejectionReason,
                CancellationReason = e.CancellationReason,
                CancelledAt = e.CancelledAt,
                CreatedAt = e.CreatedAt
            }).ToList();

            return new BaseResponse<List<LeaveRequestDto>> { Data = dtos };
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
            logger.LogError(e, "Error getting my leave requests: {Error}", e.Message);
        }

        return new BaseResponse<List<LeaveRequestDto>>
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

public sealed class GetLeaveRequestsByStatusQueryHandler(
    ILogger<GetLeaveRequestsByStatusQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo) : IRequestHandler<GetLeaveRequestsByStatusQuery, BaseResponse<List<LeaveRequestDto>>>
{
    public async Task<BaseResponse<List<LeaveRequestDto>>> Handle(GetLeaveRequestsByStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!Enum.TryParse<LeaveRequestStatus>(request.Status, true, out var status))
            {
                throw new ValidationException("Invalid leave request status");
            }

            var entities = await leaveRequestRepo.GetByStatusAsync(status, cancellationToken);

            var dtos = entities.Select(e => new LeaveRequestDto
            {
                Id = e.Id.ToString(),
                EmployeeId = e.EmployeeId.ToString(),
                LeaveTypeId = e.LeaveTypeId.ToString(),
                StartDate = e.Period.StartDate,
                EndDate = e.Period.EndDate,
                RequestType = e.RequestType,
                TotalDays = e.TotalDays,
                Reason = e.Reason,
                Notes = e.Notes,
                DocumentUrls = e.DocumentUrls,
                Status = e.Status,
                SubmittedAt = e.SubmittedAt,
                CurrentApprovalLevel = e.CurrentApprovalLevel,
                ApprovedBy = e.ApprovedBy?.ToString(),
                ApprovedAt = e.ApprovedAt,
                RejectionReason = e.RejectionReason,
                CancellationReason = e.CancellationReason,
                CancelledAt = e.CancelledAt,
                CreatedAt = e.CreatedAt
            }).ToList();

            return new BaseResponse<List<LeaveRequestDto>> { Data = dtos };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting leave requests by status: {Error}", e.Message);
        }

        return new BaseResponse<List<LeaveRequestDto>>
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
