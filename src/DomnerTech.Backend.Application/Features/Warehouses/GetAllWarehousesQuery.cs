using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Inventory;

namespace DomnerTech.Backend.Application.Features.Warehouses;

/// <summary>
/// Query to get all warehouses.
/// </summary>
public sealed record GetAllWarehousesQuery(bool ActiveOnly = true) :
    IRequest<BaseResponse<List<WarehouseDto>>>;
