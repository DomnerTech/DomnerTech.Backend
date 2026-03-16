using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;

namespace DomnerTech.Backend.Application.Features.Products;

/// <summary>
/// Command to create a new product.
/// </summary>
public sealed record CreateProductCommand(CreateProductReqDto Dto) :
    IRequest<BaseResponse<string>>,
    ILogCreator,
    IValidatableRequest;
