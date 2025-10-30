namespace DomainLayer.Models.Basket;

/// <summary>
///     Represents a shopping basket (cart) that belongs to a customer.
///     It contains a unique identifier and a collection of basket items.
/// </summary>
public class CustomerBasket
{
    /// <summary>
    ///     The unique basket ID generated on the client side (usually a GUID).
    /// </summary>
    public string Id { get; set; } = null!; // Guid Created From Client [Frontend]

    /// <summary>
    ///     A collection of all items that the customer has added to the basket.
    /// </summary>
    public ICollection<BasketItem> BasketItems { get; set; } = [];
}