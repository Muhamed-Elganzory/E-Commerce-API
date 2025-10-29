namespace DomainLayer.Exceptions.NotFound;

/// <summary>
///     Represents a base exception that should be thrown when a requested entity or resource is not found.
/// </summary>
/// <param name="message">The error message that describes the reason for the exception.</param>
public abstract class NotFoundException (string message) : Exception(message)
{
}
