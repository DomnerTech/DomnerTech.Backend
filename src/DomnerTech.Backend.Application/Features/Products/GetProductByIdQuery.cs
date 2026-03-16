using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;
using MongoDB.Bson;

namespace DomnerTech.Backend.Application.Features.Products;

/// <summary>
/// Query to get a product by ID.
/// </summary>
public sealed record GetProductByIdQuery(string ProductId) :
    IRequest<BaseResponse<ProductDto>>;
