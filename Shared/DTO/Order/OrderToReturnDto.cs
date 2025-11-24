namespace Shared.DTO.Order
{
    /// <summary>
    ///     Represents the details of an order returned to the client.
    /// </summary>
    public record OrderToReturnDto
    {
        /// <summary>
        ///     Unique identifier of the order.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Email of the customer who placed the order.
        /// </summary>
        public string BuyerEmail { get; set; } = null!; // => UserEmail

        /// <summary>
        ///     Shipping address associated with this order.
        /// </summary>
        public ShippingAddressDto ShipToAddress { get; set; } = null!; // => ShippingAddress

        /// <summary>
        ///     Collection of items included in the order.
        /// </summary>
        public ICollection<OrderItemsDto> items { get; set; } = new List<OrderItemsDto>(); // => OrderItems

        /// <summary>
        ///     Current status of the order as a string
        ///     (e.g., "Pending", "PaymentReceived").
        /// </summary>
        public string Status { get; set; } = null!; // Enum mapped to string // => OrderStatus

        /// <summary>
        ///     Name of the selected delivery method.
        /// </summary>
        public string DeliveryMethod { get; set; } = null!;

        /// <summary>
        ///     ID of the delivery method chosen for this order.
        /// </summary>
        public int DeliveryMethodId { get; set; }

        /// <summary>
        ///     Cost of delivery associated with this order.
        /// </summary>
        public decimal DeliveryCost { get; set; }

        /// <summary>
        ///     Order subtotal before applying delivery cost.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        ///     Date and time when the order was placed (with timezone offset).
        /// </summary>
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        ///     Stripe payment intent identifier used to track payment status.
        /// </summary>
        public string PaymentIntentId { get; set; } = null!;

        /// <summary>
        ///     Total cost of the order, including delivery.
        /// </summary>
        public decimal Total { get; set; }
    }
}
