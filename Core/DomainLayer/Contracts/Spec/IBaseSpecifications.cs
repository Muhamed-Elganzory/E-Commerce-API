using System.Linq.Expressions;
using DomainLayer.Models.Shared;

namespace DomainLayer.Contracts.Spec;

/// <summary>
///     Represents a specification for querying entities.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The primary key type.</typeparam>
public interface IBaseSpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    /// <summary>
    ///     Criteria is a filtering condition (WHERE clause).
    /// </summary>
    public Expression<Func<TEntity, bool>>? Criteria { get; set; }

    /// <summary>
    ///     IncludeExpression is a related entities to include (navigation properties).
    /// </summary>
    public List<Expression<Func<TEntity, object>>>? IncludeExpression { get; }

    /// <summary>
    ///     Gets or sets the ascending order expression for sorting query results.
    ///     When specified, the query results will be ordered in ascending order based on the provided property expression.
    /// </summary>
    /// <example>
    ///     <code>
    ///     OrderByAscending = p => p.Name;
    ///     </code>
    ///     This will sort products by their <c>Name</c> in ascending order.
    /// </example>
    public Expression<Func<TEntity, object>>? OrderByAscending { get; set; }

    /// <summary>
    ///     Gets or sets the descending order expression for sorting query results.
    ///     When specified, the query results will be ordered in descending order based on the provided property expression.
    /// </summary>
    /// <example>
    ///     <code>
    ///     OrderByDescending = p => p.Price;
    ///     </code>
    ///     This will sort products by their <c>Price</c> in descending order.
    /// </example>
    public Expression<Func<TEntity, object>>? OrderByDescending { get; set; }

    /// <summary>
    ///     Number of items to retrieve per page (page size).
    /// </summary>
    public int Take { get; set; }

    /// <summary>
    ///     Number of items to skip before starting to take results.
    ///     Calculated as (pageIndex - 1) * pageSize.
    /// </summary>
    public int Skip { get; set; }

    /// <summary>
    ///     Indicates whether pagination should be applied or not.
    /// </summary>
    public bool IsPaginated { get; set; }
}
