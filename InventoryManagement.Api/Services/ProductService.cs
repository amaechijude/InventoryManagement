using InventoryManagement.Api.ApiResponseResult;
using InventoryManagement.Api.Domain;
using InventoryManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Api.Services;

public class ProductService(InventoryDbContext dbContext) : IProductService
{
    private readonly InventoryDbContext _dbContext = dbContext;

    public async Task<ApiResponse<List<ProductResponse>>> GetAllProductsAsync(
        CancellationToken cancellationToken
    )
    {
        var products = await _dbContext
            .Products.AsNoTracking()
            .Select(p => new ProductResponse(p.Id, p.Name, p.SKU, p.Price, p.StockLevel))
            .ToListAsync(cancellationToken);

        products ??= [];

        return ApiResponse<List<ProductResponse>>.Success(products);
    }

    public async Task<ApiResponse<ProductResponse>> GetProductByIdAsync(
        Guid id,
        CancellationToken cancellationToken
    )
    {
        var product = await _dbContext
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
        var Validator = new CreateProductValidator();

        var validationResult = await Validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return ApiResponse<ProductResponse>.ValidationError(validationResult.Errors);

        var existingProduct = await _dbContext.Products.AnyAsync(
            p => p.Name == request.Name,
            cancellationToken
        );

        if (existingProduct)
            return ApiResponse<ProductResponse>.Conflict("Product with name alredy exists");

        var newProduct = Product.Create(request);
        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync(cancellationToken);

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
        var Validator = new UpdateProductRequestValidator();

        var validationResult = await Validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return ApiResponse<ProductResponse>.ValidationError(validationResult.Errors);

        var product = await _dbContext.Products.FindAsync(
            [productId],
            cancellationToken: cancellationToken
        );

        if (product is null)
            return ApiResponse<ProductResponse>.NotFound("Product not found");

        product.Updateproduct(request.Name, request.Price);
        await _dbContext.SaveChangesAsync(cancellationToken);

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
        await _dbContext
            .Products.Where(p => p.Id == id)
            .ExecuteUpdateAsync(
                s => s.SetProperty(p => p.IsDeleted, true),
                cancellationToken: cancellationToken
            );
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

        var product = await _dbContext.Products.FindAsync([productId], cancellationToken);

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

        _dbContext.InventoryRecords.Add(newInventory);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<CreateInventoryResponse>.Success(
            new CreateInventoryResponse(productId, request.QuantityChanged)
        );
    }

    public async Task<ApiResponse<InventoryHistoryResponse>> GetProductInventoryHistoryAsync(
        Guid productId,
        CancellationToken cancellationToken
    )
    {
        var entries = await _dbContext
            .InventoryRecords.AsNoTracking()
            .Where(r => r.ProductId == productId)
            .Select(s => new InventoryEntry(
                s.Id,
                s.ProductId,
                s.QuantityChanged,
                s.Reason,
                s.RecordedAt
            ))
            .ToListAsync(cancellationToken);

        entries ??= [];
        return ApiResponse<InventoryHistoryResponse>.Success(new InventoryHistoryResponse(entries));
    }
}
