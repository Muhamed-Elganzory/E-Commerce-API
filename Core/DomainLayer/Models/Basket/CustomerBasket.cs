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
    public ICollection<BasketItem> items { get; set; } = []; // => BasketItems

    /// <summary>
    ///     The payment intent identifier returned by the payment provider (e.g., Stripe).
    ///     Used for managing and updating the payment session.
    /// </summary>
    public string? PaymentIntentId { get; set; }

    /// <summary>
    ///     The client secret associated with the payment intent.
    ///     Required by the client to complete the payment process.
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    ///     The shipping price for the selected delivery method.
    /// </summary>
    public decimal? ShippingPrice { get; set; } // DeliveryMethod.Price

    /// <summary>
    ///     The ID of the selected delivery method.
    /// </summary>
    public int? DeliveryMethodId { get; set; }
}
