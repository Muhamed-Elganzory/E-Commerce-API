namespace Shared.DTO.Order
{
    /// <summary>
    ///     Represents a delivery method returned to the client,
    ///     including cost and descriptive details.
    /// </summary>
    public record DeliveryMethodDto
    {
        /// <summary>
        ///     Unique identifier of the delivery method.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Short, user-friendly name of the delivery method
        ///     (e.g., Standard, Express).
        /// </summary>
        public string ShortName { get; set; } = null!;

        /// <summary>
        ///     Detailed description of what this delivery method offers.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        ///     Estimated delivery time displayed to the user
        ///     (e.g., "2-3 business days", "Next day delivery").
        /// </summary>
        public string DeliveryTime { get; set; } = null!;

        /// <summary>
        ///     Delivery cost associated with this method.
        /// </summary>
        public decimal Cost { get; set; }
    }
}
