using System.Text.Json;
using DomainLayer.Contracts.Repository.Basket;
using DomainLayer.Models.Basket;
using StackExchange.Redis;

namespace Persistence.Repository.Basket;

/// <summary>
///     Repository implementation for managing customer baskets using Redis as the storage mechanism.
///     Provides methods to create, update, retrieve, and delete baskets efficiently.
/// </summary>
/// <remarks>
///     Install Package StackExchange.Redis
/// </remarks>
/// <param name="connectionMultiplexer">
///     The <see cref="IConnectionMultiplexer"/> instance that manages the connection to the Redis server.
/// </param>
public class BasketRepository(IConnectionMultiplexer connectionMultiplexer) : IBasketRepository
{
    /// <summary>
    ///     Represents a Redis database instance used for performing operations on stored basket data.
    /// </summary>
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    /// <summary>
    ///     Creates or updates a customer's basket in Redis.
    /// </summary>
    /// <param name="basket">The basket object to be created or updated.</param>
    /// <param name="timeToLive">
    ///     Optional expiration duration. If not provided, the basket will expire after 1 day by default.
    /// </param>
    /// <returns>
    ///     The created or updated <see cref="CustomerBasket"/> if successful; otherwise, <c>null</c>.
    /// </returns>
    public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
    {
        // Convert the basket object into a JSON string
        var basketJson = JsonSerializer.Serialize(basket);

        // Store or update the basket in Redis with an expiration time
        var isCreatedOrUpdated = await _database.StringSetAsync(
            basket.Id,
            basketJson,
            timeToLive ?? TimeSpan.FromDays(1)
        );

        // If successfully created or updated, return the updated basket
        if (isCreatedOrUpdated)
            return await GetBasketAsync(basket.Id);

        // Otherwise, return null to indicate failure
        return null;
    }

    /// <summary>
    ///     Retrieves a basket from Redis by its unique key (typically the basket ID or user ID).
    /// </summary>
    /// <param name="key">The unique key of the basket to fetch.</param>
    /// <returns>
    ///     The <see cref="CustomerBasket"/> object if found; otherwise, <c>null</c> if the basket doesn't exist.
    /// </returns>
    public async Task<CustomerBasket?> GetBasketAsync(string key)
    {
        // Retrieve the serialized basket data (as a JSON string) from Redis
        var basket = await _database.StringGetAsync(key);

        // Return null if the basket does not exist
        if (basket.IsNullOrEmpty) return null;

        // Deserialize and return the basket object
        return JsonSerializer.Deserialize<CustomerBasket>(basket!);
    }

    /// <summary>
    ///     Deletes a customer's basket from Redis by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the basket to be deleted.</param>
    /// <returns>
    ///     <c>true</c> if the basket was successfully deleted; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> DeleteBasketAsync(string id)
    {
        // Delete the basket key from Redis and return the result
        return await _database.KeyDeleteAsync(id);
    }
}
