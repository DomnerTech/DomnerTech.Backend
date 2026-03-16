using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;

namespace DomnerTech.Backend.Application.Features.Products;

/// <summary>
/// Command to delete (soft delete) a product.
/// </summary>
public sealed record DeleteProductCommand(string ProductId) :
    IRequest<BaseResponse<bool>>,
    ILogCreator;
