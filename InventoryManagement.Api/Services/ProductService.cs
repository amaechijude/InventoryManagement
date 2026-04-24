using InventoryManagement.Api.ApiResponseResult;
using InventoryManagement.Api.Domain;
using InventoryManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Api.Services;

public class ProductService(InventoryDbContext dbContext) : IProductService
{
    private const int MaxPageSize = 100;
    private const int MinPageNumber = 1;

    public async Task<ApiResponse<PagedResponse<ProductResponse>>> GetAllProductsAsync(
        PagedRequest request,
        CancellationToken cancellationToken
    )
    {
        var (pageNumber, pageSize) = GetPageParams(request);

        IQueryable<Product> query = dbContext.Products.AsNoTracking();

        var totalProduct = await query.CountAsync(cancellationToken);

        var products = await query
            .OrderByDescending(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductResponse(p.Id, p.Name, p.SKU, p.Price, p.StockLevel))
            .ToListAsync(cancellationToken);

        var response = new PagedResponse<ProductResponse>(
            products,
            pageNumber,
            pageSize,
            totalProduct
        );
        return ApiResponse<PagedResponse<ProductResponse>>.Success(response);
    }

    public async Task<ApiResponse<ProductResponse>> GetProductByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var product = await dbContext
            .Products.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductResponse(p.Id, p.Name, p.SKU, p.Price, p.StockLevel))
            .FirstOrDefaultAsync(cancellationToken);

        return product is null
            ? ApiResponse<ProductResponse>.NotFound("Product not found")
            : ApiResponse<ProductResponse>.Success(product);
    }

    public async Task<ApiResponse<ProductResponse>> CreateproductAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken
    )
    {
        var validator = new CreateProductValidator();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return ApiResponse<ProductResponse>.ValidationError(validationResult.Errors);

        var existingProduct = await dbContext.Products.AnyAsync(
            p => p.Name == request.Name,
            cancellationToken
        );

        if (existingProduct)
            return ApiResponse<ProductResponse>.Conflict("Product with name alredy exists");

        var newProduct = Product.Create(request);
        dbContext.Products.Add(newProduct);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<ProductResponse>.Success(
            new ProductResponse(
                newProduct.Id,
                newProduct.Name,
                newProduct.SKU,
                newProduct.Price,
                newProduct.StockLevel
            )
        );
    }

    public async Task<ApiResponse<ProductResponse>> UpdateproductAsync(
        Guid productId,
        UpdateProductRequest request,
        CancellationToken cancellationToken
    )
    {
        var validator = new UpdateProductRequestValidator();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return ApiResponse<ProductResponse>.ValidationError(validationResult.Errors);

        var product = await dbContext.Products.FindAsync(
            [productId],
            cancellationToken: cancellationToken
        );

        if (product is null)
            return ApiResponse<ProductResponse>.NotFound("Product not found");

        product.Updateproduct(request.Name, request.Price);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<ProductResponse>.Success(
            new ProductResponse(
                product.Id,
                product.Name,
                product.SKU,
                product.Price,
                product.StockLevel
            )
        );
    }

    public async Task<ApiResponse<int>> DeleteProductAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var product = await dbContext.Products.FindAsync([id], cancellationToken);
        if (product is null)
            return ApiResponse<int>.NoContent();

        product.Delete();
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<int>.NoContent();
    }

    public async Task<ApiResponse<CreateInventoryResponse>> CreateInventoryAsync(
        Guid productId,
        CreateInventoryRequest request,
        CancellationToken cancellationToken
    )
    {
        var validator = new CreateInventoryRequestValidator();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return ApiResponse<CreateInventoryResponse>.ValidationError(validationResult.Errors);

        var product = await dbContext.Products.FindAsync([productId], cancellationToken);

        if (product is null)
            return ApiResponse<CreateInventoryResponse>.NotFound(
                "Cannot create inventory for non exixting product"
            );

        var newStockLevel = product.StockLevel + request.QuantityChanged;
        if (newStockLevel < 0)
            return ApiResponse<CreateInventoryResponse>.BadRequest("Insufficient stock");

        product.UpdateStockLevel(newStockLevel);

        var newInventory = InventoryRecord.Create(
            productId,
            request.QuantityChanged,
            request.Reason
        );

        dbContext.InventoryRecords.Add(newInventory);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<CreateInventoryResponse>.Success(
            new CreateInventoryResponse(productId, request.QuantityChanged)
        );
    }

    public async Task<ApiResponse<PagedResponse<InventoryEntry>>> GetProductInventoryHistoryAsync(
        Guid productId,
        PagedRequest request,
        CancellationToken cancellationToken
    )
    {
        var (pageNumber, pageSize) = GetPageParams(request);

        var query = dbContext.InventoryRecords.AsNoTracking();

        var totalHistory = await query.CountAsync(cancellationToken);

        var entries = await query
            .Where(r => r.ProductId == productId)
            .Select(s => new InventoryEntry(
                s.Id,
                s.ProductId,
                s.QuantityChanged,
                s.Reason,
                s.RecordedAt
            ))
            .ToListAsync(cancellationToken);

        var response = new PagedResponse<InventoryEntry>(
            entries,
            pageNumber,
            pageSize,
            totalHistory
        );
        return ApiResponse<PagedResponse<InventoryEntry>>.Success(response);
    }

    private static (int pageNumber, int pageSize) GetPageParams(PagedRequest request)
    {
        var pageNumber = Math.Max(MinPageNumber, request.PageNumber);
        var pageSize = Math.Min(MaxPageSize, request.PageSize);

        return (pageNumber, pageSize);
    }
}
