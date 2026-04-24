namespace InventoryManagement.Api.Models;

public sealed record PagedRequest(int PageNumber = 1, int PageSize = 50);
