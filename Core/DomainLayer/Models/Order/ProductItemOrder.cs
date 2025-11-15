namespace DomainLayer.Models.Order
{
    /// <summary>
    ///     Represents a snapshot of product details associated with an order item.
    ///     This class is used to store essential product information
    ///     at the time the order was placed to maintain historical accuracy.
    /// </summary>
    public class ProductItemOrder
    {
        /// <summary>
        ///     The unique identifier of the product.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        ///     The name of the product.
        /// </summary>
        public string ProductName { get; set; } = null!;

        /// <summary>
        ///     The URL of the product image (optional).
        /// </summary>
        public string? PictureUrl { get; set; }
    }
}