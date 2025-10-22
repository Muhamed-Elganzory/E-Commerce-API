using DomainLayer.Contracts.Spec;
using DomainLayer.Models.Shared;

namespace DomainLayer.Contracts.Repository;

/// <summary>
///     Generic Repository Interface
///     This interface defines the contract for a generic repository that can handle CRUD operations
///     for any entity type that extends BaseEntity with a specified key type.
///     It provides a way to abstract data access logic and promote code reusability.
/// </summary>
/// <remarks>
///     The repository is a layer between the data access layer and the business logic layer.
///     It is used to define what data the application needs from the data access layer,
///     without exposing how that data is retrieved or persisted.
/// </remarks>
/// <remarks>
///     TEntity: The type of the entity that the repository will manage. It must inherit from BaseEntity.
///     TKey: The type of the primary key for the entity (e.g., int, Guid, string).
/// </remarks>
/// <typeparam name="TEntity">Like: Product</typeparam>
/// <typeparam name="TKey">Like: int</typeparam>
public interface IGenericRepository <TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    // Get all entities
    /// <summary>
    ///     Gets all entities of type TEntity from the repository.
    /// </summary>
    /// <returns>Returns a collection of all entities of type TEntity.</returns>
    public Task <IEnumerable <TEntity>> GetAllAsync ();

    // Get entity by id
    /// <summary>
    ///     GetByIdAsync retrieves an entity by its primary key.
    /// </summary>
    /// <param name="id">The primary key of the entity to retrieve.</param>
    /// <returns>Returns the entity with the specified id, or null if not found.</returns>
    public Task<TEntity?> GetByIdAsync (TKey id);

    // Add entity
    /// <summary>
    ///     Create a new entity in the repository.
    /// </summary>
    /// <param name="entity">Like Product</param>
    /// <returns></returns>
    public Task CreateAsync (TEntity entity);

    // Update entity
    /// <summary>
    ///     Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">Like Product</param>
    /// <returns></returns>
    public void Update (TEntity entity);

    // Delete entity
    /// <summary>
    ///     Deletes an entity from the repository.
    /// </summary>
    /// <param name="entity">Like Product</param>
    public void Delete (TEntity entity);

    // Get all entities but, with criteria by Specification that to handle
    /// <summary>
    ///     Retrieves all entities that satisfy the given specification criteria.
    /// </summary>
    /// <param name="specification">
    ///     The specification that defines the filtering, sorting, and including rules for the query.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains an enumerable collection of entities that match the specification.
    /// </returns>
    public Task<List<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification);

    // Get entity by id but, with criteria by Specification that to handle
    /// <summary>
    ///     Retrieves a single entity that matches the given specification criteria.
    /// </summary>
    /// <param name="specification">
    ///     The specification that defines the filtering, sorting, and including rules for the entity query.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the entity that matches the specification, or <c>null</c> if no match is found.
    /// </returns>
    public Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification);
}