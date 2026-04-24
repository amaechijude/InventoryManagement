namespace InventoryManagement.Api.Models;

public sealed record InventoryEntry(
    Guid Id,
    Guid ProductId,
    int QuantityChanged,
    string Reason,
    DateTimeOffset RecordedAt
);

public sealed record InventoryHistoryResponse(List<InventoryEntry> InventoryEntries);
