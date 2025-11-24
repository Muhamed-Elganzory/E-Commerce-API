using Shared.DTO.Order;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
using serviceAbstraction.Contracts.Service;

namespace Presentation.Controllers.Order
{
    /// <summary>
    ///     API controller responsible for managing customer orders.
    ///     <para>
    ///         Acts as the entry point for creating orders, retrieving delivery methods,
    ///         and fetching customer order history.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     - All endpoints require authentication via <see cref="AuthorizeAttribute"/>.
    ///     - Inherits from <see cref="BaseController"/> for shared routing and helper utilities.
    /// </remarks>
    [Authorize]
    public class OrdersController : BaseController
    {
        /// <summary>
        ///     Provides access to business logic services such as orders, products, and baskets.
        /// </summary>
        private readonly IServiceManager _serviceManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrdersController"/> class,
        ///     injecting the service manager that provides access to business services.
        /// </summary>
        /// <param name="serviceManager">The application's service manager.</param>
        public OrdersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        /// <summary>
        ///     Creates a new order using basket contents, delivery method, and shipping address.
        /// </summary>
        /// <param name="orderDto">
        ///     DTO that contains basket ID, selected delivery method, and full shipping address.
        /// </param>
        /// <returns>
        ///     Returns a fully populated <see cref="OrderToReturnDto"/> containing the created order details.
        /// </returns>
        /// <response code="200">Order created successfully.</response>
        /// <response code="400">Returned if email claim is missing.</response>
        /// <response code="401">Returned if user is not authenticated.</response>
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            // Extract authenticated user email from JWT token
            var email = GetEmailFromClaimsToken();

            if (string.IsNullOrEmpty(email))
                return BadRequest("User email not found in token.");

            // Create the order using the business service layer
            var order = await _serviceManager.OrderService.CreateOrderAsync(orderDto, email);

            return Ok(order);
        }

        /// <summary>
        ///     Retrieves all available delivery methods (shipping options).
        /// </summary>
        /// <returns>
        ///     A list of <see cref="DeliveryMethodDto"/> representing all delivery methods.
        /// </returns>
        /// <response code="200">Returns all available delivery methods.</response>
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await _serviceManager.OrderService.GetAllDeliveryMethodAsync();
            return Ok(deliveryMethods);
        }

        /// <summary>
        ///     Retrieves all orders that belong to the authenticated user.
        /// </summary>
        /// <returns>
        ///     Returns a collection of <see cref="OrderToReturnDto"/> representing the user's order history.
        /// </returns>
        /// <response code="200">Returns all orders for the authenticated user.</response>
        /// <response code="400">Returned if user email claim is missing.</response>
        [HttpGet("Orders")]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrders()
        {
            var email = GetEmailFromClaimsToken();

            if (string.IsNullOrEmpty(email))
                return BadRequest("User email not found in token.");

            var orders = await _serviceManager.OrderService.GetAllOrderAsync(email);
            return Ok(orders);
        }

        /// <summary>
        ///     Retrieves a single order by its unique identifier.
        /// </summary>
        /// <param name="orderId">The unique ID of the order.</param>
        /// <returns>
        ///     A single <see cref="OrderToReturnDto"/> containing the requested order details.
        /// </returns>
        /// <response code="200">Returns the requested order.</response>
        /// <response code="404">Returned if the order does not exist.</response>
        [HttpGet("{orderId:Guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid orderId)
        {
            var order = await _serviceManager.OrderService.GetOrderByIdAsync(orderId);

            return Ok(order);
        }
    }
}
