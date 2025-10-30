namespace DomainLayer.Models.Basket;

/// <summary>
///     Represents a single product item inside a customer's basket.
/// </summary>
public class BasketItem
{
    /// <summary>
    ///     The unique identifier of the product.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The name of the product.
    /// </summary>
    public string ProductName { get; set; } = null!;

    /// <summary>
    ///     The URL of the product image (optional).
    /// </summary>
    public string? PictureUrl { get; set; }

    /// <summary>
    ///     The price of a single unit of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    ///     The quantity of this product added to the basket.
    /// </summary>
    public int Quantity { get; set; }
}