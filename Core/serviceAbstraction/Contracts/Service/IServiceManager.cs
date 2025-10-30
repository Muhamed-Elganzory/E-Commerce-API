using serviceAbstraction.Contracts.Basket;
using serviceAbstraction.Contracts.Product;

namespace serviceAbstraction.Contracts.Service;

/// <summary>
///     Defines a centralized interface for accessing all core application services.
///     The Service Manager provides a single entry point to interact with different
///     business logic layers such as products, baskets, and others.
/// </summary>
public interface IServiceManager
{
    /// <summary>
    ///     Provides access to the product-related business logic and operations.
    ///     This service is responsible for handling product retrieval, filtering,
    ///     and pagination logic.
    /// </summary>
    IProductService ProductService { get; }

    /// <summary>
    ///     Provides access to the basket-related business logic and operations.
    ///     This service manages customer basket creation, updates, and deletion.
    /// </summary>
    IBasketService BasketService { get; }
}