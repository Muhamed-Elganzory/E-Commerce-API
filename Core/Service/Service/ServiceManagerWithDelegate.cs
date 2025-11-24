using AutoMapper;
using Service.Payment;
using ServiceAbstraction.Contracts.Auth;
using serviceAbstraction.Contracts.Basket;
using serviceAbstraction.Contracts.Order;
using serviceAbstraction.Contracts.Payment;
using serviceAbstraction.Contracts.Product;
using serviceAbstraction.Contracts.Service;

namespace Service.Service;

/// <summary>
///     A service manager implementation that uses factory delegates to lazily create service instances.
///     <para>
///         Instead of injecting services directly, this pattern injects factory functions
///         (<see cref="Func{T}"/>) that return service instances on demand.
///     </para>
///     <para>
///         This is particularly useful when implementing scoped services within a singleton consumer,
///         or when services must be resolved lazily for performance or dependency reasons.
///     </para>
/// </summary>
/// <param name="productFactory">
///     Factory delegate responsible for creating <see cref="IProductService"/> instances.
/// </param>
/// <param name="basketFactory">
///     Factory delegate responsible for creating <see cref="IBasketService"/> instances.
/// </param>
/// <param name="orderFactory">
///     Factory delegate responsible for creating <see cref="IOrderService"/> instances.
/// </param>
/// <param name="authenticationFactory">
///     Factory delegate responsible for creating <see cref="IAuthenticationService"/> instances.
/// </param>
/// <param name="paymentFactory">
///     Factory delegate responsible for creating <see cref="IPaymentService"/> instances.
/// </param>
public class ServiceManagerWithDelegate(
        Func<IProductService> productFactory,
        Func<IBasketService> basketFactory,
        Func<IOrderService> orderFactory,
        Func<IAuthenticationService> authenticationFactory,
        Func<IPaymentService> paymentFactory
    ) : IServiceManager
{
    /// <summary>
    ///     Gets a new instance of <see cref="IOrderService"/> using the injected factory.
    ///     <para>This ensures the service is created lazily and respects the DI lifetime.</para>
    /// </summary>
    public IOrderService OrderService => orderFactory.Invoke();

    /// <summary>
    ///     Gets a new instance of <see cref="IBasketService"/> using the injected factory.
    ///     <para>Useful when basket operations depend on scoped data such as user context.</para>
    /// </summary>
    public IBasketService BasketService => basketFactory.Invoke();

    /// <summary>
    ///     Gets a new instance of <see cref="IProductService"/> using the injected factory.
    ///     <para>Ensures product-related operations are resolved with correct DI lifetime.</para>
    /// </summary>
    public IProductService ProductService => productFactory.Invoke();

    /// <summary>
    ///     Gets a new instance of <see cref="IAuthenticationService"/> using the injected factory.
    ///     <para>Used for login, registration, and JWT token operations.</para>
    /// </summary>
    public IAuthenticationService AuthenticationService => authenticationFactory.Invoke();

    /// <summary>
    ///     Gets an instance of <see cref="IPaymentService"/> created via the factory delegate.
    ///     This allows lazy creation of the service when accessed.
    /// </summary>
    public IPaymentService PaymentService => paymentFactory.Invoke();
}
