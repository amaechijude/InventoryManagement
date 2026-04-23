namespace InventoryManagement.Api.Models;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string SKU,
    decimal Price,
    int StockLevel
);
