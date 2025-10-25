namespace Shared.Enums.Product;

/// <summary>
///     Defines the available sorting options for product queries.
///     Used to specify how the product list should be ordered when retrieved from the repository or service.
/// </summary>
/// <remarks>
///     This enum can be used in combination with specifications or query parameters in API endpoints
///     to apply dynamic ordering to product results.
/// </remarks>
/// <example>
///     Example usage:
///     <code>
///         var sortOption = ProductSortingOptions.PriceDescending;
///         var specification = new ProductWithSortingSpecification(sortOption);
///     </code>
/// </example>
public enum ProductSortingOptions
{
    /// <summary>
    ///     Sort products alphabetically by name (A → Z).
    /// </summary>
    NameAscending = 1,

    /// <summary>
    ///     Sort products alphabetically by name (Z → A).
    /// </summary>
    NameDescending = 2,

    /// <summary>
    ///     Sort products by price from lowest to highest.
    /// </summary>
    PriceAscending = 3,

    /// <summary>
    ///     Sort products by price from highest to lowest.
    /// </summary>
    PriceDescending = 4,
}
