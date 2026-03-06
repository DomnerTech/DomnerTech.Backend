using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveApprovals;

namespace DomnerTech.Backend.Application.Features.LeaveApprovals;

public sealed record ApproveLeaveCommand(ApproveLeaveReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;