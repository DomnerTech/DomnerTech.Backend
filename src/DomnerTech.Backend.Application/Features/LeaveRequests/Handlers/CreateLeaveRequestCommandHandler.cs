using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.Enums;
using DomnerTech.Backend.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveRequests.Handlers;

public sealed class CreateLeaveRequestCommandHandler(
    ILogger<CreateLeaveRequestCommandHandler> logger,
    ILeaveRequestRepo leaveRequestRepo,
    ILeaveBalanceRepo leaveBalanceRepo,
    ILeaveTypeRepo leaveTypeRepo,
    ILeaveValidationService leaveValidationService,
    INotificationService notificationService,
    ITenantService tenantService,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<CreateLeaveRequestCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var leaveTypeId = ObjectId.Parse(r.LeaveTypeId);
            var employeeId = httpContextAccessor.HttpContext?.GetClaim(ClaimConstant.EmpId).ToObjectId() ?? ObjectId.Empty;

            if (employeeId == ObjectId.Empty)
            {
                throw new UnauthorizedException();
            }

            // Calculate leave days
            var isHalfDay = r.RequestType != LeaveRequestType.FullDay;
            var totalDays = await leaveValidationService.CalculateLeaveDaysAsync(
                leaveTypeId, r.StartDate, r.EndDate, isHalfDay, cancellationToken);

            // Validate leave request
            var (isValid, errors) = await leaveValidationService.ValidateLeaveRequestAsync(
                employeeId, leaveTypeId, r.StartDate, r.EndDate, totalDays, null, cancellationToken);

            if (!isValid)
            {
                throw new ValidationException(string.Join(", ", errors));
            }

            // Check document requirement
            var leaveType = await leaveTypeRepo.GetByIdAsync(leaveTypeId, cancellationToken);
            if (leaveType?.RequiresDocument == true && (r.DocumentUrls == null || r.DocumentUrls.Count == 0))
            {
                throw new ValidationException("Supporting documents are required for this leave type");
            }

            var date = DateTime.UtcNow;
            var entity = new LeaveRequestEntity
            {
                Id = ObjectId.GenerateNewId(),
                CompanyId = tenantService.CompanyId.ToObjectId(),
                EmployeeId = employeeId,
                LeaveTypeId = leaveTypeId,
                Period = new LeavePeriodValueObject
                {
                    StartDate = r.StartDate.Date,
                    EndDate = r.EndDate.Date
                },
                RequestType = r.RequestType,
                TotalDays = totalDays,
                Reason = r.Reason,
                Notes = r.Notes,
                DocumentUrls = r.DocumentUrls,
                Status = LeaveRequestStatus.Pending,
                SubmittedAt = date,
                CreatedAt = date,
                UpdatedAt = date,
                IsDeleted = false
            };

            var id = await leaveRequestRepo.CreateAsync(entity, cancellationToken);

            // Send notification
            await notificationService.SendLeaveRequestSubmittedAsync(employeeId, id, cancellationToken);

            return new BaseResponse<string>
            {
                Data = id.ToString()
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (BaseErrorException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating leave request: {Error}", e.Message);
        }

        return new BaseResponse<string>
        {
            Data = string.Empty,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
