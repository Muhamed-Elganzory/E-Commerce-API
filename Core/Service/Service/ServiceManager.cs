using AutoMapper;
using DomainLayer.Contracts.Repository.Basket;
using DomainLayer.Contracts.Unit;
using Service.Basket;
using Service.Product;
using serviceAbstraction.Contracts.Basket;
using serviceAbstraction.Contracts.Product;
using serviceAbstraction.Contracts.Service;

namespace Service.Service;

/// <summary>
///     Implements the <see cref="IServiceManager"/> interface.
///     Acts as a centralized entry point for accessing all application services.
///     Uses lazy initialization to ensure that services are only created when needed.
/// </summary>
public class ServiceManager : IServiceManager
{
    /// <summary>
    ///     Provides access to the product service with lazy initialization.
    ///     The service is only created when first accessed.
    /// </summary>
    private readonly Lazy<IProductService> _productService;

    /// <summary>
    ///     Provides access to the basket service with lazy initialization.
    ///     The service is only created when first accessed.
    /// </summary>
    private readonly Lazy<IBasketService> _basketService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ServiceManager"/> class.
    /// </summary>
    /// <param name="unitOfWork">The Unit of Work used for coordinating repository operations.</param>
    /// <param name="mapper">The AutoMapper instance for mapping between entities and DTOs.</param>
    /// <param name="basketRepository">The Basket repository used for managing Redis basket data.</param>
    public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository)
    {
        // Initialize ProductService lazily, ensuring it's only created when needed
        _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));

        // Initialize BasketService lazily, ensuring it's only created when needed
        _basketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
    }

    /// <summary>
    ///     Gets the product service responsible for handling product-related business logic.
    /// </summary>
    public IProductService ProductService => _productService.Value;

    /// <summary>
    ///     Gets the basket service responsible for handling shopping basket operations.
    /// </summary>
    public IBasketService BasketService => _basketService.Value;
}