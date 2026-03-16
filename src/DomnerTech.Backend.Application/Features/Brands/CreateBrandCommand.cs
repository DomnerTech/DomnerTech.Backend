using Bas24.CommandQuery;
using DomnerTech.Backend.Application.Abstractions;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.DTOs.Products;

namespace DomnerTech.Backend.Application.Features.Brands;

/// <summary>
/// Command to create a new brand.
/// </summary>
public sealed record CreateBrandCommand(CreateBrandReqDto Dto) :
    IRequest<BaseResponse<string>>,
    ILogCreator,
    IValidatableRequest;
