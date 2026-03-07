using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Reports;

namespace DomnerTech.Backend.Application.Features.Reports;

public sealed record GetLeaveUsageReportQuery(int Year, string? Department = null) :
    IRequest<BaseResponse<List<LeaveUsageReportDto>>>,
    ILogCreator,
    IValidatableRequest;