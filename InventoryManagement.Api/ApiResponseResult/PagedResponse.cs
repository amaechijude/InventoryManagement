namespace InventoryManagement.Api.ApiResponseResult;

public sealed record PagedResponse<T>(
    IEnumerable<T> Items,
    int PageNumber,
    int PageSize,
    int TotalRecords
)
{
    public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
}
