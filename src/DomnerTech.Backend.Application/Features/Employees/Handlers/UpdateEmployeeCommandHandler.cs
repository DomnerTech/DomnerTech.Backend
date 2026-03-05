using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Exceptions;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Employees.Handlers;

/// <summary>
/// Handler for processing employee update commands.
/// </summary>
public sealed class UpdateEmployeeCommandHandler(
    ILogger<UpdateEmployeeCommandHandler> logger,
    IEmployeeRepo employeeRepo,
    ITenantService tenantService) : IRequestHandler<UpdateEmployeeCommand, BaseResponse<bool>>
{
    /// <summary>
    /// Handles the update employee command by validating and updating the employee information.
    /// </summary>
    /// <param name="request">The update employee command containing the employee data.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A response indicating success or failure of the update operation.</returns>
    public async Task<BaseResponse<bool>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            
            if (!ObjectId.TryParse(r.Id, out var employeeId))
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorCode = ErrorCodes.Employee.IdInvalid
                    }
                };
            }

            var existingEmployee = await employeeRepo.GetByIdAsync(employeeId, cancellationToken);
            
            if (existingEmployee == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorCode = ErrorCodes.Employee.EmpNotFound
                    }
                };
            }

            if (existingEmployee.CompanyId != tenantService.CompanyId.ToObjectId())
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status403Forbidden,
                        ErrorCode = ErrorCodes.Forbidden
                    }
                };
            }

            existingEmployee.FirstName = r.FirstName;
            existingEmployee.LastName = r.LastName;
            existingEmployee.Email = r.Email;
            existingEmployee.PhoneNumber = r.PhoneNumber;
            existingEmployee.DateOfBirth = r.DateOfBirth.Date;
            existingEmployee.Department = r.Department;
            existingEmployee.JobTitle = r.JobTitle;
            existingEmployee.Address = r.Address?.ToValueObject();
            existingEmployee.IsActive = r.IsActive;
            existingEmployee.UpdatedAt = DateTime.UtcNow;

            await employeeRepo.UpdateAsync(existingEmployee, cancellationToken);
            
            return new BaseResponse<bool>
            {
                Data = true
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating employee: {Error}", e.Message);
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
