using AutoMapper;
using Shared.DTO.Basket;
using DomainLayer.Models.Basket;
using DomainLayer.Exceptions.Basket;
using serviceAbstraction.Contracts.Basket;
using DomainLayer.Contracts.Repository.Basket;

namespace Service.Basket;

/// <summary>
///     Provides the business logic for managing customer baskets,
///     bridging between the repository (data layer) and DTOs (presentation layer).
///     This service handles retrieving, creating/updating, and deleting baskets.
/// </summary>
public class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
{
    /// <summary>
    ///     The repository responsible for interacting with the basket data in Redis.
    /// </summary>
    private readonly IBasketRepository _basketRepository = basketRepository;

    /// <summary>
    ///     AutoMapper instance used for converting between domain models and DTOs.
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    ///     Retrieves a customer's basket from the repository using the given key (basket ID).
    /// </summary>
    /// <param name="key">The unique basket identifier (usually a GUID generated on the frontend).</param>
    /// <returns>
    ///     A <see cref="CustomerBasketDto"/> representing the customer’s basket if found.
    /// </returns>
    /// <exception cref="BasketNotFoundException">
    ///     Thrown when the basket does not exist in Redis for the given key.
    /// </exception>
    public async Task<CustomerBasketDto> GetBasketAsync(string key)
    {
        // Fetch the basket data from Redis using the repository.
        var basket = await _basketRepository.GetBasketAsync(key);

        // If no basket is found, throw a domain-specific exception.
        if (basket is null)
            throw new BasketNotFoundException(key);

        // Map the domain model to a DTO and return it to the caller.
        return _mapper.Map<CustomerBasket, CustomerBasketDto>(basket);
    }

    /// <summary>
    ///     Creates a new basket or updates an existing one in Redis.
    /// </summary>
    /// <param name="basketDto">The basket data transfer object received from the client.</param>
    /// <returns>
    ///     A <see cref="CustomerBasketDto"/> representing the newly created or updated basket.
    /// </returns>
    /// <exception cref="Exception">
    ///     Thrown when the operation fails due to connectivity or repository errors.
    /// </exception>
    public async Task<CustomerBasketDto> CreateOrUpdateBasketAsync(CustomerBasketDto basketDto)
    {
        // Map the incoming DTO (from the API layer) to the domain model.
        var customerBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basketDto);

        // Create or update the basket in Redis via the repository.
        var createdOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);

        // If the repository failed to store the basket, throw an exception.
        if (createdOrUpdatedBasket is null)
            throw new Exception("Cannot create or update basket now, please try again later");

        // Map the updated domain model back to a DTO and return it.
        return _mapper.Map<CustomerBasket, CustomerBasketDto>(createdOrUpdatedBasket);
    }

    /// <summary>
    ///     Deletes a basket from Redis using its unique key.
    /// </summary>
    /// <param name="key">The unique basket identifier (usually provided by the client).</param>
    /// <returns>
    ///     <c>true</c> if the basket was successfully deleted; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> DeleteBasketAsync(string key)
    {
        // Delete the basket entry from Redis and return the operation result.
        return await _basketRepository.DeleteBasketAsync(key);
    }
}
