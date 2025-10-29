using DomainLayer.Exceptions.NotFound;
using Shared.Errors;

namespace E_Commerce.Middleware.Exceptions;

/// <summary>
///     Custom middleware that provides a global exception handling mechanism for the application.
///     It catches unhandled exceptions, logs them, and returns a standardized JSON error response.
///     It also handles 404 "Not Found" endpoints gracefully by returning a consistent JSON response.
/// </summary>
/// <param name="next">The next middleware component in the request pipeline.</param>
/// <param name="logger">The logger instance used for capturing exception details.</param>
public class CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
{
    /// <summary>
    ///     Represents the next middleware component in the HTTP request pipeline.
    /// </summary>
    private readonly RequestDelegate _next = next;

    /// <summary>
    ///     Provides logging capabilities for capturing exception details.
    /// </summary>
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger = logger;

    /// <summary>
    ///     Intercepts incoming HTTP requests to handle any unhandled exceptions.
    ///     Logs the error and returns a standardized JSON response containing
    ///     the HTTP status code and a human-readable error message.
    /// </summary>
    /// <param name="httpContext">The current HTTP context for the incoming request.</param>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // Pass the HTTP context (the current request) to the next middleware component
            await _next.Invoke(httpContext);

            // Check and handle the "Not Found" (404) endpoint scenario
            await HandleNotFoundEndpointAsync(httpContext);
        }
        catch (Exception ex)
        {
            // Log the exception details
            _logger.LogError(ex, "An unhandled exception occurred.");

            // Handle and return a proper error response to the client
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    /// <summary>
    ///     Handles requests where the requested endpoint does not exist (404).
    ///     Returns a consistent JSON error response instead of the default HTML response.
    /// </summary>
    /// <param name="httpContext">The current HTTP context containing request and response data.</param>
    private static async Task HandleNotFoundEndpointAsync(HttpContext httpContext)
    {
        // Check if the requested endpoint was not found (HTTP 404)
        if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound && !httpContext.Response.HasStarted)
        {
            // Prepare a structured error response
            var response = new ErrorToReturn
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"The endpoint '{httpContext.Request.Path}' was not found."
            };

            // Set the response content type to JSON
            httpContext.Response.ContentType = "application/json";

            // Write the JSON response to the client
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }

    /// <summary>
    ///     Handles unhandled exceptions that occur within the request pipeline.
    ///     Determines the appropriate HTTP status code based on the exception type
    ///     and returns a standardized JSON error response.
    /// </summary>
    /// <param name="httpContext">The current HTTP context associated with the request.</param>
    /// <param name="exception">The exception that was thrown during request processing.</param>
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        // Determine the appropriate HTTP status code based on the exception type
        httpContext.Response.StatusCode = exception switch
        {
            // Case 1: Custom Not Found Exception
            NotFoundException => StatusCodes.Status404NotFound,

            // Default: Internal Server Error (500)
            _ => StatusCodes.Status500InternalServerError
        };

        // Set the response content type to JSON
        httpContext.Response.ContentType = "application/json";

        // Create a standardized error response object
        var response = new ErrorToReturn
        {
            StatusCode = httpContext.Response.StatusCode,
            ErrorMessage = exception.Message
        };

        // Serialize and write the error response as JSON
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}