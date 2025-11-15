using DomainLayer.Models.Shared;

namespace DomainLayer.Models.Order
{
    /// <summary>
    ///     Represents a delivery method option available for customer orders.
    /// </summary>
    public class DeliveryMethod : BaseEntity<int>
    {
        /// <summary>
        ///     Gets or sets a short name or title for the delivery method.
        /// </summary>
        public string ShortName { get; set; } = null!;

        /// <summary>
        ///     Gets or sets a detailed description of the delivery method.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the estimated delivery time (e.g., "2-3 days").
        /// </summary>
        public string DeliveryTime { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the cost associated with this delivery method.
        /// </summary>
        public decimal Cost { get; set; }
    }
}