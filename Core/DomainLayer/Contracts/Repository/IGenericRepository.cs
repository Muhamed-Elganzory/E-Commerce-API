using DomainLayer.Contracts.Spec;
using DomainLayer.Models.Shared;

namespace DomainLayer.Contracts.Repository
{
    /// <summary>
    ///     Generic Repository Interface.
    ///     Defines a set of common CRUD operations that can be applied to any entity type
    ///     inheriting from <see cref="BaseEntity{TKey}"/>.
    ///     This interface promotes abstraction, code reusability, and separation of concerns.
    /// </summary>
    /// <remarks>
    ///     Acts as an intermediary between the data access layer and the business logic layer.
    ///     It defines *what* operations can be performed on data without exposing *how* they are implemented.
    /// </remarks>
    /// <typeparam name="TEntity">The type of the entity managed by the repository (e.g., <c>Product</c>).</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key (e.g., <c>int</c>, <c>Guid</c>, <c>string</c>).</typeparam>
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        // ───────────────────────────────
        // 📦 BASIC CRUD OPERATIONS
        // ───────────────────────────────

        /// <summary>
        ///     Asynchronously retrieves all entities of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <returns>
        ///     A task representing the asynchronous operation.
        ///     The task result contains a collection of all entities.
        /// </returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        ///     Asynchronously retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The primary key of the entity to retrieve.</param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        ///     The task result contains the entity if found, otherwise <c>null</c>.
        /// </returns>
        Task<TEntity?> GetByIdAsync(TKey id);

        /// <summary>
        ///     Asynchronously creates a new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity instance to add (e.g., <c>Product</c>).</param>
        Task CreateAsync(TEntity entity);

        /// <summary>
        ///     Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity instance to update (e.g., <c>Product</c>).</param>
        void Update(TEntity entity);

        /// <summary>
        ///     Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity instance to delete (e.g., <c>Product</c>).</param>
        void Delete(TEntity entity);

        // ───────────────────────────────
        // 🧩 SPECIFICATION-BASED OPERATIONS
        // ───────────────────────────────

        /// <summary>
        ///     Retrieves all entities that satisfy the given specification.
        ///     The specification defines the filtering (criteria), eager loading (includes), and other query rules.
        /// </summary>
        /// <param name="baseSpecification">
        ///     The <see cref="IBaseSpecification{TEntity, TKey}"/> that encapsulates the query logic.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        ///     The task result contains a list of entities matching the specification.
        /// </returns>
        Task<List<TEntity>> GetAllAsync(IBaseSpecification<TEntity, TKey> baseSpecification);

        /// <summary>
        ///     Retrieves a single entity that satisfies the given specification.
        ///     The specification defines filtering and eager loading rules.
        /// </summary>
        /// <param name="baseSpecification">
        ///     The <see cref="IBaseSpecification{TEntity, TKey}"/> instance containing query rules.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        ///     The task result contains the matching entity or <c>null</c> if no match is found.
        /// </returns>
        Task<TEntity?> GetByIdAsync(IBaseSpecification<TEntity, TKey> baseSpecification);

        /// <summary>
        ///     Asynchronously counts the total number of entities that match the given specification criteria.
        /// </summary>
        /// <param name="baseSpecification">
        ///     The specification that defines the filtering and conditions used to count matching entities.
        /// </param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains the total number of entities that satisfy the specified criteria.
        /// </returns>
        Task<int> CountAsync(IBaseSpecification<TEntity, TKey> baseSpecification);

    }
}