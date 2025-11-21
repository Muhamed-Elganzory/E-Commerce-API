using DomainLayer.Contracts.Repository.Redis;
using StackExchange.Redis;

namespace Persistence.Repository.Redis;

/// <summary>
///     Redis cache repository that provides basic operations for setting
///     and retrieving cached values using StackExchange.Redis.
///
///     This implementation uses <see cref="IConnectionMultiplexer"/> to manage
///     Redis connections and <see cref="IDatabase"/> to execute cache operations.
/// </summary>
/// <param name="connectionMultiplexer">
///     The Redis connection multiplexer responsible for managing pooled connections.
/// </param>
public class CashRepository(IConnectionMultiplexer connectionMultiplexer) : ICashRepository
{
    /// <summary>
    ///     Holds the shared connection multiplexer instance.
    ///     Used to access Redis databases and manage efficient connections.
    /// </summary>
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;

    /// <summary>
    ///     Represents a logical Redis database instance used for executing
    ///     cache operations such as StringSet and StringGet.
    /// </summary>
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    /// <summary>
    ///     Stores a string value inside Redis using a specific key.
    /// </summary>
    /// <param name="cashKey">
    ///     A unique cache key used to store and later retrieve the value.
    /// </param>
    /// <param name="cashValue">
    ///     The string value to be cached.
    /// </param>
    /// <param name="timeToLive">
    ///     Determines how long the value should remain in the cache
    ///     before expiring automatically.
    /// </param>
    public async Task SetAsync(string cashKey, string cashValue, TimeSpan timeToLive)
    {
        // Stores the given value in Redis under the specified key with an expiration time (TTL).
        await _database.StringSetAsync(cashKey, cashValue, timeToLive);
    }

    /// <summary>
    ///     Retrieves a cached value from Redis by its key.
    /// </summary>
    /// <param name="cashKey">
    ///     The key used to locate the cached value.
    /// </param>
    /// <returns>
    ///     The cached value as a string if found; otherwise, null.
    /// </returns>
    public async Task<string?> GetAsync(string cashKey)
    {
        // Retrieve the value from Redis
        var cashingValues = await _database.StringGetAsync(cashKey);

        // Convert to nullable string
        return cashingValues.HasValue ? cashingValues.ToString() : null;
    }
}