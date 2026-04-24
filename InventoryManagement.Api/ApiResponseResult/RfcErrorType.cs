namespace InventoryManagement.Api.ApiResponseResult;

public static class RfcErrorType
{
    public static string GetProblemType(int statusCode) =>
        statusCode switch
        {
            400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1", // Bad Request
            401 => "https://tools.ietf.org/html/rfc9110#section-15.5.2", // Unauthorized
            402 => "https://tools.ietf.org/html/rfc9110#section-15.5.3", // Payment Required
            403 => "https://tools.ietf.org/html/rfc9110#section-15.5.4", // Forbidden
            404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5", // Not Found
            405 => "https://tools.ietf.org/html/rfc9110#section-15.5.6", // Method Not Allowed
            406 => "https://tools.ietf.org/html/rfc9110#section-15.5.7", // Not Acceptable
            407 => "https://tools.ietf.org/html/rfc9110#section-15.5.8", // Proxy Authentication Required
            408 => "https://tools.ietf.org/html/rfc9110#section-15.5.9", // Request Timeout
            409 => "https://tools.ietf.org/html/rfc9110#section-15.5.10", // Conflict
            410 => "https://tools.ietf.org/html/rfc9110#section-15.5.11", // Gone
            411 => "https://tools.ietf.org/html/rfc9110#section-15.5.12", // Length Required
            412 => "https://tools.ietf.org/html/rfc9110#section-15.5.13", // Precondition Failed
            413 => "https://tools.ietf.org/html/rfc9110#section-15.5.14", // Content Too Large
            414 => "https://tools.ietf.org/html/rfc9110#section-15.5.15", // URI Too Long
            415 => "https://tools.ietf.org/html/rfc9110#section-15.5.16", // Unsupported Media Type
            416 => "https://tools.ietf.org/html/rfc9110#section-15.5.17", // Range Not Satisfiable
            417 => "https://tools.ietf.org/html/rfc9110#section-15.5.18", // Expectation Failed
            421 => "https://tools.ietf.org/html/rfc9110#section-15.5.20", // Misdirected Request
            422 => "https://tools.ietf.org/html/rfc9110#section-15.5.21", // Unprocessable Content
            423 => "https://tools.ietf.org/html/rfc4918#section-11.3", // Locked (WebDAV)
            424 => "https://tools.ietf.org/html/rfc4918#section-11.4", // Failed Dependency (WebDAV)
            425 => "https://tools.ietf.org/html/rfc8470#section-5.2", // Too Early
            426 => "https://tools.ietf.org/html/rfc9110#section-15.5.22", // Upgrade Required
            428 => "https://tools.ietf.org/html/rfc6585#section-3", // Precondition Required
            429 => "https://tools.ietf.org/html/rfc6585#section-4", // Too Many Requests
            431 => "https://tools.ietf.org/html/rfc6585#section-5", // Request Header Fields Too Large
            451 => "https://tools.ietf.org/html/rfc7725#section-3", // Unavailable For Legal Reasons
            500 => "https://tools.ietf.org/html/rfc9110#section-15.6.1", // Internal Server Error
            501 => "https://tools.ietf.org/html/rfc9110#section-15.6.2", // Not Implemented
            502 => "https://tools.ietf.org/html/rfc9110#section-15.6.3", // Bad Gateway
            503 => "https://tools.ietf.org/html/rfc9110#section-15.6.4", // Service Unavailable
            504 => "https://tools.ietf.org/html/rfc9110#section-15.6.5", // Gateway Timeout
            505 => "https://tools.ietf.org/html/rfc9110#section-15.6.6", // HTTP Version Not Supported
            _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1", // fallback → 500
        };
}
