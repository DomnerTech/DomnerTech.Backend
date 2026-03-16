using Bas24.CommandQuery;
using DomnerTech.Backend.Application.DTOs;
using DomnerTech.Backend.Application.Errors;
using DomnerTech.Backend.Application.Extensions;
using DomnerTech.Backend.Application.IRepo;
using DomnerTech.Backend.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DomnerTech.Backend.Application.Features.Products.Handlers;

/// <summary>
/// Handler for deleting a product (soft delete).
/// </summary>
public sealed class DeleteProductCommandHandler(
    ILogger<DeleteProductCommandHandler> logger,
    IProductRepo productRepo,
    IProductService productService,
    IHttpContextAccessor accessor) : IRequestHandler<DeleteProductCommand, BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var productId = request.ProductId.ToObjectId();

            // Check if product exists
            var product = await productRepo.GetByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorCode = ErrorCodes.SystemError,
                        Desc = "Product not found"
                    }
                };
            }

            // Check if product can be deleted
            var canDelete = await productService.CanDeleteProductAsync(productId, cancellationToken);
            if (!canDelete)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Status = new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorCode = ErrorCodes.SystemError,
                        Desc = "Product cannot be deleted. It has active transactions or stock."
                    }
                };
            }

            var deletedBy = accessor.HttpContext!.GetUserId().ToObjectId();
            await productRepo.DeleteAsync(productId, deletedBy, cancellationToken);

            logger.LogInformation("Product deleted successfully: {ProductId}", productId);

            return new BaseResponse<bool>
            {
                Data = true
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting product: {Error}", e.Message);
        }

        return new BaseResponse<bool>
        {
            Data = false,
            Status = new ResponseStatus
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorCode = ErrorCodes.SystemError
            }
        };
    }
}
