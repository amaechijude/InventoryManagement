using InventoryManagement.Api.ApiResponseResult;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Exceptions;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        logger.LogError(
            "An Exception Occured on {timestamp} {traceId} {path}",
            DateTimeOffset.UtcNow,
            httpContext.TraceIdentifier,
            httpContext.Request.Path
        );

        var (statusCode, title, detail) = GetExceptionInfo(exception);

        ProblemDetails problemDetails = new()
        {
            Title = title,
            Detail = detail,
            Status = statusCode,
            Type = RfcErrorType.GetProblemType(statusCode),
            Instance = httpContext.Request.Path,
            Extensions =
            {
                ["traceId"] = httpContext.TraceIdentifier,
                ["timestamp"] = DateTimeOffset.UtcNow,
            },
        };

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext { ProblemDetails = problemDetails, HttpContext = httpContext }
        );
    }

    private static (int statusCode, string title, string details) GetExceptionInfo(
        Exception exception
    )
    {
        return exception switch
        {
            InvalidProductPriceException => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                exception.Message
            ),
            InvalidProductStockException => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                exception.Message
            ),
            BadHttpRequestException => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                "Invalid request"
            ),
            _ => (StatusCodes.Status500InternalServerError, "Server Error", "An error occured"),
        };
    }
}
