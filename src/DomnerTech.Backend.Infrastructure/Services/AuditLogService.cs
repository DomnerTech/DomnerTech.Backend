using DomnerTech.Backend.Application.Constants;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Infrastructure.Services;

public sealed class AuditLogService(
    ILogger<AuditLogService> logger,
    IAuditLogRepo auditLogRepo,
    ITenantService tenantService,
    IHttpContextAccessor httpContextAccessor) : IAuditLogService
{
    public async Task LogActionAsync(
        LogActionParams param,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpContext = httpContextAccessor.HttpContext;
            var date = DateTime.UtcNow;

            var auditLog = new AuditLogEntity
            {
                Id = ObjectId.GenerateNewId(),
                CompanyId = tenantService.CompanyId.ToObjectId(),
                UserId = param.UserId,
                UserName = param.Username,
                Action = param.Action,
                EntityType = param.EntityType,
                EntityId = param.EntityId,
                Description = param.Description,
                OldValues = param.OldValues,
                NewValues = param.NewValues,
                IpAddress = httpContext?.Connection.RemoteIpAddress?.ToString(),
                UserAgent = httpContext?.Request.Headers.UserAgent.ToString(),
                CreatedAt = date,
                UpdatedAt = date
            };

            await auditLogRepo.CreateAsync(auditLog, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error logging audit action: {Action}", param.Action);
        }
    }

    public async Task LogLeaveRequestCreatedAsync(
        ObjectId userId,
        string userName,
        ObjectId leaveRequestId,
        CancellationToken cancellationToken = default)
    {
        await LogActionAsync(new LogActionParams
        {
            UserId = userId,
            Username = userName,
            Action = "LeaveRequestCreated",
            EntityType = LogAuditActivity.LeaveRequest,
            Description = $"Leave request created by {userName}",
            EntityId = leaveRequestId
        }, cancellationToken);
    }

    public async Task LogLeaveRequestApprovedAsync(
        ObjectId userId,
        string userName,
        ObjectId leaveRequestId,
        CancellationToken cancellationToken = default)
    {
        await LogActionAsync(new LogActionParams
        {
            UserId = userId,
            Username = userName,
            Action = "LeaveRequestApproved",
            EntityType = LogAuditActivity.LeaveRequest,
            EntityId = leaveRequestId,
            Description = $"Leave request approved by {userName}"
        }, cancellationToken);
    }

    public async Task LogLeaveRequestRejectedAsync(
        ObjectId userId,
        string userName,
        ObjectId leaveRequestId,
        string reason,
        CancellationToken cancellationToken = default)
    {
        await LogActionAsync(new LogActionParams
        {
            UserId = userId,
            Username = userName,
            Action = "LeaveRequestRejected",
            EntityType = LogAuditActivity.LeaveRequest,
            EntityId = leaveRequestId,
            Description = $"Leave request rejected by {userName}. Reason: {reason}"
        }, cancellationToken);
    }

    public async Task LogLeaveRequestCancelledAsync(
        ObjectId userId,
        string userName,
        ObjectId leaveRequestId,
        CancellationToken cancellationToken = default)
    {
        await LogActionAsync(new LogActionParams
        {
            UserId = userId,
            Username = userName,
            Action = "LeaveRequestCancelled",
            EntityType = LogAuditActivity.LeaveRequest,
            EntityId = leaveRequestId,
            Description = "Leave request cancelled by {userName}"
        }, cancellationToken);
    }

    public async Task LogPolicyUpdatedAsync(
        ObjectId userId,
        string userName,
        ObjectId policyId,
        string oldValues,
        string newValues,
        CancellationToken cancellationToken = default)
    {
        await LogActionAsync(new LogActionParams
        {
            UserId = userId,
            Username = userName,
            Action = "PolicyUpdated",
            EntityType = LogAuditActivity.LeavePolicy,
            EntityId = policyId,
            Description = $"Leave policy updated by {userName}",
            OldValues = oldValues,
            NewValues = newValues
        }, cancellationToken);
    }
}
