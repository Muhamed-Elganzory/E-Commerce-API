using Shared.DTO.Order;

namespace serviceAbstraction.Contracts.Order
{
    /// <summary>
    ///     Service contract for managing customer orders and delivery methods.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        ///     Creates a new order for the specified user.
        /// </summary>
        /// <param name="orderDto">The order data transfer object containing order details.</param>
        /// <param name="email">The email address of the user placing the order.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the created order details.
        /// </returns>
        Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email);

        /// <summary>
        ///     Retrieves all available delivery methods.
        /// </summary>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains a list of delivery method DTOs.
        /// </returns>
        Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodAsync();

        /// <summary>
        ///     Retrieves all orders placed by the specified user.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains a list of order DTOs.
        /// </returns>
        Task<IEnumerable<OrderToReturnDto>> GetAllOrderAsync(string email);

        /// <summary>
        ///     Retrieves the details of a specific order by its ID.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the order details.
        /// </returns>
        Task<OrderToReturnDto> GetOrderByIdAsync(Guid orderId);
    }
}