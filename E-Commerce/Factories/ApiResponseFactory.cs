using Microsoft.AspNetCore.Mvc;
using Shared.Errors;

namespace E_Commerce.Factories;

/// <summary>
///     Provides a centralized factory for generating standardized API responses.
///     Specifically, this class is responsible for creating formatted responses
///     for validation errors when the model state is invalid.
/// </summary>
public static class ApiResponseFactory
{
    /// <summary>
    ///     Generates a standardized validation error response when model validation fails.
    ///     This method collects all validation errors from the ModelState and returns
    ///     them in a structured JSON format with a 400 Bad Request status code.
    /// </summary>
    /// <param name="context">
    ///     The <see cref="ActionContext"/> containing details about the current request
    ///     and the validation state of the model.
    /// </param>
    /// <returns>
    ///     An <see cref="IActionResult"/> containing a <see cref="ValidationErrorToReturn"/> object
    ///     with all validation errors, returned as a 400 Bad Request response.
    /// </returns>
    public static IActionResult GenerateApiValidationErrorResponse(ActionContext context)
    {
        // Extract all validation errors from the ModelState
        var errors = context.ModelState

            // Keep only entries that actually contain errors
            .Where(er => er.Value!.Errors.Any())

            // Convert each invalid field into a ValidationError object
            .Select(er => new ValidationError()
            {
                // The name of the field that failed validation
                Field = er.Key,

                // The list of error messages associated with that field
                Errors = er.Value!.Errors.Select(e => e.ErrorMessage)
            });

        // Create a standardized error response object that wraps all validation errors
        var response = new ValidationErrorToReturn()
        {
            ValidationErrors = errors
        };

        // Return a 400 Bad Request response containing the validation details in JSON format
        return new BadRequestObjectResult(response);
    }
}