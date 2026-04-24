using System.ComponentModel.DataAnnotations;
using InventoryManagement.Api.Exceptions;

namespace InventoryManagement.Api.Domain;

public class Product
{
    public Guid Id { get; private init; }
    public string Name { get; private set; } = string.Empty;
    public string SKU { get; private set; } = string.Empty;

    [Range(1, double.MaxValue)] public decimal Price { get; private set; }

    [Range(0, int.MaxValue)] public int StockLevel { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private init; }
    public ICollection<InventoryRecord> InventoryRecords { get; private set; } = [];

    public static Product Create(Models.CreateProductRequest request)
    {
        if (request.Price <= 0)
            throw new InvalidProductPriceException("Product price cannot be zero or negative");

        if (request.StockLevel <= 0)
            throw new InvalidProductStockException("At least one product should be in stock");

        return new Product
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            SKU = GenerateSku(request.Name),
            Price = request.Price,
            StockLevel = request.StockLevel,
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }

    public void Updateproduct(string? name, decimal? price)
    {
        if (price is > 0)
            Price = (decimal)price;

        if (!string.IsNullOrWhiteSpace(name) && name.Length is >= 3 and < 100)
            Name = name;
    }

    public void Delete() => IsDeleted = true;

    public void UpdateStockLevel(int newStockLevel)
    {
        if (newStockLevel < 0)
            return;
        StockLevel = newStockLevel;
    }

    private static string GenerateSku(string name)
    {
        var suffix = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        return $"{name.ToUpperInvariant()}-{suffix}";
    }
}
