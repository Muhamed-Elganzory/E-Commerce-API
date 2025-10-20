using DomainLayer.Contracts.Repository;
using DomainLayer.Models.Shared;

namespace DomainLayer.Contracts.Unit;

/// <summary>
///     IUnitOfWork Interface
///     This interface defines the contract for a Unit of Work pattern implementation.
///     It provides a way to manage and coordinate multiple repository operations within a single transaction.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     CreateRepositoryAsync retrieves a generic repository for the specified entity type and key type asynchronously.
    ///     This way, you can get a repository for any entity without needing to define specific properties for each one.
    /// </summary>
    /// <remarks>
    ///     Where TEntity is constrained to be a type that inherits from BaseEntity
    /// </remarks>
    /// <typeparam name="TEntity">Like: Product</typeparam>
    /// <typeparam name="TKey">Like: Product ID</typeparam>
    /// <returns></returns>
    public Task<IGenericRepository<TEntity, TKey>> CreateRepositoryAsync <TEntity, TKey>() where TEntity: BaseEntity <TKey>;

    /// <summary>
    ///     Saves all changes made in the context to the database asynchronously.
    /// </summary>
    /// <returns>Returns the number of state entries written to the database.</returns>
    public Task <int> SaveChangesAsync();
}