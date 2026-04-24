namespace InventoryManagement.Api.Domain;

public class InventoryRecord
{
    public Guid Id { get; private init; }
    public Guid ProductId { get; private init; }
    public Product? Product { get; set; }
    public int QuantityChanged { get; private init; }
    public string Reason { get; private set; } = string.Empty;
    public DateTimeOffset RecordedAt { get; private init; }

    internal static InventoryRecord Create(Guid productId, int quantityChanged, string reason)
    {
        return new InventoryRecord
        {
            Id = Guid.CreateVersion7(),
            ProductId = productId,
            QuantityChanged = quantityChanged,
            Reason = reason,
            RecordedAt = DateTimeOffset.UtcNow,
        };
    }
}
