using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Holidays;

namespace DomnerTech.Backend.Application.Features.Holidays;

/// <summary>
/// Command to update an existing holiday.
/// </summary>
public sealed record UpdateHolidayCommand(UpdateHolidayReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;
