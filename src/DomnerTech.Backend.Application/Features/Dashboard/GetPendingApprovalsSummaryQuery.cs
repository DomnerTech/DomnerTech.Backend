using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Dashboard;

namespace DomnerTech.Backend.Application.Features.Dashboard;

/// <summary>
/// Query to get pending approvals summary.
/// </summary>
public sealed record GetPendingApprovalsSummaryQuery :
    IRequest<BaseResponse<List<PendingApprovalSummaryDto>>>,
    ILogCreator;
