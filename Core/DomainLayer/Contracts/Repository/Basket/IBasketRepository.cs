using DomainLayer.Models.Basket;

namespace DomainLayer.Contracts.Repository.Basket;

/// <summary>
///     Defines the contract for managing customer baskets (shopping carts).
///     Provides methods to retrieve, create/update, and delete baskets — typically stored in a cache like Redis.
/// </summary>
public interface IBasketRepository
{
    /// <summary>
    ///     Creates a new basket or updates an existing one in the data store (e.g., Redis).
    /// </summary>
    /// <param name="basket">The basket object containing all basket items and data.</param>
    /// <param name="timeToLive">
    ///     (Optional) The duration the basket should remain in the cache before expiration.
    ///     If not provided, a default time may be applied.
    /// </param>
    /// <returns>
    ///     The created or updated <see cref="CustomerBasket"/> instance.
    /// </returns>
    public Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null);

    /// <summary>
    ///     Retrieves a customer's basket by its unique key (usually the basket ID or user ID).
    /// </summary>
    /// <param name="key">The unique identifier for the basket.</param>
    /// <returns>
    ///     The <see cref="CustomerBasket"/> associated with the given key, or null if not found.
    /// </returns>
    public Task<CustomerBasket?> GetBasketAsync(string key);

    /// <summary>
    ///     Deletes a customer's basket from the data store.
    /// </summary>
    /// <param name="id">The unique identifier of the basket to delete.</param>
    public Task<bool> DeleteBasketAsync(string id);
}