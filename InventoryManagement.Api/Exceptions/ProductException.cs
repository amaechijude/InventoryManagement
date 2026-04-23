namespace InventoryManagement.Api.Exceptions;

public sealed class InvalidProductPriceException(string Message) : Exception(Message);

public sealed class InvalidProductStockException(string Message) : Exception(Message);
