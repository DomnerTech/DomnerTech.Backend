using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Reports;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Reports.Handlers;

/// <summary>
/// Handler for GetLeaveTrendQuery.
/// </summary>
public sealed class GetLeaveTrendQueryHandler(
    ILogger<GetLeaveTrendQueryHandler> logger,
    ILeaveRequestRepo leaveRequestRepo) : IRequestHandler<GetLeaveTrendQuery, BaseResponse<List<LeaveTrendDto>>>
{
    public async Task<BaseResponse<List<LeaveTrendDto>>> Handle(GetLeaveTrendQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all requests for the year
            var approvedRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Approved, cancellationToken);
            var rejectedRequests = await leaveRequestRepo.GetByStatusAsync(LeaveRequestStatus.Rejected, cancellationToken);
            var allRequests = approvedRequests.Concat(rejectedRequests)
                .Where(r => r.Period.StartDate.Year == request.Year || r.Period.EndDate.Year == request.Year)
                .ToList();

            // Group by month
            var monthlyData = new Dictionary<int, LeaveTrendDto>();

            for (var month = 1; month <= 12; month++)
            {
                monthlyData[month] = new LeaveTrendDto
                {
                    Month = new DateTime(request.Year, month, 1).ToString("MMMM"),
                    Year = request.Year,
                    TotalRequests = 0,
                    TotalDays = 0,
                    ApprovedCount = 0,
                    RejectedCount = 0
                };
            }

            foreach (var leaveRequest in allRequests)
            {
                var requestMonth = leaveRequest.Period.StartDate.Month;
                
                if (leaveRequest.Period.StartDate.Year == request.Year)
                {
                    monthlyData[requestMonth].TotalRequests++;
                    monthlyData[requestMonth].TotalDays += leaveRequest.Period.TotalDays;

                    switch (leaveRequest.Status)
                    {
                        case LeaveRequestStatus.Approved:
                            monthlyData[requestMonth].ApprovedCount++;
                            break;
                        case LeaveRequestStatus.Rejected:
                            monthlyData[requestMonth].RejectedCount++;
                            break;
                    }
                }
            }

            var trends = monthlyData.OrderBy(m => m.Key).Select(m => m.Value).ToList();

            return new BaseResponse<List<LeaveTrendDto>> { Data = trends };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting leave trends: {Error}", e.Message);
        }

        return new BaseResponse<List<LeaveTrendDto>>
        {
            Data = [],
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
