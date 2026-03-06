using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Holidays;

namespace DomnerTech.Backend.Application.Features.Holidays;

/// <summary>
/// Query to get holidays for a specific year.
/// </summary>
public sealed record GetHolidaysByYearQuery(int Year) :
    IRequest<BaseResponse<IEnumerable<HolidayDto>>>,
    ILogCreator,
    IValidatableRequest;
