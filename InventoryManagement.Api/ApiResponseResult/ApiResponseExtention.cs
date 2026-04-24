using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.ApiResponseResult;

public static class ApiResponseExtention
{
    public static IActionResult ToControllerResponse<T>(
        this ApiResponse<T> result,
        HttpContext httpContext
    )
    {
        if (result.IsSucces)
        {
            return result.Data is null ? new NoContentResult() : new OkObjectResult(result.Data);
        }

        var (problemDetails, statusCode) = CreateProblemDetailsResponse(result.Error!, httpContext);

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }

    private static (ProblemDetails, int) CreateProblemDetailsResponse(
        ApiError error,
        HttpContext httpContext
    )
    {
        var problemDetails = new ProblemDetails
        {
            Title = error.Code,
            Detail = error.Message,
            Status = error.StatusCode,
            Type = RfcErrorType.GetProblemType(error.StatusCode),
            Instance = httpContext.Request.Path,
            Extensions =
            {
                ["traceId"] = httpContext.TraceIdentifier,
                ["timestamp"] = DateTimeOffset.UtcNow,
            },
        };
        if (error.Fields is not null)
            problemDetails.Extensions["errors"] = error.Fields;

        return (problemDetails, error.StatusCode);
    }
}
