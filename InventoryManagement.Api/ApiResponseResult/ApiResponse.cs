using FluentValidation.Results;
using InventoryManagement.Api.Models;
using Microsoft.AspNetCore.Authentication;

namespace InventoryManagement.Api.ApiResponseResult;

public sealed class ApiResponse<T>
{
    public bool IsSucces { get; }
    public T? Data { get; }
    public ApiError? Error { get; }

    private ApiResponse(T data)
    {
        IsSucces = true;
        Data = data;
    }

    private ApiResponse()
    {
        IsSucces = true;
    }

    private ApiResponse(ApiError error)
    {
        IsSucces = false;
        Error = error;
    }

    public static ApiResponse<T> Success(T data) => new(data);

    public static ApiResponse<T> NoContent() => new();

    // errors
    private static ApiResponse<T> Failure(ApiError error) => new(error);

    // bad request
    public static ApiResponse<T> BadRequest(string message) =>
        Failure(new ApiError("Bad Request", message, StatusCodes.Status400BadRequest));

    public static ApiResponse<T> NotFound(string message) =>
        Failure(new ApiError("Not Found", message, StatusCodes.Status404NotFound));

    public static ApiResponse<T> Conflict(string message) =>
        Failure(new ApiError("Conflict Request", message, StatusCodes.Status409Conflict));

    public static ApiResponse<T> ValidationError(List<ValidationFailure> validationFailures)
    {
        var errorFields = validationFailures
            .GroupBy(e => e.PropertyName)
            .ToDictionary(e => e.Key, e => e.Select(m => m.ErrorMessage).ToArray());
        return Failure(
            new ApiError(
                "Validation Error",
                "One Or more validation Failed",
                StatusCodes.Status422UnprocessableEntity,
                errorFields
            )
        );
    }
}

public sealed record ApiError(
    string Code,
    string Message,
    int StatusCode,
    Dictionary<string, string[]>? Fields = null
);
