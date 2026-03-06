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
        ObjectId userId,
        string userName,
        string action,
        string entityType,
        ObjectId? entityId,
        string description,
        string? oldValues = null,
        string? newValues = null,
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
                UserId = userId,
                UserName = userName,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                Description = description,
                OldValues = oldValues,
                NewValues = newValues,
                IpAddress = httpContext?.Connection.RemoteIpAddress?.ToString(),
                UserAgent = httpContext?.Request.Headers["User-Agent"].ToString(),
                CreatedAt = date,
                UpdatedAt = date
            };

            await auditLogRepo.CreateAsync(auditLog, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error logging audit action: {Action}", action);
        }
    }

    public async Task LogLeaveRequestCreatedAsync(ObjectId userId, string userName, ObjectId leaveRequestId, CancellationToken cancellationToken = default)
    {
        await LogActionAsync(
            userId,
            userName,
            "LeaveRequestCreated",
            "LeaveRequest",
            leaveRequestId,
            $"Leave request created by {userName}",
            cancellationToken: cancellationToken);
    }

    public async Task LogLeaveRequestApprovedAsync(ObjectId userId, string userName, ObjectId leaveRequestId, CancellationToken cancellationToken = default)
    {
        await LogActionAsync(
            userId,
            userName,
            "LeaveRequestApproved",
            "LeaveRequest",
            leaveRequestId,
            $"Leave request approved by {userName}",
            cancellationToken: cancellationToken);
    }

    public async Task LogLeaveRequestRejectedAsync(ObjectId userId, string userName, ObjectId leaveRequestId, string reason, CancellationToken cancellationToken = default)
    {
        await LogActionAsync(
            userId,
            userName,
            "LeaveRequestRejected",
            "LeaveRequest",
            leaveRequestId,
            $"Leave request rejected by {userName}. Reason: {reason}",
            cancellationToken: cancellationToken);
    }

    public async Task LogLeaveRequestCancelledAsync(ObjectId userId, string userName, ObjectId leaveRequestId, CancellationToken cancellationToken = default)
    {
        await LogActionAsync(
            userId,
            userName,
            "LeaveRequestCancelled",
            "LeaveRequest",
            leaveRequestId,
            $"Leave request cancelled by {userName}",
            cancellationToken: cancellationToken);
    }

    public async Task LogPolicyUpdatedAsync(ObjectId userId, string userName, ObjectId policyId, string oldValues, string newValues, CancellationToken cancellationToken = default)
    {
        await LogActionAsync(
            userId,
            userName,
            "PolicyUpdated",
            "LeavePolicy",
            policyId,
            $"Leave policy updated by {userName}",
            oldValues,
            newValues,
            cancellationToken);
    }
}
