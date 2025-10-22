using Service.Spec.Base;

namespace Service.Spec.Product;

/// <summary>
///     A specification that includes related <c>ProductBrand</c> and <c>ProductType</c>
///     navigation properties when querying <c>Product</c> entities.
/// </summary>
public class ProductWithBrandAndTypeSpecifications : BaseSpecification<DomainLayer.Models.Product.Product, int>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProductWithBrandAndTypeSpecifications"/> class.
    ///     Retrieves all products along with their associated <c>ProductBrand</c> and <c>ProductType</c>.
    /// </summary>
    public ProductWithBrandAndTypeSpecifications()
        : base(null)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProductWithBrandAndTypeSpecifications"/> class
    ///     that retrieves a specific product by its identifier, including its <c>ProductBrand</c>
    ///     and <c>ProductType</c> navigation properties.
    /// </summary>
    /// <param name="id">The unique identifier of the product to retrieve.</param>
    public ProductWithBrandAndTypeSpecifications(int id)
        : base(p => p.Id == id)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }
}