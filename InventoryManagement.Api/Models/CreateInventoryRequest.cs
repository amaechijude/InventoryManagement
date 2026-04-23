using FluentValidation;

namespace InventoryManagement.Api.Models;

public sealed record CreateInventoryResponse(Guid ProductId, int QuantityChanged)
{
    public string Operation => QuantityChanged < 0 ? "Subtraction" : "Addition";
    private readonly string info = QuantityChanged < 0 ? "subtracted from" : "added to";
    public string Message =>
        $"{Math.Abs(QuantityChanged)} was {info} the initial stock level of the product";
};

public sealed record CreateInventoryRequest(string Reason, int QuantityChanged);

public class CreateInventoryRequestValidator : AbstractValidator<CreateInventoryRequest>
{
    public CreateInventoryRequestValidator()
    {
        RuleFor(ir => ir.Reason).NotEmpty().MaximumLength(1024);
        RuleFor(ir => ir.QuantityChanged).NotNull();
    }
}
