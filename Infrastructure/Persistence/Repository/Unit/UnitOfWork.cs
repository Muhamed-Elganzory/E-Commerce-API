using DomainLayer.Contracts.Repository;
using DomainLayer.Contracts.Unit;
using DomainLayer.Models.Shared;
using Persistence.DB;

namespace Persistence.Repository.Unit;

/// <summary>
///     UnitOfWork Implementation
///     This class provides a concrete implementation of the IUnitOfWork interface,
///     allowing for the management of multiple repository instances and coordinating their operations within a single transaction.
/// </summary>
/// <param name="dbContext">dbContext: The database context used to interact with the database.</param>
public class UnitOfWork(StoreDbContext dbContext): IUnitOfWork
{
    // DbContext
    private readonly StoreDbContext _dbContext = dbContext;

    // Dictionary to hold repository instances
    private readonly Dictionary<string, object> _dictionaryRepositories = [];

    /// <summary>
    ///     CreateRepositoryAsync retrieves a generic repository for the specified entity type and key type asynchronously.
    ///     This way, you can get a repository for any entity without needing to define specific properties for each one.
    /// </summary>
    /// <remarks>
    ///     There are three common ways to manage repository instances in Unit of Work:
    ///         1- Direct Property:
    ///             - Create a property for each repository (e.g., ProductRepository).
    ///             - Simple, but creates all instances even if not used.
    ///         2- Lazy Initialization:
    ///             - Use Lazy so the repository is only created when first accessed.
    ///             - Improves performance when many repositories exist.
    ///         3- Dictionary-based Caching (this approach):
    ///             - Store repositories in a dictionary by type name.
    ///             - Creates each repository once, only when requested, and reuses it later.
    /// </remarks>
    /// <typeparam name="TEntity">TEntity is constrained to be a type that inherits from BaseEntity Like: Product</typeparam>
    /// <typeparam name="TKey">TKey is constrained to be a type that inherits from BaseEntity Like: Product ID</typeparam>
    /// <returns></returns>
    public async Task<IGenericRepository<TEntity, TKey>> CreateRepositoryAsync<TEntity, TKey>() where TEntity : BaseEntity<TKey>
    {
        // 1️⃣ Get the entity type name ("Product")
        var typeName = typeof(TEntity).Name;

        // 2️⃣ Dictionary<string, object> string = Type Name, object = Repository Instance
        // Check if the repository already exists in the dictionary
        // if (!_dictionaryRepositories.ContainsKey(typeName)) TODO: Old way to check prefer use TryGetValue
        if (!_dictionaryRepositories.TryGetValue(typeName, out var existingRepository))
        {
            // 3️⃣ Create a new instance of GenericRepository
            var repositoryInstance = new GenericRepository<TEntity, TKey>(_dbContext);

            // Add the repository instance to the dictionary
            // _dictionaryRepositories.Add(typeName, repositoryInstance);

            // 4️⃣ Store it in the dictionary using indexer syntax
            _dictionaryRepositories[typeName] = repositoryInstance;

            // 5️⃣ Return the created repository instance as a Task
            return await Task.FromResult<IGenericRepository<TEntity, TKey>>(repositoryInstance);
        }

        // The way to cast object to IGenericRepository<TEntity, TKey> it use that if you work sync method
        // (IGenericRepository<TEntity, TKey>)_dictionaryRepositories[typeName]

        // The way to cast object to IGenericRepository<TEntity, TKey> it use that if you work async method
        // 6️⃣ If the repository exists, return it (after casting)
        return await Task.FromResult((IGenericRepository<TEntity, TKey>)existingRepository);
    }

    /// <summary>
    ///     Saves all changes made in the context to the database asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}