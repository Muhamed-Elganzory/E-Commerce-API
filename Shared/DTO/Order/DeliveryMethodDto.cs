namespace Shared.DTO.Order
{
    /// <summary>
    ///     Represents a delivery method returned to the client,
    ///     including cost and descriptive details.
    /// </summary>
    public class DeliveryMethodDto
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
        ///     The name of the carrier or delivery service provider.
        /// </summary>
        public string DeliveryMethod { get; set; } = null!;

        /// <summary>
        ///     Delivery cost associated with this method.
        /// </summary>
        public decimal Cost { get; set; }
    }
}