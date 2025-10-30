using Shared.DTO.Basket;

namespace serviceAbstraction.Contracts.Basket;

/// <summary>
///     Defines the contract for basket-related operations, including retrieving,
///     creating/updating, and deleting customer baskets.
/// </summary>
public interface IBasketService
{
    /// <summary>
    ///     Retrieves a customer basket based on its unique key (basket ID).
    /// </summary>
    /// <param name="key">
    ///     The unique identifier of the basket (usually a GUID generated on the client side).
    /// </param>
    /// <returns>
    ///     A <see cref="CustomerBasketDto"/> representing the customer's basket data.
    /// </returns>
    public Task<CustomerBasketDto> GetBasketAsync(string key);

    /// <summary>
    ///     Creates a new basket or updates an existing one in the data store.
    /// </summary>
    /// <param name="basketDto">
    ///     The basket data transfer object containing the basket and its items.
    /// </param>
    /// <returns>
    ///     A <see cref="CustomerBasketDto"/> representing the newly created or updated basket.
    /// </returns>
    public Task<CustomerBasketDto> CreateOrUpdateBasketAsync(CustomerBasketDto basketDto);

    /// <summary>
    ///     Deletes a customer basket by its unique key (basket ID).
    /// </summary>
    /// <param name="key">
    ///     The unique identifier of the basket to delete.
    /// </param>
    /// <returns>
    ///     A boolean value indicating whether the basket was successfully deleted.
    /// </returns>
    public Task<bool> DeleteBasketAsync(string key);
}