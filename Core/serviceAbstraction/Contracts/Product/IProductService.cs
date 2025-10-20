using Shared.DTO.Product;

namespace serviceAbstraction.Contracts.Product;

/// <summary>
///     Interface for Product Service
/// </summary>
public interface IProductService
{
    /// <summary>
    ///     Get all products
    /// </summary>
    /// <returns></returns>
    public Task <IEnumerable<ProductDto>> GetAllProductsAsync();

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