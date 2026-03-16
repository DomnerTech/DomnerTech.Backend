using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Inventory;

namespace DomnerTech.Backend.Application.Features.Inventory;

/// <summary>
/// Query to get stock information for a product.
/// </summary>
public sealed record GetStockByProductQuery(
    string ProductId,
    string? WarehouseId = null,
    string? VariantId = null) :
    IRequest<BaseResponse<List<StockDto>>>;
