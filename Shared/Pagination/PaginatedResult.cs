namespace Shared.Pagination
{
    /// <summary>
    ///     Represents a paginated result that contains a subset of data along with pagination metadata.
    /// </summary>
    /// <typeparam name="TEntity">The type of data items returned in the paginated result.</typeparam>
    public class PaginatedResult<TEntity>
    {
        /// <summary>
        ///     Gets or sets the current page index (starting from 1).
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     Gets or sets the number of items displayed per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     Gets or sets the total number of pages available for the given dataset.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        ///     Gets or sets the collection of items contained in the current page.
        /// </summary>
        public IEnumerable<TEntity> Data { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PaginatedResult{TEntity}"/> class
        ///     with the specified pagination details and data.
        /// </summary>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="pageIndex">The current page index (starting from 1).</param>
        /// <param name="totalCount">The total number of available pages.</param>
        /// <param name="data">The collection of items in the current page.</param>
        public PaginatedResult(int pageSize, int pageIndex, int totalCount, IEnumerable<TEntity> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalCount = totalCount;
            Data = data;
        }
    }
}