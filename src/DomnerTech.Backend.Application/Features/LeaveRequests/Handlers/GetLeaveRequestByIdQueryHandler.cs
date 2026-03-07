using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.LeaveRequests.Handlers;

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
                return new BaseResponse<LeaveRequestDetailDto>
                {
                    Data = null!,
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorCode = ErrorCodes.Leave.RequestNotFound
                    }
                };
            }

            var leaveType = await leaveTypeRepo.GetByIdAsync(entity.LeaveTypeId, cancellationToken);
            var employee = await employeeRepo.GetByIdAsync(entity.EmployeeId, cancellationToken);
            var empName = employee != null ? $"{employee.FirstName} {employee.LastName}" : null;
            return new BaseResponse<LeaveRequestDetailDto>
            {
                Data = entity.ToDto(leaveType?.Name, empName)
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