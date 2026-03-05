using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveBalances;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using DomnerTech.Backend.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveBalances.Handlers;

public sealed class InitializeLeaveBalanceCommandHandler(
    ILogger<InitializeLeaveBalanceCommandHandler> logger,
    ILeaveBalanceRepo leaveBalanceRepo,
    ILeaveTypeRepo leaveTypeRepo,
    IEmployeeRepo employeeRepo,
    ITenantService tenantService) : IRequestHandler<InitializeLeaveBalanceCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> Handle(InitializeLeaveBalanceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var employeeId = ObjectId.Parse(r.EmployeeId);
            var leaveTypeId = ObjectId.Parse(r.LeaveTypeId);

            // Check if employee exists
            var employee = await employeeRepo.GetByIdAsync(employeeId, cancellationToken);
            if (employee == null)
            {
                throw new NotFoundException("Employee not found");
            }

            // Check if leave type exists
            var leaveType = await leaveTypeRepo.GetByIdAsync(leaveTypeId, cancellationToken);
            if (leaveType == null)
            {
                throw new NotFoundException("Leave type not found");
            }

            // Check if balance already exists
            var existing = await leaveBalanceRepo.GetByEmployeeAndTypeAsync(employeeId, leaveTypeId, r.Year, cancellationToken);
            if (existing != null)
            {
                throw new ConflictException("Leave balance already exists for this employee and leave type");
            }

            var date = DateTime.UtcNow;
            var entity = new LeaveBalanceEntity
            {
                Id = ObjectId.GenerateNewId(),
                CompanyId = tenantService.CompanyId.ToObjectId(),
                EmployeeId = employeeId,
                LeaveTypeId = leaveTypeId,
                Year = r.Year,
                Allowance = new LeaveAllowanceValueObject
                {
                    TotalAllowance = r.TotalAllowance,
                    UsedDays = 0,
                    CarriedForwardDays = r.CarriedForwardDays
                },
                IsActive = true,
                CreatedAt = date,
                UpdatedAt = date
            };

            var id = await leaveBalanceRepo.CreateAsync(entity, cancellationToken);

            return new BaseResponse<string> { Data = id.ToString() };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (ConflictException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error initializing leave balance: {Error}", e.Message);
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

public sealed class AdjustLeaveBalanceCommandHandler(
    ILogger<AdjustLeaveBalanceCommandHandler> logger,
    ILeaveBalanceRepo leaveBalanceRepo) : IRequestHandler<AdjustLeaveBalanceCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(AdjustLeaveBalanceCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var employeeId = ObjectId.Parse(r.EmployeeId);
            var leaveTypeId = ObjectId.Parse(r.LeaveTypeId);

            var balance = await leaveBalanceRepo.GetByEmployeeAndTypeAsync(employeeId, leaveTypeId, r.Year, cancellationToken);
            if (balance == null)
            {
                throw new NotFoundException("Leave balance not found");
            }

            balance.Allowance.TotalAllowance += r.AdjustmentDays;
            balance.UpdatedAt = DateTime.UtcNow;

            await leaveBalanceRepo.UpdateAsync(balance, cancellationToken);

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
            logger.LogError(e, "Error adjusting leave balance: {Error}", e.Message);
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

public sealed class GetMyLeaveBalancesQueryHandler(
    ILogger<GetMyLeaveBalancesQueryHandler> logger,
    ILeaveBalanceRepo leaveBalanceRepo,
    ILeaveTypeRepo leaveTypeRepo,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetMyLeaveBalancesQuery, BaseResponse<List<LeaveBalanceSummaryDto>>>
{
    public async Task<BaseResponse<List<LeaveBalanceSummaryDto>>> Handle(GetMyLeaveBalancesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "EmployeeId")?.Value.ToObjectId() ?? ObjectId.Empty;

            if (employeeId == ObjectId.Empty)
            {
                throw new UnauthorizedException();
            }

            var balances = await leaveBalanceRepo.GetByEmployeeAsync(employeeId, request.Year, cancellationToken);
            var leaveTypes = await leaveTypeRepo.GetAllActiveAsync(cancellationToken);

            var summary = balances.Select(b =>
            {
                var leaveType = leaveTypes.FirstOrDefault(lt => lt.Id == b.LeaveTypeId);
                return new LeaveBalanceSummaryDto
                {
                    LeaveTypeName = leaveType?.Name ?? "Unknown",
                    LeaveTypeCode = leaveType?.Code ?? "UNKNOWN",
                    TotalAllowance = b.Allowance.TotalAllowance,
                    UsedDays = b.Allowance.UsedDays,
                    RemainingDays = b.Allowance.RemainingDays,
                    CarriedForwardDays = b.Allowance.CarriedForwardDays
                };
            }).ToList();

            return new BaseResponse<List<LeaveBalanceSummaryDto>> { Data = summary };
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
            logger.LogError(e, "Error getting my leave balances: {Error}", e.Message);
        }

        return new BaseResponse<List<LeaveBalanceSummaryDto>>
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

public sealed class GetEmployeeLeaveBalancesQueryHandler(
    ILogger<GetEmployeeLeaveBalancesQueryHandler> logger,
    ILeaveBalanceRepo leaveBalanceRepo,
    ILeaveTypeRepo leaveTypeRepo) : IRequestHandler<GetEmployeeLeaveBalancesQuery, BaseResponse<List<LeaveBalanceSummaryDto>>>
{
    public async Task<BaseResponse<List<LeaveBalanceSummaryDto>>> Handle(GetEmployeeLeaveBalancesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employeeId = ObjectId.Parse(request.EmployeeId);
            var balances = await leaveBalanceRepo.GetByEmployeeAsync(employeeId, request.Year, cancellationToken);
            var leaveTypes = await leaveTypeRepo.GetAllActiveAsync(cancellationToken);

            var summary = balances.Select(b =>
            {
                var leaveType = leaveTypes.FirstOrDefault(lt => lt.Id == b.LeaveTypeId);
                return new LeaveBalanceSummaryDto
                {
                    LeaveTypeName = leaveType?.Name ?? "Unknown",
                    LeaveTypeCode = leaveType?.Code ?? "UNKNOWN",
                    TotalAllowance = b.Allowance.TotalAllowance,
                    UsedDays = b.Allowance.UsedDays,
                    RemainingDays = b.Allowance.RemainingDays,
                    CarriedForwardDays = b.Allowance.CarriedForwardDays
                };
            }).ToList();

            return new BaseResponse<List<LeaveBalanceSummaryDto>> { Data = summary };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting employee leave balances: {Error}", e.Message);
        }

        return new BaseResponse<List<LeaveBalanceSummaryDto>>
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
