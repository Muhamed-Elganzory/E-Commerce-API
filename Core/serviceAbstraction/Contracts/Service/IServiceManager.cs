using ServiceAbstraction.Contracts.Auth;
using serviceAbstraction.Contracts.Basket;
using serviceAbstraction.Contracts.Order;
using serviceAbstraction.Contracts.Product;

namespace serviceAbstraction.Contracts.Service
{
    /// <summary>
    ///     Defines a centralized interface for accessing all core application services.
    ///     <para>
    ///         The <see cref="IServiceManager"/> acts as a unified entry point to interact
    ///         with different domain-specific services, promoting loose coupling and
    ///         better dependency management.
    ///     </para>
    /// </summary>
    public interface IServiceManager
    {
        /// <summary>
        ///     Provides access to the product-related business logic and operations.
        ///     <para>
        ///         Responsible for handling product retrieval, filtering, sorting,
        ///         and pagination logic across the application.
        ///     </para>
        /// </summary>
        public IProductService ProductService { get; }

        /// <summary>
        ///     Provides access to the basket-related business logic and operations.
        ///     <para>
        ///         Manages customer shopping basket creation, updates, retrieval,
        ///         and deletion from the persistent storage (e.g., Redis).
        ///     </para>
        /// </summary>
        public IBasketService BasketService { get; }

        /// <summary>
        ///     Provides access to authentication and authorization operations.
        ///     <para>
        ///         Handles user registration, login, logout, and token generation logic
        ///         through the <see cref="IAuthenticationService"/>.
        ///     </para>
        /// </summary>
        public IAuthenticationService AuthenticationService { get; }

        /// <summary>
        ///     Provides access to order management operations.
        ///     <para>
        ///         Handles order creation, retrieval, and persistence logic.
        ///         Manages related entities such as <c>OrderItems</c>, <c>DeliveryMethod</c>,
        ///         and <c>ShippingAddress</c>, ensuring consistency across the ordering process.
        ///     </para>
        /// </summary>
        public IOrderService OrderService { get; }
    }
}