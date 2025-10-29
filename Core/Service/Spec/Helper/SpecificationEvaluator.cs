using DomainLayer.Models.Shared;
using DomainLayer.Contracts.Spec;
using Microsoft.EntityFrameworkCore;

namespace Service.Spec.Helper
{
    /// <summary>
    ///     A helper class responsible for converting a baseSpecification into a LINQ query.
    ///     It applies filtering (Criteria) and eager loading (Include expressions)
    ///     on top of a provided <see cref="IQueryable{TEntity}"/>.
    /// </summary>
    /// <example>
    ///     <code>
    ///     // Example of what SpecificationEvaluator builds internally:
    ///     // Equivalent to:
    ///         _dbContext.Products
    ///             .Where(p => p.id == id)
    ///             .Include(p => p.ProductType)
    ///             .Include(p => p.ProductBrand);
    ///     </code>
    /// </example>
    /// <remarks>
    ///     <b>Why implement this helper class?</b><br/>
    ///     Instead of repeating the same logic in every repository (like applying criteria and includes),
    ///     the <see cref="SpecificationEvaluator"/> centralizes that behavior in one place. <br/>
    ///     This improves code reuse, keeps repositories clean, and ensures all specifications
    ///     are applied consistently across the application.
    /// </remarks>
    public static class SpecificationEvaluator
    {
        /// <summary>
        ///     Builds a query based on the provided baseSpecification by applying filtering and include expressions.
        ///     Represents a query that is not executed immediately.
        ///     The <see cref="IQueryable"/> interface builds an expression tree
        ///     that Entity Framework Core translates into SQL when the query
        ///     is materialized (e.g., by calling <c>ToListAsync()</c> or <c>FirstOrDefaultAsync()</c>).
        /// </summary>
        /// <typeparam name="TEntity">The entity type (e.g., Product).</typeparam>
        /// <typeparam name="TKey">The entity key type (e.g., ProductId).</typeparam>
        /// <param name="inputQuery">A DbSet&lt;TEntity&gt; from the DbContext (e.g., dbContext.Products).</param>
        /// <param name="baseSpecification">The baseSpecification that defines filtering and include rules.</param>
        /// <returns>
        ///     An <see cref="IQueryable{TEntity}"/> representing the composed query according to the baseSpecification.
        /// </returns>
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, IBaseSpecification<TEntity, TKey> baseSpecification) where TEntity : BaseEntity<TKey>
        {
            // Start from the base query (DbSet<TEntity>)
            var query = inputQuery;

            // Apply filtering (WHERE clause) if a Criteria expression is provided
            if (baseSpecification.Criteria is not null)
            {
                query = query.Where(baseSpecification.Criteria);
            }

            // Apply sorting by order by (Order Ascending)
            if (baseSpecification.OrderByAscending is not null)
            {
                query = query.OrderBy(baseSpecification.OrderByAscending);
            }

            // Apply sorting by order by descending (Order Descending)
            if (baseSpecification.OrderByDescending is not null)
            {
                query = query.OrderByDescending(baseSpecification.OrderByDescending);
            }

            // Apply eager loading (INCLUDE clauses) if Include expressions exist
            if (baseSpecification.IncludeExpression is not null && baseSpecification.IncludeExpression.Count >= 0)
            {
                // Aggregate() iterates through each Include expression and applies it to the current query.
                // - query: the initial DbSet<TEntity> (e.g., dbContext.Products)
                // - currentQuery: represents the query as it is being built after each Include()
                // - includeExpression: the current Include expression (e.g., p => p.ProductType)
                query = baseSpecification.IncludeExpression.Aggregate(
                    query,
                    (currentQuery, includeExpression) => currentQuery.Include(includeExpression)
                );
            }

            // Apply pagination only if enabled in the base specification
            if (baseSpecification.IsPaginated)
            {
                // Skip the specified number of records and take the defined page size
                query = query.Skip(baseSpecification.Skip).Take(baseSpecification.Take);
            }

            // Return the composed query
            return query;
        }
    }
}
