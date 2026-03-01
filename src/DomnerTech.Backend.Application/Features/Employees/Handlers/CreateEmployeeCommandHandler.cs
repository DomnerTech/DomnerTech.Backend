using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using DomnerTech.Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Employees.Handlers;

public sealed class CreateEmployeeCommandHandler(
    ILogger<CreateEmployeeCommandHandler> logger,
    IEmployeeRepo employeeRepo,
    ITenantService tenantService) : IRequestHandler<CreateEmployeeCommand, BaseResponse<bool>>
{
    private const int StartingEmployeeNumber = 100;
    public async Task<BaseResponse<bool>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var r = request.Dto;
            var empCount = await employeeRepo.GetCountAsync(cancellationToken);
            var date = DateTime.UtcNow;
            var entity = new EmployeeEntity
            {
                FirstName = r.FirstName,
                LastName = r.LastName,
                Email = r.Email,
                PhoneNumber = r.PhoneNumber,
                Department = r.Department,
                JobTitle = r.JobTitle,
                Address = r.Address.ToValueObject(),
                EmployeeNumber = $"{StartingEmployeeNumber + empCount + 1}",
                CompanyId = tenantService.CompanyId.ToObjectId(),
                CreatedAt = date,
                DateOfBirth = r.DateOfBirth.Date,
                HireDate = r.HireDate,
                Id = ObjectId.GenerateNewId(),
                IsActive = true,
                UpdatedAt = date
            };
            await employeeRepo.CreateAsync(entity, cancellationToken);
            return new BaseResponse<bool>
            {
                Data = true
            };
        }
        catch (OperationCanceledException)
        {
            // Preserve cooperative cancellation semantics
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error create employee: {Error}", e.Message);
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