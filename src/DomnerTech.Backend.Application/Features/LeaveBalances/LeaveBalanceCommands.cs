using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveBalances;

namespace DomnerTech.Backend.Application.Features.LeaveBalances;

public sealed record InitializeLeaveBalanceCommand(InitializeLeaveBalanceReqDto Dto) :
    IRequest<BaseResponse<string>>,
    ILogCreator,
    IValidatableRequest;

public sealed record AdjustLeaveBalanceCommand(AdjustLeaveBalanceReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;

public sealed record GetMyLeaveBalancesQuery(int Year) :
    IRequest<BaseResponse<List<LeaveBalanceSummaryDto>>>,
    ILogCreator;

public sealed record GetEmployeeLeaveBalancesQuery(string EmployeeId, int Year) :
    IRequest<BaseResponse<List<LeaveBalanceSummaryDto>>>,
    ILogCreator,
    IValidatableRequest;
