namespace serviceAbstraction.Contracts.Redis
{
    /// <summary>
    ///     Provides high-level operations for working with Redis caching.
    /// <para>
    ///     This service abstracts caching logic from the underlying Redis repository.
    ///     Used to store and retrieve cached values with optional expiration (TTL).
    /// </para>
    /// </summary>
    public interface ICashService
    {
        /// <summary>
        ///     Stores an object value in the cache under the specified key.
        /// <para>
        ///     The object will be serialized (typically to JSON) before being stored.
        ///     The entry will expire automatically after the provided Time-To-Live (TTL).
        /// </para>
        /// </summary>
        /// <param name="cashKey">
        ///     A unique key used to identify the cached entry.
        /// </param>
        /// <param name="cashValue">
        ///     The object to store in cache; typically DTOs or domain objects.
        /// </param>
        /// <param name="timeToLive">
        ///     The period after which the cached entry will expire and be removed.
        /// </param>
        /// <returns>
        ///     A task that represents the asynchronous write operation.
        /// </returns>
        public Task SetAsync(string cashKey, object cashValue, TimeSpan timeToLive);

        /// <summary>
        ///     Retrieves a cached value using the provided key.
        /// <para>
        ///     The method returns the raw cached string (usually JSON) or <c>null</c>
        ///     if no value exists for the given key.
        /// </para>
        /// </summary>
        /// <param name="cashKey">
        ///     The key used to locate the cached value.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation,
        ///     containing the cached string value or <c>null</c> if not found.
        /// </returns>
        public Task<string?> GetAsync(string cashKey);
    }
}