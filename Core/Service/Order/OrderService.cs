using AutoMapper;
using DomainLayer.Contracts.Repository.Basket;
using DomainLayer.Contracts.Unit;
using DomainLayer.Exceptions.Basket;
using DomainLayer.Models.Order;
using DomainLayer.Exceptions.Order;
using DomainLayer.Exceptions.Product;
using OrderClass = DomainLayer.Models.Order.Order;
using Service.Spec.Order;
using serviceAbstraction.Contracts.Order;
using Shared.DTO.Order;

namespace Service.Order
{
    /// <summary>
    ///     Service responsible for managing customer orders, including creation and retrieval.
    /// </summary>
    /// <remarks>
    ///     This service:
    ///     <list type="number">
    ///         <item>Validates the customer's basket and ensures products exist.</item>
    ///         <item>Maps DTOs to domain models and builds order aggregates.</item>
    ///         <item>Persists the order and related data using the Unit of Work pattern.</item>
    ///         <item>Retrieves delivery methods and orders based on specifications.</item>
    ///     </list>
    /// </remarks>
    public class OrderService(IBasketRepository basketRepository, IMapper mapper, IUnitOfWork unitOfWork) : IOrderService
    {
        /// <summary>
        ///     Used to map between DTOs and domain models.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <summary>
        ///     Provides access to repositories and handles transaction management.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        ///     Repository for accessing customer baskets.
        /// </summary>
        private readonly IBasketRepository _basketRepository = basketRepository;

        /// <summary>
        ///     Creates a new order based on the provided <see cref="OrderDto"/> and user email.
        /// </summary>
        /// <param name="orderDto">The order data transfer object containing basket ID, shipping address, and delivery method ID.</param>
        /// <param name="email">The email of the user placing the order.</param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        ///     The result contains an <see cref="OrderToReturnDto"/> representing the created order.
        /// </returns>
        /// <exception cref="BasketNotFoundException">Thrown if the basket ID does not exist.</exception>
        /// <exception cref="ProductNotFoundException">Thrown if any product in the basket is not found.</exception>
        /// <exception cref="DeliveryMethodNotFoundException">Thrown if the delivery method ID is invalid.</exception>
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
        {
            // Map Shipping Address DTO to Domain Model
            var orderAddress = _mapper.Map<ShippingAddressDto, ShippingAddress>(orderDto.ShipToAddress);

            // Retrieve the basket by ID or throw if not found
            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId) ?? throw new BasketNotFoundException(orderDto.BasketId);

            // Prepare list to hold order items
            var orderItems = new List<OrderItems>();

            // Get product repository
            var productRepo = await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Product.Product, int>();

            // For each basket item, validate and create order items
            foreach (var item in basket.items)
            {
                var product = await productRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);

                var orderItem = new OrderItems
                {
                    ProductItemOrder = new ProductItemOrder
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl
                    },
                    Price = product.Price,
                    Quantity = item.Quantity
                };

                orderItems.Add(orderItem);
            }

            // Retrieve and validate the delivery method
            var deliveryMethodRepo = await _unitOfWork.CreateRepositoryAsync<DeliveryMethod, int>();
            var deliveryMethod = await deliveryMethodRepo.GetByIdAsync(orderDto.DeliveryMethodId) ?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

            // Calculate subtotal = sum of (quantity * price)
            var subTotal = orderItems.Sum(oi => oi.Quantity * oi.Price);

            if (basket.PaymentIntentId != null)
            {
                // Create the Order aggregate root
                var order = new OrderClass(email, orderAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);


                // Add the new order to the repository and save changes
                var orderRepo = await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Order.Order, Guid>();
                await orderRepo.CreateAsync(order);
                await _unitOfWork.SaveChangesAsync();

                // Map the created order domain model to DTO and return
                return _mapper.Map<DomainLayer.Models.Order.Order, OrderToReturnDto>(order);
            }

            throw new InvalidOperationException("Cannot create order because payment intent is missing.");
        }

        /// <summary>
        ///     Retrieves all available delivery methods.
        /// </summary>
        /// <returns>
        ///     A task representing the asynchronous operation.
        ///     The result contains a collection of <see cref="DeliveryMethodDto"/>.
        /// </returns>
        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodAsync()
        {
            var deliveryMethodRepo = await _unitOfWork.CreateRepositoryAsync<DeliveryMethod, int>();
            var deliveryMethods = await deliveryMethodRepo.GetAllAsync();

            return _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }

        /// <summary>
        ///     Retrieves all orders placed by a specific user.
        /// </summary>
        /// <param name="email">The email of the user whose orders to retrieve.</param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        ///     The result contains a collection of <see cref="OrderToReturnDto"/>.
        /// </returns>
        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrderAsync(string email)
        {
            // Use specification to load navigation property ( ShippingAddress - OrderItems - deliveryMethod ) and sort data
            var spec = new OrderSpecification(email);
            var ordersRepo = await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Order.Order, Guid>();
            var orders = await ordersRepo.GetAllAsync(spec);

            return _mapper.Map<IEnumerable<DomainLayer.Models.Order.Order>, IEnumerable<OrderToReturnDto>>(orders);
        }

        /// <summary>
        ///     Retrieves the details of a specific order by its unique identifier.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to retrieve.</param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        ///     The result contains an <see cref="OrderToReturnDto"/> with order details.
        /// </returns>
        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid orderId)
        {
            // Use specification to load navigation property ( ShippingAddress - OrderItems - deliveryMethod )
            var spec = new OrderSpecification(orderId);
            var orderRepo = await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Order.Order, Guid>();
            var order = await orderRepo.GetByIdWithSpecificationAsync(spec);

            return _mapper.Map<OrderToReturnDto>(order);
        }
    }
}
