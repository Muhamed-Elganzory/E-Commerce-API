using System.Text.Json;
using serviceAbstraction.Contracts.Redis;
using DomainLayer.Contracts.Repository.Redis;

namespace Service.Redis
{
    /// <summary>
    ///     Provides high-level caching operations built on top of the Redis repository.
    /// <para>
    ///     This service is responsible for serializing objects before caching them
    ///     and delegating low-level Redis operations to <see cref="ICashRepository"/>.
    /// </para>
    /// <para>
    ///     Used when application services need to store frequently accessed data
    ///     such as user sessions, product lists, or temporary computations.
    /// </para>
    /// </summary>
    /// <param name="cashRepository">
    ///     The underlying Redis repository responsible for direct communication with Redis.
    /// </param>
    public class CashService(ICashRepository cashRepository) : ICashService
    {
        /// <summary>
        ///     The Redis repository for performing raw cache operations.
        /// </summary>
        private readonly ICashRepository _cashRepository = cashRepository;

        /// <summary>
        ///     Stores an object in the cache under the specified key.
        /// <para>
        ///     The object is serialized to JSON before being saved.
        ///     The entry will expire automatically after the specified time-to-live (TTL).
        /// </para>
        /// </summary>
        /// <param name="cashKey">
        ///     The unique cache key to associate with this value.
        /// </param>
        /// <param name="cashValue">
        ///     Any serializable .NET object to be cached (DTOs, ViewModels, anonymous objects...).
        /// </param>
        /// <param name="timeToLive">
        ///     How long the cached value should remain before expiration.
        /// </param>
        public async Task SetAsync(string cashKey, object cashValue, TimeSpan timeToLive)
        {
            // Serialize the object to a JSON string for storage
            var valueSerialized = JsonSerializer.Serialize(cashValue);

            // Save the serialized value into Redis with the provided TTL
            await _cashRepository.SetAsync(cashKey, valueSerialized, timeToLive);
        }

        /// <summary>
        ///     Retrieves a cached value as a raw JSON string.
        /// <para>
        ///     If the key does not exist or has expired, <c>null</c> is returned.
        ///     Consumers are responsible for deserializing the result back to the desired type.
        /// </para>
        /// </summary>
        /// <param name="cashKey">
        ///     The key used to look up the cached value.
        /// </param>
        /// <returns>
        ///     The cached JSON string if found; otherwise, <c>null</c>.
        /// </returns>
        public async Task<string?> GetAsync(string cashKey)
        {
            return await _cashRepository.GetAsync(cashKey);
        }
    }
}