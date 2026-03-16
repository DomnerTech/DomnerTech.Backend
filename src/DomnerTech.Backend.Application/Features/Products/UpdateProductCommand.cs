using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;

namespace DomnerTech.Backend.Application.Features.Products;

/// <summary>
/// Command to update an existing product.
/// </summary>
public sealed record UpdateProductCommand(string ProductId, CreateProductReqDto Dto) :
    IRequest<BaseResponse<bool>>,
    ILogCreator,
    IValidatableRequest;
