using DomainLayer.Exceptions.Auth;
using DomainLayer.Exceptions.NotFound;
using Shared.Errors;

namespace E_Commerce.Middleware.Exceptions;

/// <summary>
///     Middleware that provides centralized exception handling for the entire application.
///     It catches unhandled exceptions, logs them, and returns a standardized JSON response.
///     It also gracefully handles 404 "Not Found" endpoints.
/// </summary>
/// <param name="next">The next middleware component in the request pipeline.</param>
/// <param name="logger">Logger used for capturing exception details.</param>
public class CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger = logger;

    /// <summary>
    ///     Intercepts HTTP requests to handle exceptions globally.
    /// </summary>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);

            // Handle 404 responses after request execution
            await HandleNotFoundEndpointAsync(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    /// <summary>
    ///     Handles cases where the requested endpoint was not found (404).
    /// </summary>
    private static async Task HandleNotFoundEndpointAsync(HttpContext httpContext)
    {
        if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound && !httpContext.Response.HasStarted)
        {
            var response = new ErrorToReturn
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"The endpoint '{httpContext.Request.Path}' was not found."
            };

            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }

    /// <summary>
    ///     Handles all exceptions by mapping them to appropriate HTTP status codes and JSON responses.
    /// </summary>
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var response = new ErrorToReturn();

        // Determine HTTP status code based on exception type
        httpContext.Response.StatusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            BadRequestException badRequestException => GetBadRequestErrors(badRequestException, response),
            _ => StatusCodes.Status500InternalServerError
        };

        response.StatusCode = httpContext.Response.StatusCode;
        response.ErrorMessage = exception.Message;

        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(response);
    }

    /// <summary>
    ///     Extracts validation or request errors from a <see cref="BadRequestException"/> and returns HTTP 400.
    /// </summary>
    /// <param name="badRequestException">The exception containing validation error details.</param>
    /// <param name="errorToReturn">The structured error response to update.</param>
    /// <returns>HTTP status code 400 (Bad Request).</returns>
    private static int GetBadRequestErrors(BadRequestException badRequestException, ErrorToReturn errorToReturn)
    {
        errorToReturn.Errors = badRequestException.Errors;
        return StatusCodes.Status400BadRequest;
    }
}