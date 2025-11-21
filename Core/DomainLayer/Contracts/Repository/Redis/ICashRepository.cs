namespace DomainLayer.Contracts.Repository.Redis
{
    /// <summary>
    ///     Represents a contract for interacting with a distributed Redis cache.
    ///     Provides basic operations for storing and retrieving cached data.
    /// </summary>
    public interface ICashRepository
    {
        /// <summary>
        ///     Stores a string value inside the Redis cache using the given key.
        /// </summary>
        /// <param name="cashKey">
        ///     The unique identifier used to store and retrieve the cached value.
        /// </param>
        /// <param name="cashValue">
        ///     The data to be cached. Must be serializable to a string format.
        /// </param>
        /// <param name="timeToLive">
        ///     Determines how long the cached value should remain before it expires automatically.
        ///     Used to control cache invalidation and prevent stale data.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        /// </returns>
        public Task SetAsync(string cashKey, string cashValue, TimeSpan timeToLive);

        /// <summary>
        ///     Retrieves a cached string value from Redis using the provided key.
        /// </summary>
        /// <param name="cashKey">
        ///     The unique key used to locate the cached item.
        /// </param>
        /// <returns>
        ///     The cached value if found; otherwise, null.
        /// </returns>
        public Task<string?> GetAsync(string cashKey);
    }
}