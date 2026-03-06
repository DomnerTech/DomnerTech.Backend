using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.LeaveRequests;

namespace DomnerTech.Backend.Application.Features.LeaveRequests;

public sealed record GetLeaveRequestByIdQuery(string Id) :
    IRequest<BaseResponse<LeaveRequestDetailDto>>,
    ILogCreator,
    IValidatableRequest;