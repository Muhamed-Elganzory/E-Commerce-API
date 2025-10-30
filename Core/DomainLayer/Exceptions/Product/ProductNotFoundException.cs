using DomainLayer.Exceptions.NotFound;

namespace DomainLayer.Exceptions.Product;

/// <summary>
///     Represents an exception that is thrown when a requested product cannot be found.
/// </summary>
/// <param name="id">The unique identifier of the product that was not found.</param>
public sealed class ProductNotFoundException(int id) : NotFoundException($"The product with ID {id} is not found.")
{
}