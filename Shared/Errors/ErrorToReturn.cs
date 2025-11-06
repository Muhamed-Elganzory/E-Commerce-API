namespace Shared.Errors;

/// <summary>
///     Represents a standardized structure for returning error details to the client.
/// </summary>
public class ErrorToReturn
{
    /// <summary>
    ///     Gets or sets the HTTP status code associated with the error response.
    ///     Example: 400 (Bad Request), 404 (Not Found), 500 (Internal Server Error).
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    ///     Gets or sets a descriptive error message that provides details about the failure.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    public List<string>? Errors { get; set; } = null!;
}
