using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Inventory;

namespace DomnerTech.Backend.Application.Features.Inventory;

/// <summary>
/// Command to create a stock transfer between warehouses.
/// </summary>
public sealed record CreateStockTransferCommand(CreateStockTransferReqDto Dto) :
    IRequest<BaseResponse<string>>,
    ILogCreator,
    IValidatableRequest;
