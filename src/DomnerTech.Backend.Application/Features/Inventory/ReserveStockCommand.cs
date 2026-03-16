using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.Features.Inventory;

/// <summary>
/// Command to reserve stock for an order.
/// </summary>
public sealed record ReserveStockCommand(
    string ProductId,
    string WarehouseId,
    decimal Quantity,
    string OrderId,
    string? VariantId = null,
    DateTime? ExpiresAt = null) :
    IRequest<BaseResponse<string>>,
    ILogCreator,
    IValidatableRequest;
