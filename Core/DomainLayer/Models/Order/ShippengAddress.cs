namespace DomainLayer.Models.Order
{
    /// <summary>
    ///     Represents the shipping address details for an order.
    /// </summary>
    public class ShippingAddress
    {
        /// <summary>
        ///     Gets or sets the recipient's first name.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the recipient's last name.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the street address.
        /// </summary>
        public string Street { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the city name.
        /// </summary>
        public string City { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the country name.
        /// </summary>
        public string Country { get; set; } = null!;
    }
}