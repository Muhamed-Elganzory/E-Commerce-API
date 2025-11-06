namespace DomainLayer.Exceptions.Auth
{
    /// <summary>
    ///     Represents an exception that occurs when a request fails validation.
    ///     Typically, thrown when user registration or login inputs contain errors.
    /// </summary>
    /// <remarks>
    ///     This exception is commonly used to return a list of validation or identity
    ///     errors to the API client in a structured way.
    /// </remarks>
    /// <param name="errors">A collection of validation error messages.</param>
    public sealed class BadRequestException(List<string> errors) : Exception("Validation Failed")
    {
        /// <summary>
        ///     Gets the list of validation or identity errors that caused the bad request.
        /// </summary>
        public List<string> Errors { get; } = errors;
    }
}