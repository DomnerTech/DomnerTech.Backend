using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Inventory;

namespace DomnerTech.Backend.Application.Features.Inventory;

/// <summary>
/// Query to get low stock items.
/// </summary>
public sealed record GetLowStockItemsQuery(string? WarehouseId = null) :
    IRequest<BaseResponse<List<StockDto>>>;
