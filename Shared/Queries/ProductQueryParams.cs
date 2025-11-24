using Shared.Enums.Product;

namespace Shared.Queries;

/// <summary>
///     Represents query parameters for retrieving products through the API.
///     This class encapsulates optional filters and sorting preferences,
///     enabling cleaner controller actions and automatic model binding from query strings.
/// </summary>
/// <remarks>
///     ✅ **Purpose:**
///     - Centralizes query-related parameters for the <see>
///         <cref>ProductsController</cref>
///     </see>
///     .
///     - Makes it easier to extend with future parameters (e.g., pagination, search keywords).
///
///     ⚙️ **Usage Example:**
///     <code>
///         GET /api/Products
///         GET /api/Products?brandId=2
///         GET /api/Products?brandId=2 and typeId=3
///         GET /api/Products?sortingOptions=PriceDescending
///     </code>
/// </remarks>
public class ProductQueryParams
{
    /// <summary>
    ///     (Optional) The ID of the brand to filter products by.
    ///     If <c>null</c>, all brands are included in the results.
    /// </summary>
    public int? BrandId { get; set; }

    /// <summary>
    ///     (Optional) The ID of the type to filter products by.
    ///     If <c>null</c>, all types are included in the results.
    /// </summary>
    public int? TypeId { get; set; }

    /// <summary>
    ///     Determines how the results should be sorted.
    ///     Uses the <see cref="ProductSortingOptions"/> enum (e.g., NameAscending, PriceDescending).
    ///     Defaults to <see cref="ProductSortingOptions.nameAsc"/> if not specified.
    /// </summary>
    public ProductSortingOptions sort { get; set; } = ProductSortingOptions.nameAsc; // => SortingOptions

    /// <summary>
    ///     (Optional) A free-text search keyword used to filter products by name or description.
    ///     If specified, only products whose name or description contains this value will be included in the results.
    /// </summary>
    /// <remarks>
    ///     This parameter enables **partial text matching** within the product catalog,
    ///     allowing clients to search by keywords (e.g., "shirt", "laptop", "Nike").
    ///     It is typically implemented using a <c>WHERE</c> clause with a <c>LIKE</c> operator
    ///     or an equivalent case-insensitive comparison in LINQ.
    /// </remarks>
    /// <example>
    ///     Example usage:
    ///     <code>
    ///         // Retrieve products whose names contain "shoe"
    ///         GET /api/Products?searchValue=shoe
    ///     </code>
    /// </example>
    public string? Search { get; set; } // => SearchValue

    /// <summary>
    ///     The default number of items to display per page.
    /// </summary>
    private const int DefaultPageSize = 5;

    /// <summary>
    ///     The maximum allowed number of items per page.
    /// </summary>
    private const int MaxPageSize = 10;

    /// <summary>
    ///     The current page index (starts from 1).
    /// </summary>
    public int PageNumber { get; set; } = 1; // => PageIndex

    /// <summary>
    ///     The backing field for the <see cref="PageSize"/> property.
    /// </summary>
    private int pageSize { get; set; } = DefaultPageSize;

    /// <summary>
    ///     Gets or sets the number of items per page.
    ///     If the assigned value exceeds <see cref="MaxPageSize"/>,
    ///     it will be automatically limited to the maximum allowed value.
    /// </summary>
    public int PageSize
    {
        get => pageSize;
        set => pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}
