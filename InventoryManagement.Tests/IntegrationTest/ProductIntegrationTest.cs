using System.Net.Http.Json;
using InventoryManagement.Api.Domain;
using InventoryManagement.Api.Models;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Tests.IntegrationTest;

public class ProductIntegrationTest : IClassFixture<CustomFactory>
{
    private readonly InventoryDbContext _context;
    private readonly HttpClient _client;
    private readonly Guid _productId;

    private const string Route = "/api/products";

    public ProductIntegrationTest(CustomFactory factory)
    {
        _client = factory.CreateClient();
        var scope = factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        _context.Database.EnsureCreated();
        // seed product
        var testProduct1 = Product.Create(new CreateProductRequest("Product1", 490m, 223));
        _context.Products.Add(testProduct1);
        _context.SaveChanges();

        _productId = testProduct1.Id;
    }

    [Fact]
    public async Task ListProducts_Always_Succeds()
    {
        var response = await _client.GetAsync(Route);
        response.EnsureSuccessStatusCode();
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetProduct_Returns_Product_When_It_Exists()
    {
        var response = await _client.GetAsync($"{Route}/{_productId}");
        response.EnsureSuccessStatusCode();
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Create_Product_Succeds_With_Valid_Data()
    {
        var body = new CreateProductRequest("Product2", 40m, 22);
        var response = await _client.PostAsJsonAsync(Route, body);
        response.EnsureSuccessStatusCode();
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Update_Product_Succeds_WIth_Valid_Data()
    {
        var body = new UpdateProductRequest("update", 890m);
        var response = await _client.PutAsJsonAsync($"{Route}/{_productId}", body);

        response.EnsureSuccessStatusCode();
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Delete_Product_Returns_NoContent()
    {
        var response = await _client.DeleteAsync($"{Route}/{_productId}");

        response.EnsureSuccessStatusCode();
        Assert.True(response.IsSuccessStatusCode);
    }
}
