using InventoryManagement.Api.ApiResponseResult;
using InventoryManagement.Api.Models;

namespace InventoryManagement.Api.Services;

public interface IProductService
{
    Task<ApiResponse<PagedResponse<ProductResponse>>> GetAllProductsAsync(
        PagedRequest request,
        CancellationToken cancellationToken
    );

    Task<ApiResponse<ProductResponse>> GetProductByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    );

    Task<ApiResponse<ProductResponse>> CreateproductAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken
    );

    Task<ApiResponse<ProductResponse>> UpdateproductAsync(
        Guid productId,
        UpdateProductRequest request,
        CancellationToken cancellationToken
    );

    Task<ApiResponse<int>> DeleteProductAsync(Guid id, CancellationToken cancellationToken);

    Task<ApiResponse<CreateInventoryResponse>> CreateInventoryAsync(
        Guid productId,
        CreateInventoryRequest request,
        CancellationToken cancellationToken
    );

    Task<ApiResponse<PagedResponse<InventoryEntry>>> GetProductInventoryHistoryAsync(
        Guid productId,
        PagedRequest request,
        CancellationToken cancellationToken
    );
}
