namespace DomainLayer.Exceptions.Auth;

/// <summary>
///     Represents an exception that is thrown when a user attempts to
///     access a resource or perform an operation without valid authentication credentials.
/// </summary>
/// <remarks>
///     Typically used in authentication or authorization scenarios, such as when a user
///     provides an invalid email or password during login.
/// </remarks>
/// <param name="message">
///     A custom error message that describes the cause of the exception.
///     The default message is "Invalid email or password".
/// </param>
public sealed class UnauthorizedException(string message = "Invalid email or password") : Exception(message)
{
}