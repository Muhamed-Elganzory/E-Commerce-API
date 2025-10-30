namespace Shared.DTO.Basket;

/// <summary>
///     Represents a customer's shopping basket that contains all the items
///     they have selected. This DTO (Data Transfer Object) is used to transfer
///     basket data between the client and the server.
/// </summary>
public class CustomerBasketDto
{
    /// <summary>
    ///     The unique identifier for the basket, typically generated on the client side (e.g., a GUID).
    ///     This ID allows the server to identify and retrieve the correct basket for a specific user.
    /// </summary>
    public string Id { get; set; } = null!; // Guid created from client [Frontend]

    /// <summary>
    ///     A collection of items that the customer has added to their basket.
    ///     Each item includes details such as product ID, name, price, quantity, etc.
    /// </summary>
    public ICollection<BasketItemDto> BasketItems { get; set; } = [];
}