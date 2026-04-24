using InventoryManagement.Api.ApiResponseResult;
using InventoryManagement.Api.Models;
using InventoryManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet]
    public async Task<IActionResult> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        var response = await _productService.GetAllProductsAsync(cancellationToken);
        return response.ToControllerResponse(HttpContext);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var response = await _productService.GetProductByIdAsync(id, cancellationToken);
        return response.ToControllerResponse(HttpContext);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken
    )
    {
        var response = await _productService.CreateproductAsync(request, cancellationToken);
        return response.ToControllerResponse(HttpContext);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProductAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken cancellationToken
    )
    {
        var response = await _productService.UpdateproductAsync(id, request, cancellationToken);
        return response.ToControllerResponse(HttpContext);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProductAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var response = await _productService.DeleteProductAsync(id, cancellationToken);
        return response.ToControllerResponse(HttpContext);
    }

    [HttpPost("{id:guid}/stock")]
    public async Task<IActionResult> CreateInventoryRecordAsync(
        [FromRoute] Guid id,
        [FromBody] CreateInventoryRequest request,
        CancellationToken cancellationToken
    )
    {
        var response = await _productService.CreateInventoryAsync(id, request, cancellationToken);
        return response.ToControllerResponse(HttpContext);
    }

    [HttpGet("{productId:guid}/stock/history")]
    public async Task<IActionResult> GetProductInventoryHistoryAsync(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken
    )
    {
        var response = await _productService.GetProductInventoryHistoryAsync(
            productId,
            cancellationToken
        );
        return response.ToControllerResponse(HttpContext);
    }
}
