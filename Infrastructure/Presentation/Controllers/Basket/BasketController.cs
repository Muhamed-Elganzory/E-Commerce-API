using Shared.DTO.Basket;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Shared;
using serviceAbstraction.Contracts.Service;

namespace Presentation.Controllers.Basket;

/// <summary>
///     Controller responsible for managing customer baskets in the system.
///     Provides endpoints to get, create/update, and delete baskets.
/// </summary>
public class BasketController(IServiceManager serviceManager) : BaseController
{
    /// <summary>
    ///     Provides access to the application services (including basket service).
    /// </summary>
    private readonly IServiceManager _serviceManager = serviceManager;

    /// <summary>
    ///     Retrieves a basket by its unique key (ID).
    /// </summary>
    /// <param name="key">The basket ID provided by the client (GUID).</param>
    /// <returns>Returns the basket data if found, otherwise a 404 error.</returns>
    [HttpGet("{key}")] // ✅ specify route parameter explicitly
    public async Task<ActionResult<CustomerBasketDto>> GetBasketAsync(string key)
    {
        // Get the basket from the service layer using the provided key
        var basket = await _serviceManager.BasketService.GetBasketAsync(key);

        // Return 200 OK with the basket data
        return Ok(basket);
    }

    /// <summary>
    ///     Creates a new basket or updates an existing one.
    /// </summary>
    /// <param name="basketDto">The basket data sent from the client (contains ID and items).</param>
    /// <returns>Returns the created or updated basket.</returns>
    [HttpPost]
    public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdateBasketAsync([FromBody] CustomerBasketDto basketDto)
    {
        var basket = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basketDto);

        // Return 200 OK with the created or updated basket
        return Ok(basket);
    }

    /// <summary>
    ///     Deletes a basket by its unique key (ID).
    /// </summary>
    /// <param name="key">The basket ID to delete.</param>
    /// <returns>Returns 204 No Content if successfully deleted.</returns>
    [HttpDelete("{key}")]
    public async Task<IActionResult> DeleteBasketAsync(string key)
    {
        var result = await _serviceManager.BasketService.DeleteBasketAsync(key);

        if (!result)
            return NotFound($"Basket with key '{key}' was not found.");

        // ✅ Standard REST response: NoContent instead of returning a bool
        return NoContent();
    }
}