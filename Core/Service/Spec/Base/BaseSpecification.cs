using System.Linq.Expressions;
using DomainLayer.Contracts.Spec;
using DomainLayer.Models.Shared;

namespace Service.Spec.Base
{
    /// <summary>
    ///     BaseSpecification provides a base implementation for creating specifications.
    ///     It defines filtering criteria and includes related entities dynamically.
    ///     After all steps go to SpecificationEvaluator class to
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseSpecification{TEntity, TKey}"/> class
        ///     with a filtering expression (criteria).
        /// </summary>
        /// <param name="criteriaExpression">The expression that defines the filtering logic.</param>
        protected BaseSpecification(Expression<Func<TEntity, bool>>? criteriaExpression)
        {
            Criteria = criteriaExpression;
        }

        /// <summary>
        ///     Gets the filtering expression used to select entities.
        /// </summary>
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }

        /// <summary>
        ///     Gets the list of include expressions used for eager loading related entities.
        /// </summary>
        public List<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];

        /// <summary>
        ///     Adds a new include expression to the specification.
        /// </summary>
        /// <param name="includeExpression">The expression representing the related entity to include.</param>
        /// <remarks>
        ///     <b>Why implement another function (AddInclude) ?</b><br/>
        ///     The reason for having a separate <see cref="AddInclude"/> method is to provide
        ///     a clean and consistent way to add include expressions (for eager loading)
        ///     without directly exposing the internal list. <br/>
        ///     This improves encapsulation and allows subclasses to easily extend or modify
        ///     which navigation properties should be included when querying the database.
        /// </remarks>
        public void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpression.Add(includeExpression);
        }
    }
}
