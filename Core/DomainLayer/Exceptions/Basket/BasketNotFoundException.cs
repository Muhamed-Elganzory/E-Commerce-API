using DomainLayer.Exceptions.NotFound;

namespace DomainLayer.Exceptions.Basket;

/// <summary>
///     Represents an exception that is thrown when a requested Basket cannot be found.
/// </summary>
/// <param name="id">The unique identifier of the Basket that was not found.</param>
public sealed class BasketNotFoundException(string id) : NotFoundException($"Basket {id} is not found")
{

}