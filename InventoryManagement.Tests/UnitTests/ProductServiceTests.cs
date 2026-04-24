using InventoryManagement.Api.Domain;
using InventoryManagement.Api.Models;
using InventoryManagement.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Tests.UnitTests;

public class ProductServiceTests
{
    private readonly InventoryDbContext _context;
    private readonly IProductService _sut;

    public ProductServiceTests()
    {
        var options = new DbContextOptionsBuilder<InventoryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new InventoryDbContext(options);
        _sut = new ProductService(_context);
    }

    [Fact]
    public async Task Create_Product_With_Valid_Data_Should_Succed_And_Persist()
    {
        // Arrange
        var request = new CreateProductRequest("testProduct1", 400m, 45);

        // Act
        var response = await _sut.CreateproductAsync(request, CancellationToken.None);
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == request.Name);

        // Assert
        Assert.True(response.IsSucces);
        Assert.Null(response.Error);
        Assert.NotNull(response.Data);
        Assert.NotNull(product);
    }

    [Fact]
    public async Task Create_Product_With_Invalid_Price_Should_Fail_And_Not_Persist()
    {
        // Arrange
        var request = new CreateProductRequest("testProduct2", -430m, 45);

        // Act
        var response = await _sut.CreateproductAsync(request, CancellationToken.None);
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == request.Name);

        // Assert
        Assert.False(response.IsSucces);
        Assert.Null(response.Data);
        Assert.NotNull(response.Error);
        Assert.Null(product);
    }

    [Fact]
    public async Task Create_Product_With_Invalid_Stock_Should_Fail_And_Not_Persist()
    {
        // Arrange
        var request = new CreateProductRequest("testProduct3", 430m, -45);

        // Act
        var response = await _sut.CreateproductAsync(request, CancellationToken.None);
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == request.Name);

        // Assert
        Assert.False(response.IsSucces);
        Assert.Null(response.Data);
        Assert.NotNull(response.Error);
        Assert.Null(product);
    }

    [Fact]
    public async Task Adding_Stock_Update_The_Quantity_Correctly()
    {
        // Arrange
        var newProduct = new CreateProductRequest("testProduct4", 430m, 29);
        var product = Product.Create(newProduct);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var request = new CreateInventoryRequest("Addintion", 5);

        // Act
        var response = await _sut.CreateInventoryAsync(product.Id, request, CancellationToken.None);
        var currentProduct = await _context.Products.FindAsync(product.Id);

        // Assert
        Assert.True(response.IsSucces);
        Assert.NotNull(currentProduct);
        Assert.Equal(newProduct.StockLevel + request.QuantityChanged, currentProduct.StockLevel);
    }

    [Fact]
    public async Task Removing_Stock_When_Quantity_Would_Go_Negative_Should_Fail()
    {
        // Arrange
        var newProduct = new CreateProductRequest("testProduct5", 430m, 29);
        var product = Product.Create(newProduct);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var request = new CreateInventoryRequest("Subtraction", -55);

        // Act
        var response = await _sut.CreateInventoryAsync(product.Id, request, CancellationToken.None);

        // Assert
        Assert.False(response.IsSucces);
        Assert.NotNull(response.Error);
    }
}
