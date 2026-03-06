using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Dashboard;

namespace DomnerTech.Backend.Application.Features.Dashboard;

/// <summary>
/// Query to get employees currently on leave.
/// </summary>
public sealed record GetEmployeesOnLeaveQuery :
    IRequest<BaseResponse<List<EmployeeOnLeaveDto>>>,
    ILogCreator;