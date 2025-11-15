using DomainLayer.Exceptions.NotFound;
using DomainLayer.Models.Order;

namespace DomainLayer.Exceptions.Order;

/// <summary>
///     Exception thrown when a requested <see cref="DeliveryMethod"/> cannot be found.
/// </summary>
/// <remarks>
///     This exception is typically used in the order processing workflow when:
///     <list type="bullet">
///         <item><description>A delivery method ID provided by the client does not exist in the database.</description></item>
///         <item><description>The delivery method was deleted or disabled after being referenced.</description></item>
///     </list>
///     It inherits from <see cref="NotFoundException"/> to provide a standardized "404-style" exception type.
/// </remarks>
/// <param name="id">The ID of the delivery method that could not be found.</param>
public class DeliveryMethodNotFoundException(int id) : NotFoundException($"Delivery method with ID '{id}' was not found.")
{
}