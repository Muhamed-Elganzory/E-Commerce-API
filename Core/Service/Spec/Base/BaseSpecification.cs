using System.Linq.Expressions;
using DomainLayer.Contracts.Spec;
using DomainLayer.Models.Shared;
using Service.Spec.Helper;

namespace Service.Spec.Base
{
    /// <summary>
    ///     Represents the base implementation of a specification used to define query logic
    ///     for a specific entity type.
    ///     It allows dynamically building LINQ expressions for filtering, sorting,
    ///     and eager loading related entities.
    /// </summary>
    /// <remarks>
    ///     The <see cref="BaseSpecification{TEntity, TKey}"/> class acts as the foundation
    ///     for all concrete specifications (such as <c>ProductWithBrandAndTypeBaseSpecifications</c>).
    ///     It defines reusable properties and methods — like <c>Criteria</c> and <c>AddInclude()</c> —
    ///     that describe *what data to query* and *how to load it*.
    ///     <br/><br/>
    ///     After defining the specification, the <see cref="SpecificationEvaluator"/> class
    ///     is responsible for transforming it into an executable LINQ query,
    ///     applying filtering, sorting, and include expressions before executing it against the database.
    /// </remarks>
    /// <typeparam name="TEntity">The entity type (e.g., <c>Product</c>).</typeparam>
    /// <typeparam name="TKey">The key type (e.g., <c>int</c> or <c>Guid</c>).</typeparam>
    public abstract class BaseSpecification<TEntity, TKey> : IBaseSpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        /// <summary>
        ///     Gets or sets the filtering expression used to define selection criteria for the query.
        ///     This expression represents the <c>WHERE</c> condition in LINQ, used to filter which entities should be returned.
        /// </summary>
        /// <example>
        ///     <code>
        ///         Criteria = product => product.Price > 100 and product.IsAvailable;
        ///     </code>
        ///     This will return only products priced above 100 that are available.
        /// </example>
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }

        /// <summary>
        ///     Gets the collection of expressions specifying navigation properties for eager loading.
        ///     Each expression corresponds to an <c>Include</c> statement in Entity Framework,
        ///     allowing related entities to be loaded as part of the same query.
        /// </summary>
        /// <example>
        ///     <code>
        ///     IncludeExpression.Add(p => p.ProductBrand);
        ///     IncludeExpression.Add(p => p.ProductType);
        ///     </code>
        ///     This ensures that both <c>ProductBrand</c> and <c>ProductType</c> are loaded with each product.
        /// </example>
        public List<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];

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

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseSpecification{TEntity,TKey}"/> class
        ///     with a filtering expression (criteria).
        /// </summary>
        /// <param name="criteriaExpression">The expression that defines the filtering logic.</param>
        protected BaseSpecification(Expression<Func<TEntity, bool>>? criteriaExpression)
        {
            Criteria = criteriaExpression;
        }

        /// <summary>
        ///     Adds a new include expression to the baseSpecification.
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
        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpression.Add(includeExpression);
        }

        /// <summary>
        ///     Sets the ascending order expression for the specification query.
        /// </summary>
        /// <param name="orderByAscending">
        ///     An expression that defines the property by which to sort results in ascending order.
        ///     Example: <c>x => x.Name</c>
        /// </param>
        protected void AddOrderByAscending(Expression<Func<TEntity, object>> orderByAscending)
        {
            OrderByAscending = orderByAscending;
        }

        /// <summary>
        ///     Sets the descending order expression for the specification query.
        /// </summary>
        /// <param name="orderByDescending">
        ///     An expression that defines the property by which to sort results in descending order.
        ///     Example: <c>x => x.Price</c>
        /// </param>
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
        }

        /// <summary>
        ///     Applies pagination settings based on the provided page size and page index.
        ///     Calculates how many records to skip and how many to take for the current page.
        /// </summary>
        /// <param name="pageSize">The number of items to include per page.</param>
        /// <param name="pageIndex">The current page index (starting from 1).</param>
        /// <example>
        ///     <code>
        ///         Total = 100 Products  ==>  100 / 10 = 10 Pages
        ///
        ///         PageSize = 10, PageIndex = 1
        ///         Take = 10
        ///         Skip = (1 - 1) * 10 = 0
        ///
        ///         PageSize = 10, PageIndex = 3
        ///         Take = 10
        ///         Skip = (3 - 1) * 10 = 20
        ///     </code>
        /// </example>
        /// <remarks>
        ///     PageIndex starts from 1.
        ///     For example, pageIndex = 1 will return the first page.
        /// </remarks>
        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index must be greater than zero.");
            }

            IsPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }
    }
}
