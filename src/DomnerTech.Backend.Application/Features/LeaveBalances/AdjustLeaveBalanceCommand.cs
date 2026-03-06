using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveBalances;

namespace DomnerTech.Backend.Application.Features.LeaveBalances;

public sealed record AdjustLeaveBalanceCommand(AdjustLeaveBalanceReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;