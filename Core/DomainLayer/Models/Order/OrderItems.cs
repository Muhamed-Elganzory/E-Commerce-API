using DomainLayer.Models.Shared;

namespace DomainLayer.Models.Order
{
    /// <summary>
    ///     Represents a single item within an order.
    /// </summary>
    public class OrderItems : BaseEntity<int>
    {
        /// <summary>
        ///     Gets or sets a snapshot of the product details for this order item.
        ///     This is a value object containing product ID, name, and picture URL.
        /// </summary>
        public ProductItemOrder ProductItemOrder { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the price of the product at the time of ordering.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        ///     Gets or sets the quantity of this product ordered.
        /// </summary>
        public int Quantity { get; set; }
    }
}