namespace DomainLayer.Models.Order
{
    /// <summary>
    ///     Represents the current status of an order.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        ///     The order has been created but payment is not yet received.
        /// </summary>
        Pending = 0, // Default Value

        /// <summary>
        ///     Payment for the order has been successfully received.
        /// </summary>
        PaymentReceived = 1,

        /// <summary>
        ///     Payment for the order has failed.
        /// </summary>
        PaymentFailed = 2
    }
}