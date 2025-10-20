using AutoMapper;
using DomainLayer.Contracts.Unit;
using serviceAbstraction.Contracts.Product;
using DomainLayer.Models.Product;
using Shared.DTO.Product;

namespace Service.Product;

/// <summary>
///     Product Service Implementation.
///     This class provides the business logic related to products, brands, and types.
///     It defines how the business does its operations — how data is retrieved, processed,
///     and returned to the controller.
///     It uses the Unit of Work pattern to manage repositories,
///     and AutoMapper to map between domain models and DTOs.
/// </summary>
/// <param name="unitOfWork">The Unit of Work instance used to manage repositories.</param>
/// <param name="mapper">The AutoMapper instance used for object mapping.</param>
public class ProductService(IUnitOfWork unitOfWork, IMapper mapper): IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    ///     Get all products asynchronously from the repository and map them to ProductDto using AutoMapper.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        // Create repository for Product entity asynchronously it way 1
        // var repo = await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Product.Product, int>();

        // Get all products asynchronously from the repository it way 1
        // var products = await repo.GetAllAsync();

        // Create repository for Product entity asynchronously it way 2 and Get all products asynchronously from the repository it way 2 (One Line)
        var repo = await (await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Product.Product, int>()).GetAllAsync();

        // Map the products to ProductDto using AutoMapper
        var productData = _mapper.Map<IEnumerable<DomainLayer.Models.Product.Product>,  IEnumerable<ProductDto>>(repo);

        // Return the mapped product data
        return productData;
    }

    /// <summary>
    ///     Get product by id asynchronously from the repository and map it to ProductDto using AutoMapper.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        // Create repository for Product entity asynchronously
        var repo = await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Product.Product, int>();

        // Get product by id asynchronously from the repository
        var product = await repo.GetByIdAsync(id);

        // Map the product to ProductDto using AutoMapper
        var productData = _mapper.Map<DomainLayer.Models.Product.Product?, ProductDto?>(product);

        // Return the mapped product data or null if not found
        return productData ?? null;
    }

    /// <summary>
    ///     Get all brands asynchronously from the repository and map them to BrandDto using AutoMapper.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
    {
        // Create repository for ProductBrand entity asynchronously
        var repo = await _unitOfWork.CreateRepositoryAsync<ProductBrand, int>();

        var brands = await repo.GetAllAsync();

        // Map the types to TypeDto using AutoMapper
        var brandsData = _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(brands);

        return brandsData;
    }

    /// <summary>
    ///     Get all types asynchronously from the repository and map them to TypeDto using AutoMapper.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
    {
        // Create repository for ProductType entity asynchronously and Get all types asynchronously from the repository (One Line)
        var repoAndData = await (await _unitOfWork.CreateRepositoryAsync<ProductType, int>()).GetAllAsync();

        // Map the types to TypeDto using AutoMapper
        // var typesData = _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(repoAndData);

        // Return the mapped types data
        return _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(repoAndData);
    }
}