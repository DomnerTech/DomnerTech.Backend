using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.Features.Holidays;

/// <summary>
/// Command to delete a holiday.
/// </summary>
public sealed record DeleteHolidayCommand(string Id) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;
