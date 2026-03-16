using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Inventory;

namespace DomnerTech.Backend.Application.Features.Inventory;

/// <summary>
/// Command to adjust stock quantity.
/// </summary>
public sealed record AdjustStockCommand(AdjustStockReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    IValidatableRequest;
