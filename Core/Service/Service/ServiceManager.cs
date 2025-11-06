using AutoMapper;
using DomainLayer.Contracts.Repository.Basket;
using DomainLayer.Contracts.Unit;
using DomainLayer.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Auth;
using Service.Basket;
using Service.Product;
using ServiceAbstraction.Contracts.Auth;
using serviceAbstraction.Contracts.Basket;
using serviceAbstraction.Contracts.Product;
using serviceAbstraction.Contracts.Service;

namespace Service.Service
{
    /// <summary>
    ///     Implements the <see cref="IServiceManager"/> interface.
    ///     <para>
    ///         Acts as a centralized entry point for accessing all application services.
    ///         Uses lazy initialization to ensure that each service is instantiated only when first accessed,
    ///         optimizing resource utilization and improving performance.
    ///     </para>
    /// </summary>
    public class ServiceManager : IServiceManager
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
        ///     Initializes a new instance of the <see cref="ServiceManager"/> class.
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
        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
            _basketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, configuration));
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
    }
}