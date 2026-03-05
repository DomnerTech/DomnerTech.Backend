using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Holidays;

namespace DomnerTech.Backend.Application.Features.Holidays;

/// <summary>
/// Command to create a new holiday.
/// </summary>
public sealed record CreateHolidayCommand(CreateHolidayReqDto Dto) :
    IRequest<BaseResponse<string>>,
    ILogCreator,
    IValidatableRequest;
