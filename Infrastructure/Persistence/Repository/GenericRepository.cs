using Persistence.DB;
using DomainLayer.Models.Shared;
using Microsoft.EntityFrameworkCore;
using DomainLayer.Contracts.Repository;
using DomainLayer.Contracts.Spec;
using Service.Spec.Helper;

namespace Persistence.Repository;

/// <summary>
///     Generic Repository Implementation
///     This class provides a concrete implementation of the IGenericRepository interface,
///     allowing for CRUD operations on any entity type that extends BaseEntity with a specified key type
/// </summary>
/// <param name="dbContext">dbContext: The database context used to interact with the database.</param>
/// <typeparam name="TEntity">TEntity: The type of the entity that the repository will manage. It must inherit from BaseEntity.</typeparam>
/// <typeparam name="TKey">TKey: The type of the primary key for the entity (e.g., int, Guid, string).</typeparam>
public class GenericRepository<TEntity, TKey>(StoreDbContext dbContext): IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    private readonly StoreDbContext _dbContext = dbContext;

    /// <summary>
    ///     Gets all entities of type TEntity from the repository asynchronously.
    /// </summary>
    /// <returns>Returns a collection of all entities of type TEntity.</returns>
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        // Set<TEntity>(): This method is used to get a DbSet<TEntity> instance for the specified entity type TEntity.
        // ToListAsync(): This method is used to asynchronously execute the query and return the results as a list.
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    /// <summary>
    ///     GetByIdAsync retrieves an entity by its primary key asynchronously.
    /// </summary>
    /// <param name="id">The primary key of the entity to retrieve.</param>
    /// <returns>Returns the entity with the specified id, or null if not found.</returns>
    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        // Set<TEntity>(): This method is used to get a DbSet<TEntity> instance for the specified entity type TEntity.
        // FindAsync(id): This method is used to find an entity with the specified primary key value asynchronously.
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    /// <summary>
    ///     Create a new entity in the repository asynchronously.
    /// </summary>
    /// <param name="entity">Like: Product</param>
    public async Task CreateAsync(TEntity entity)
    {
        // Set<TEntity>(): This method is used to get a DbSet<TEntity> instance for the specified entity type TEntity.
        // CreateAsync(entity): This method is used to add a new entity to the DbSet asynchronously.
        await _dbContext.Set<TEntity>().AddAsync(entity);
    }

    /// <summary>
    ///     Update an existing entity in the repository.
    /// </summary>
    /// <param name="entity">Like: Product</param>
    public void Update(TEntity entity)
    {
        // Set<TEntity>(): This method is used to get a DbSet<TEntity> instance for the specified entity type TEntity.
        // Update(entity): This method is used to update the specified entity in the DbSet.
        _dbContext.Set<TEntity>().Update(entity);
    }

    /// <summary>
    ///     Delete an entity from the repository.
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(TEntity entity)
    {
        // Hard Delete
        // Set<TEntity>(): This method is used to get a DbSet<TEntity> instance for the specified entity type TEntity.
        // Remove(entity): This method is used to remove the specified entity from the DbSet.
        _dbContext.Set<TEntity>().Remove(entity);
    }

    /// <summary>
    ///     Retrieves all entities that satisfy the given specification criteria.
    /// </summary>
    /// <param name="specification">
    ///     The specification object that defines filtering, including, sorting, or paging rules to apply.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation.
    ///     The task result contains a list of entities that match the provided specification.
    /// </returns>
    public async Task<List<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification)
    {
        // Retrieves all entities that match the specification criteria,
        // including any related entities defined via Include expressions.
        return await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification).ToListAsync();
    }

    /// <summary>
    ///     Retrieves a single entity that matches the given specification criteria.
    /// </summary>
    /// <param name="specification">
    ///     The specification object that defines the filtering and include rules for querying a specific entity.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation.
    ///     The task result contains the entity that matches the specification, or <c>null</c> if no entity is found.
    /// </returns>
    public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification)
    {
        // Retrieves a single entity matching the specification criteria,
        // Including any related entities defined in Include expressions.
        // Returns null if no matching entity is found.
        return await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification).FirstOrDefaultAsync();
    }
}