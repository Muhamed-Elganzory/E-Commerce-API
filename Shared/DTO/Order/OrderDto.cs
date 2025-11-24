namespace Shared.DTO.Order
{
    /// <summary>
    ///     Represents the data required to create a new order.
    /// </summary>
    public record OrderDto
    {
        /// <summary>
        ///     Gets or sets the identifier of the basket/cart containing the items to order.
        /// </summary>
        public string BasketId { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the identifier of the selected delivery method.
        /// </summary>
        public int DeliveryMethodId { get; set; }

        /// <summary>
        ///     Gets or sets the shipping address details for the order.
        /// </summary>
        public ShippingAddressDto ShipToAddress { get; set; } = null!; // => ShippingAddress
    }
}
