using FluentValidation;

namespace InventoryManagement.Api.Models;

public sealed record CreateProductRequest(string Name, decimal Price, int StockLevel);

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MinimumLength(3).MaximumLength(100);

        RuleFor(p => p.Price).NotNull().GreaterThan(0);

        RuleFor(p => p.StockLevel).NotNull().GreaterThan(0);
    }
}
