using AutoMapper;
using DomainLayer.Contracts.Unit;
using Service.Product;
using serviceAbstraction.Contracts.Product;
using serviceAbstraction.Contracts.Service;

namespace Service.Service;

/// <summary>
///     Service Manager Implementation.
///     This class acts as a centralized manager for different services in the application.
///     It provides access to various services, such as ProductService, through lazy initialization.
/// </summary>
/// <param name="unitOfWork"></param>
/// <param name="mapper"></param>
public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper): IServiceManager
{
    /// <summary>
    ///     _productService: Lazy initialization of ProductService.
    /// </summary>
    private readonly Lazy<IProductService> _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));

    /// <summary>
    ///     Product Service to handle product-related operations.
    /// </summary>
    public IProductService ProductService  => _productService.Value;
}