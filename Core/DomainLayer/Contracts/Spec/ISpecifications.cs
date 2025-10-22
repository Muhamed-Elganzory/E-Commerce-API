using System.Linq.Expressions;
using DomainLayer.Models.Shared;

namespace DomainLayer.Contracts.Spec;

/// <summary>
///     Represents a specification for querying entities.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The primary key type.</typeparam>
public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    /// <summary>
    ///     Criteria is a filtering condition (WHERE clause).
    /// </summary>
    public Expression<Func<TEntity, bool>>? Criteria { get; set; }

    /// <summary>
    ///     IncludeExpression is a related entities to include (navigation properties).
    /// </summary>
    public List<Expression<Func<TEntity, object>>>? IncludeExpression { get; }
}
