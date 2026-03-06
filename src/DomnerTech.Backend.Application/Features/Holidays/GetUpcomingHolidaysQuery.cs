using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Holidays;

namespace DomnerTech.Backend.Application.Features.Holidays;

/// <summary>
/// Query to get upcoming holidays.
/// </summary>
public sealed record GetUpcomingHolidaysQuery(int Count = 10) :
    IRequest<BaseResponse<IEnumerable<HolidayDto>>>,
    ILogCreator;
