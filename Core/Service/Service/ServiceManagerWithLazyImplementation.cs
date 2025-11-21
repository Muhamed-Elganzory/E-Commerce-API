using AutoMapper;
using DomainLayer.Contracts.Unit;
using DomainLayer.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Auth;
using Service.Basket;
using Service.Order;
using Service.Product;
using ServiceAbstraction.Contracts.Auth;
using serviceAbstraction.Contracts.Basket;
using serviceAbstraction.Contracts.Order;
using serviceAbstraction.Contracts.Product;
using serviceAbstraction.Contracts.Service;
using DomainLayer.Contracts.Repository.Basket;

namespace Service.Service
{
    // 🔹 Go To ServiceManagerWithDelegate
    /// <summary>
    ///     Implements the <see cref="IServiceManager"/> interface.
    ///     <para>
    ///         Acts as a centralized entry point for accessing all application services.
    ///         Uses <see cref="Lazy{T}"/> initialization to ensure that each service
    ///         is instantiated only when first accessed — optimizing resource usage
    ///         and improving application performance.
    ///     </para>
    /// </summary>
    public class ServiceManagerWithLazyImplementation // : IServiceManager 🔹 To Implementation Go ServiceManagerWithDelegate
    {
        /// <summary>
        ///     Lazily initialized product service for managing product-related operations.
        /// </summary>
        private readonly Lazy<IProductService> _productService;

        /// <summary>
        ///     Lazily initialized basket service for managing user basket operations.
        /// </summary>
        private readonly Lazy<IBasketService> _basketService;

        /// <summary>
        ///     Lazily initialized authentication service for handling user authentication logic.
        /// </summary>
        private readonly Lazy<IAuthenticationService> _authenticationService;

        /// <summary>
        ///     Lazily initialized order service for managing order creation, retrieval,
        ///     and delivery-related operations.
        /// </summary>
        private readonly Lazy<IOrderService> _orderService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceManagerWithLazyImplementation"/> class.
        /// </summary>
        /// <param name="unitOfWork">
        ///     The Unit of Work instance responsible for coordinating repository operations
        ///     and ensuring transactional consistency across multiple repositories.
        /// </param>
        /// <param name="mapper">
        ///     The AutoMapper instance used for object-to-object mapping between entities and DTOs.
        /// </param>
        /// <param name="basketRepository">
        ///     The repository responsible for managing basket-related data,
        ///     typically stored in Redis or another persistent storage.
        /// </param>
        /// <param name="userManager">
        ///     The ASP.NET Core Identity <see cref="UserManager{TUser}"/> used to handle
        ///     user authentication, registration, and identity management operations.
        /// </param>
        /// <param name="configuration">
        ///     The application configuration instance used to access environment settings,
        ///     app secrets, and JWT authentication parameters.
        /// </param>
        public ServiceManagerWithLazyImplementation(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IBasketRepository basketRepository,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
            _basketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, configuration, mapper));
            _orderService = new Lazy<IOrderService>(() => new OrderService(basketRepository, mapper, unitOfWork));
        }

        /// <summary>
        ///     Gets the product service responsible for managing product-related business logic.
        /// </summary>
        public IProductService ProductService => _productService.Value;

        /// <summary>
        ///     Gets the basket service responsible for handling basket-related business logic.
        /// </summary>
        public IBasketService BasketService => _basketService.Value;

        /// <summary>
        ///     Gets the authentication service responsible for user login, registration, and token handling.
        /// </summary>
        public IAuthenticationService AuthenticationService => _authenticationService.Value;

        /// <summary>
        ///     Gets the order service responsible for creating and managing customer orders,
        ///     including order items, delivery methods, and payment integration.
        /// </summary>
        public IOrderService OrderService => _orderService.Value;
    }
}