using FluentValidation;

namespace InventoryManagement.Api.Models;

public sealed record UpdateProductRequest(string? Name, decimal? Price);

public sealed class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(p => p.Name)
            .MinimumLength(3)
            .MaximumLength(100)
            .When(p => !string.IsNullOrWhiteSpace(p.Name));

        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(0)
            .When(p => p.Price is not null || p.Price.HasValue);
    }
}
