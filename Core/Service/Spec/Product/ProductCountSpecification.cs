using Service.Spec.Base;
using Shared.Queries;

namespace Service.Spec.Product;

/// <summary>
///     Represents a specification used specifically for counting products
///     that match certain filter conditions (e.g., Brand, Type, Search keyword)
///     without including related entities or applying pagination.
/// </summary>
/// <param name="queryParams">
///     The query parameters containing filtering options such as:
///     <list type="bullet">
///         <item><description><b>BrandId</b>: Filters products by brand.</description></item>
///         <item><description><b>TypeId</b>: Filters products by type.</description></item>
///         <item><description><b>SearchValue</b>: Filters products whose names contain the given text.</description></item>
///     </list>
/// </param>
public class ProductCountSpecification(ProductQueryParams queryParams)
    : BaseSpecification<DomainLayer.Models.Product.Product, int>(p =>
        // Filter by BrandId if provided
        (!queryParams.BrandId.HasValue || p.ProductBrandId == queryParams.BrandId) &&

        // Filter by TypeId if provided
        (!queryParams.TypeId.HasValue || p.ProductTypeId == queryParams.TypeId) &&

        // Filter by name if SearchValue is provided (case-insensitive)
        (string.IsNullOrWhiteSpace(queryParams.SearchValue) ||
         p.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
{
}