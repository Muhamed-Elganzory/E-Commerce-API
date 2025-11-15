namespace Shared.DTO.Order
{
    /// <summary>
    ///     Represents a single item in an order to be returned to the client.
    /// </summary>
    public class OrderItemsDto
    {
        public int ProductId { get; set; }
        /// <summary>
        ///     Gets or sets the name of the product.
        /// </summary>
        public string ProductName { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the URL of the product image (optional).
        /// </summary>
        public string? PictureUrl { get; set; }

        /// <summary>
        ///     Gets or sets the price of the product at the time of ordering.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        ///     Gets or sets the quantity of the product ordered.
        /// </summary>
        public int Quantity { get; set; }
    }
}