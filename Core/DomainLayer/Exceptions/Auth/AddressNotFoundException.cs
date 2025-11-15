using DomainLayer.Exceptions.NotFound;

namespace DomainLayer.Exceptions.Auth
{
    /// <summary>
    ///     Exception that is thrown when a user exists but does not have an associated address record.
    /// </summary>
    /// <param name="userName">
    ///     The username or email of the user for whom the address could not be found.
    /// </param>
    public sealed class AddressNotFoundException(string userName) : NotFoundException($"User '{userName}' has no associated address.")
    {
    }
}