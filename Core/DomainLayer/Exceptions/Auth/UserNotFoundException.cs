using DomainLayer.Exceptions.NotFound;

namespace DomainLayer.Exceptions.Auth;

/// <summary>
///     Represents an exception that is thrown when a user with the specified email
///     cannot be found in the system.
/// </summary>
/// <remarks>
///     This exception is typically thrown during authentication or user-related operations
///     (e.g., login, password reset, or profile lookup) when the provided email does not match
///     any existing user record in the database.
/// </remarks>
/// <param name="email">
///     The email address of the user that could not be found.
///     Used to construct a descriptive error message.
/// </param>
public sealed class UserNotFoundException(string email) : NotFoundException($"User with email '{email}' was not found.")
{
}