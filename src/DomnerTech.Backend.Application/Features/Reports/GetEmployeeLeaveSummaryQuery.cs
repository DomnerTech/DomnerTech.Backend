using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Reports;

namespace DomnerTech.Backend.Application.Features.Reports;

public sealed record GetEmployeeLeaveSummaryQuery(string EmployeeId, int Year) :
    IRequest<BaseResponse<EmployeeLeaveSummaryDto>>,
    ILogCreator,
    IValidatableRequest;
