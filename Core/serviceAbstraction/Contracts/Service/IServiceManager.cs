using serviceAbstraction.Contracts.Product;

namespace serviceAbstraction.Contracts.Service;

/// <summary>
///     Interface for Service Manager to manage different services.
/// </summary>
public interface IServiceManager
{
    /// <summary>
    ///     Product Service to handle product-related operations.
    /// </summary>
    public IProductService ProductService { get;  }
}