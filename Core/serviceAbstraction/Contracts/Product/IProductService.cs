using Shared.DTO.Product;
using Shared.Enums.Product;
using Shared.Pagination;
using Shared.Queries;

namespace serviceAbstraction.Contracts.Product;

/// <summary>
///     Interface for Product Service
/// </summary>
public interface IProductService
{
    /// <summary>
    ///     Get all products
    /// </summary>
    /// <param name="queryParams">
    ///     An instance of <see cref="ProductQueryParams"/> containing optional filters
    ///     (e.g., <c>BrandId</c>, <c>TypeId</c>) and sorting preferences
    ///     (e.g., <see cref="Shared.Enums.Product.ProductSortingOptions"/>).
    /// </param>
    /// <returns></returns>
    public Task <PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams);

    /// <summary>
    ///     Get product by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task <ProductDto?> GetProductByIdAsync(int id);

    /// <summary>
    ///     Get all brands
    /// </summary>
    /// <returns></returns>
    public Task <IEnumerable<BrandDto>> GetAllBrandsAsync();


    /// <summary>
    ///     Get all types
    /// </summary>
    /// <returns></returns>
    public Task <IEnumerable<TypeDto>> GetAllTypesAsync();
}