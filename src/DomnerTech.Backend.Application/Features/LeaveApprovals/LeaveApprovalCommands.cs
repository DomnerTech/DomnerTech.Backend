using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveApprovals;

namespace DomnerTech.Backend.Application.Features.LeaveApprovals;

public sealed record ApproveLeaveCommand(ApproveLeaveReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;

public sealed record RejectLeaveCommand(RejectLeaveReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;

public sealed record GetPendingApprovalsQuery :
    IRequest<BaseResponse<List<LeaveApprovalDto>>>,
    ILogCreator;

public sealed record GetLeaveApprovalHistoryQuery(string LeaveRequestId) :
    IRequest<BaseResponse<List<LeaveApprovalDto>>>,
    ILogCreator,
    IValidatableRequest;
