using InventoryManagement.Api.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Tests;

public class CustomFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(service =>
        {
            // Remove ALL EF Core descriptors — including provider-specific
            // infrastructure (e.g. SQLite's IDatabaseProvider) — so that only
            // the InMemory provider we register below remains in the container.
            var descriptorsToRemove = service
                .Where(s =>
                    s.ServiceType == typeof(InventoryDbContext) ||
                    (s.ServiceType.FullName != null &&
                     s.ServiceType.FullName.Contains("EntityFrameworkCore")))
                .ToList();

            foreach (var descriptor in descriptorsToRemove)
                service.Remove(descriptor);

            // swap with in memory db
            service.AddDbContext<InventoryDbContext>(s =>
                s.UseInMemoryDatabase(databaseName: "test")
            );
        });
    }
}
