using Service.Spec.Base;

namespace Service.Spec.Order
{
    /// <summary>
    ///     Specification for querying <see cref="DomainLayer.Models.Order.Order"/> entities.
    ///     <para>
    ///         Defines filtering criteria, related data to include (eager loading), and sorting options.
    ///     </para>
    /// </summary>
    public class OrderSpecification : BaseSpecification<DomainLayer.Models.Order.Order, Guid>
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="OrderSpecification"/> to filter orders by user email.
        /// </summary>
        /// <param name="email">User email to filter orders.</param>
        /// <remarks>
        ///     Includes related entities: OrderItems, deliveryMethod, ShippingAddress.
        ///     Orders results by OrderDate descending (most recent orders first).
        /// </remarks>
        public OrderSpecification(string email) : base(e => e.BuyerEmail == email)
        {
            // Include the collection of order items related to the order
            AddInclude(i => i.items);

            // Include the delivery method associated with the order
            AddInclude(d => d.DeliveryMethod);

            // Include the shipping address for the order
            AddInclude(a => a.ShipToAddress);

            // sort orders by order date in descending order (newest first)
            AddOrderByDescending(d => d.OrderDate);
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="OrderSpecification"/> to filter a specific order by its ID.
        /// </summary>
        /// <param name="orderId">Unique identifier of the order.</param>
        /// <remarks>
        ///     Includes related entities: OrderItems, deliveryMethod, ShippingAddress.
        ///     No sorting needed as this targets a single order.
        /// </remarks>
        public OrderSpecification(Guid orderId) : base(o => o.Id == orderId)
        {
            // Include the collection of order items related to the order
            AddInclude(i => i.items);

            // Include the delivery method associated with the order
            AddInclude(d => d.DeliveryMethod);

            // Include the shipping address for the order
            AddInclude(a => a.ShipToAddress);
        }
    }
}
