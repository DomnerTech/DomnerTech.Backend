using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Leaves.Holidays;

namespace DomnerTech.Backend.Application.Features.Holidays;

/// <summary>
/// Command to bulk create holidays.
/// </summary>
public sealed record BulkCreateHolidaysCommand(BulkCreateHolidaysReqDto Dto) :
    IRequest<BaseResponse<int>>,
    ILogCreator,
    IValidatableRequest;
