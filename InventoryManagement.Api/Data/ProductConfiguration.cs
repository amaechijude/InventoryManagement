using InventoryManagement.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Api.Data;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.SKU).IsUnique();
        builder.Property(p => p.SKU).IsRequired().HasMaxLength(512);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Price).HasPrecision(16, 2);

        builder.HasQueryFilter(p => !p.IsDeleted);

        // seed data
        builder.HasData(
            new
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567001"),
                Name = "Wireless Mouse",
                SKU = "WM-001",
                Price = 29.99m,
                StockLevel = 150,
                IsDeleted = false,
                CreatedAt = new DateTimeOffset(2024, 1, 10, 9, 0, 0, TimeSpan.Zero),
            },
            new
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567002"),
                Name = "Mechanical Keyboard",
                SKU = "MK-002",
                Price = 89.99m,
                StockLevel = 75,
                IsDeleted = false,
                CreatedAt = new DateTimeOffset(2024, 1, 12, 10, 30, 0, TimeSpan.Zero),
            },
            new
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567003"),
                Name = "USB-C Hub",
                SKU = "UH-003",
                Price = 49.99m,
                StockLevel = 200,
                IsDeleted = false,
                CreatedAt = new DateTimeOffset(2024, 1, 15, 8, 0, 0, TimeSpan.Zero),
            },
            new
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567004"),
                Name = "27\" Monitor",
                SKU = "MN-004",
                Price = 349.99m,
                StockLevel = 40,
                IsDeleted = false,
                CreatedAt = new DateTimeOffset(2024, 2, 1, 11, 0, 0, TimeSpan.Zero),
            },
            new
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567005"),
                Name = "Laptop Stand",
                SKU = "LS-005",
                Price = 39.99m,
                StockLevel = 120,
                IsDeleted = false,
                CreatedAt = new DateTimeOffset(2024, 2, 5, 14, 0, 0, TimeSpan.Zero),
            },
            new
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567006"),
                Name = "Webcam 1080p",
                SKU = "WC-006",
                Price = 79.99m,
                StockLevel = 90,
                IsDeleted = false,
                CreatedAt = new DateTimeOffset(2024, 2, 10, 9, 30, 0, TimeSpan.Zero),
            },
            new
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567007"),
                Name = "Noise Cancelling Headset",
                SKU = "NH-007",
                Price = 129.99m,
                StockLevel = 60,
                IsDeleted = false,
                CreatedAt = new DateTimeOffset(2024, 2, 14, 13, 0, 0, TimeSpan.Zero),
            },
            new
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567008"),
                Name = "Desk Lamp",
                SKU = "DL-008",
                Price = 34.99m,
                StockLevel = 180,
                IsDeleted = false,
                CreatedAt = new DateTimeOffset(2024, 3, 1, 8, 0, 0, TimeSpan.Zero),
            },
            new
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567009"),
                Name = "External SSD 1TB",
                SKU = "ES-009",
                Price = 109.99m,
                StockLevel = 55,
                IsDeleted = false,
                CreatedAt = new DateTimeOffset(2024, 3, 8, 10, 0, 0, TimeSpan.Zero),
            },
            new
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567010"),
                Name = "Ergonomic Chair",
                SKU = "EC-010",
                Price = 499.99m,
                StockLevel = 25,
                IsDeleted = false,
                CreatedAt = new DateTimeOffset(2024, 3, 15, 12, 0, 0, TimeSpan.Zero),
            }
        );
    }
}
