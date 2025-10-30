using System.Net;

namespace Shared.Errors;

/// <summary>
///     Represents a standardized response object for validation-related errors.
///     Used when a request fails due to invalid input or model validation issues.
/// </summary>
public class ValidationErrorToReturn
{
    /// <summary>
    ///     Gets or sets the HTTP status code for the validation error.
    ///     Defaults to <see cref="HttpStatusCode.BadRequest"/> (400).
    /// </summary>
    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;

    /// <summary>
    ///     Gets or sets a short, descriptive error message for the validation failure.
    ///     Defaults to "Validation Failed".
    /// </summary>
    public string ErrorMessage { get; set; } = "Validation Failed";

    /// <summary>
    ///     Gets or sets a collection of detailed validation errors.
    ///     Each <see cref="ValidationError"/> provides information about a specific invalid field.
    /// </summary>
    public IEnumerable<ValidationError> ValidationErrors { get; set; } = [];
}