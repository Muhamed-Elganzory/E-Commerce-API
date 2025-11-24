namespace Shared.Enums.Product
{
    /// <summary>
    ///     Sorting options for product listing in API queries.
    /// </summary>
    public enum ProductSortingOptions
    {
        /// <summary>
        ///     Sort products alphabetically (A → Z).
        /// </summary>
        nameAsc = 1,

        /// <summary>
        ///     Sort products alphabetically (Z → A).
        /// </summary>
        nameDesc = 2,

        /// <summary>
        ///     Sort products by price (lowest → highest).
        /// </summary>
        priceAsc = 3,

        /// <summary>
        ///     Sort products by price (highest → lowest).
        /// </summary>
        priceDesc = 4
    }
}
