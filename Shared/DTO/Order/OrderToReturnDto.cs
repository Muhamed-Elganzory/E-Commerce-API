namespace Shared.DTO.Order
{
    /// <summary>
    ///     Represents the details of an order to be returned to the client.
    /// </summary>
    public class OrderToReturnDto
    {
        /// <summary>
        ///     Gets or sets the unique identifier of the order.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the email of the user who placed the order.
        /// </summary>
        public string UserEmail { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the date and time when the order was placed, including timezone offset.
        /// </summary>
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        ///     Gets or sets the status of the order as a string (e.g., "Pending", "PaymentReceived").
        /// </summary>
        public string OrderStatus { get; set; } = null!; // Enum as string for easier client use

        /// <summary>
        ///     Gets or sets the shipping address information.
        /// </summary>
        public ShippingAddressDto ShippingAddress { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the delivery method name selected for this order.
        /// </summary>
        public string DeliveryMethod { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the collection of order items included in this order.
        /// </summary>
        public ICollection<OrderItemsDto> OrderItems { get; set; } = new List<OrderItemsDto>();

        /// <summary>
        ///     Gets or sets the subtotal amount before adding delivery cost.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        ///     Gets or sets the total amount for the order, including delivery cost.
        /// </summary>
        public decimal Total { get; set; }
    }
}