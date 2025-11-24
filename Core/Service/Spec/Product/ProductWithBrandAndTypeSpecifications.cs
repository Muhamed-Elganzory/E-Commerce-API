using Shared.Queries;
using Service.Spec.Base;
using Shared.Enums.Product;

namespace Service.Spec.Product;

/// <summary>
///     A baseSpecification that includes related <c>ProductBrand</c> and <c>ProductType</c>
///     navigation properties when querying <c>Product</c> entities.
/// </summary>
public class ProductWithBrandAndTypeBaseSpecifications : BaseSpecification<DomainLayer.Models.Product.Product, int>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ProductWithBrandAndTypeBaseSpecifications"/> class,
    ///     applying dynamic **filtering**, **searching**, and **sorting** rules based on the provided <see cref="ProductQueryParams"/>.
    ///     This specification also eagerly loads related <c>ProductBrand</c> and <c>ProductType</c> entities
    ///     to ensure optimized query performance and prevent lazy-loading.
    /// </summary>
    /// <param name="queryParams">
    ///     An object containing query parameters used to filter, search, and sort products:
    ///     <list type="bullet">
    ///         <item><description><c>BrandId</c> — Optional. Filters products by brand if provided.</description></item>
    ///         <item><description><c>TypeId</c> — Optional. Filters products by type if provided.</description></item>
    ///         <item><description><c>SearchValue</c> — Optional. Performs a case-insensitive search by product name or description.</description></item>
    ///         <item><description><c>SortingOptions</c> — Determines how products are sorted (e.g., by name or price, ascending or descending).</description></item>
    ///     </list>
    /// </param>
    /// <remarks>
    ///     🧠 This constructor demonstrates **constructor chaining** by calling the base class
    ///     (<see cref="BaseSpecification{TEntity, TKey}"/>) with a composite predicate expression that:
    ///     <list type="number">
    ///         <item>Applies brand and type filters only if their values are provided.</item>
    ///         <item>Performs a case-insensitive text search when <c>SearchValue</c> is not null or empty.</item>
    ///     </list>
    ///     <br/>
    ///
    ///     Example predicate expression:
    ///     <code>
    ///     p => (!queryParams.BrandId.HasValue || p.ProductBrandId == queryParams.BrandId)
    ///          and (!queryParams.TypeId.HasValue || p.ProductTypeId == queryParams.TypeId)
    ///          and (string.IsNullOrWhiteSpace(queryParams.SearchValue)
    ///              || p.Name.Contains(queryParams.SearchValue, StringComparison.CurrentCultureIgnoreCase)))
    ///     </code>
    ///     <br/>
    ///
    ///     The <see cref="BaseSpecification{TEntity, TKey}.AddInclude"/> method is used
    ///     to include <c>ProductBrand</c> and <c>ProductType</c> for eager loading. <br/><br/>
    ///
    ///     Finally, a <c>switch</c> statement applies sorting logic
    ///     based on <c>queryParams.SortingOptions</c>, keeping all query-related logic
    ///     encapsulated within the specification for cleaner and more testable service methods.
    /// </remarks>
    /// <example>
    ///     Example usage:
    ///     <code>
    ///         var queryParams = new ProductQueryParams
    ///         {
    ///             BrandId = 2,
    ///             TypeId = null,
    ///             SearchValue = "laptop",
    ///             SortingOptions = ProductSortingOptions.PriceDescending
    ///         };
    ///
    ///         var spec = new ProductWithBrandAndTypeBaseSpecifications(queryParams);
    ///     </code>
    /// </example>
    public ProductWithBrandAndTypeBaseSpecifications(ProductQueryParams queryParams)
        : base(p =>
            (!queryParams.BrandId.HasValue || p.ProductBrandId == queryParams.BrandId) &&
            (!queryParams.TypeId.HasValue || p.ProductTypeId == queryParams.TypeId) &&
            (string.IsNullOrWhiteSpace(queryParams.Search) || p.Name.ToLower().Contains(queryParams.Search.ToLower())))
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);

        switch (queryParams.sort)
        {
            case ProductSortingOptions.nameAsc:
                AddOrderByAscending(p => p.Name);
                break;

            case ProductSortingOptions.nameDesc:
                AddOrderByDescending(p => p.Name);
                break;

            case ProductSortingOptions.priceAsc:
                AddOrderByAscending(p => p.Price);
                break;

            case ProductSortingOptions.priceDesc:
                AddOrderByDescending(p => p.Price);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(queryParams.sort), queryParams.sort, null);
        }

        // Apply pagination settings based on the provided query parameters (page size and page index)
        ApplyPagination(queryParams.PageSize, queryParams.PageNumber);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ProductWithBrandAndTypeBaseSpecifications"/> class
    ///     that retrieves a specific product by its identifier, including its <c>ProductBrand</c>
    ///     and <c>ProductType</c> navigation properties.
    /// </summary>
    /// <param name="id">The unique identifier of the product to retrieve.</param>
    /// <remarks>
    ///     This constructor uses <b>constructor chaining</b> to call the base class constructor
    ///     (<see cref="BaseSpecification{TEntity, TKey}"/>) with a filtering expression (<c>p => p.id == id</c>). <br/>
    ///     This ensures that the specification targets only the product matching the given <paramref name="id"/>. <br/>
    ///     It also uses <see cref="BaseSpecification{TEntity,TKey}.AddInclude"/> to eagerly load the related <c>ProductBrand</c> and <c>ProductType</c>
    ///     entities in a single query.
    /// </remarks>
    public ProductWithBrandAndTypeBaseSpecifications(int id) : base(p => p.Id == id)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }
}
