using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Shared;
using serviceAbstraction.Contracts.Service;
using Shared.DTO.Basket;

namespace Presentation.Controllers.Payment;

/// <summary>
///     Controller responsible for handling payment-related API requests.
///     Requires user to be authorized (logged in) to access.
/// </summary>
/// <param name="serviceManager">Injected service manager for accessing various services.</param>
[Authorize]
public class PaymentController (IServiceManager serviceManager) : BaseController
{
    /// <summary>
    ///     Holds the injected service manager instance for accessing payment service.
    /// </summary>
    private readonly IServiceManager _serviceManager = serviceManager;

    /// <summary>
    ///     HTTP POST endpoint to create or update a payment intent for the basket.
    ///     This action receives the basket identifier from the route using the parameter {basketId}.
    /// </summary>
    /// <param name="basketId">
    ///     The basket identifier extracted from the route.
    ///     This value is automatically bound because the route template contains {basketId},
    ///     meaning any value sent in the URL (e.g., /api/payment/123) will be mapped here.
    /// </param>
    /// <returns>
    ///     Returns the updated basket DTO containing the payment intent information.
    /// </returns>
    [HttpPost("{basketId}")]
    public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdateBasketAsync(string basketId)
    {
        // Call payment service to create or update payment intent for the given basket
        var basket = await _serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync (basketId);

        // Return HTTP 200 OK with the updated basket information
        return Ok(basket);
    }
}
