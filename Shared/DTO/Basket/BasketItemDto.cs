namespace Shared.DTO.Basket;

/// <summary>
///     Represents an individual item within a customer's shopping basket.
///     Contains key product details such as name, price, and quantity.
/// </summary>
public class BasketItemDto
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