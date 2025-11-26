using Shared.DTO.Basket;

namespace serviceAbstraction.Contracts.Payment;

/// <summary>
///     Provides operations for handling payment-related processes,
///     including creating or updating a payment intent for a customer's basket.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    ///     Creates or updates a payment intent associated with the specified basket.
    ///     This ensures the basket has a valid PaymentIntent and ClientSecret for processing payments.
    /// </summary>
    /// <param name="basketId">The unique identifier of the customer's basket.</param>
    /// <returns>
    /// A <see cref="CustomerBasketDto"/> object containing the updated basket
    /// along with payment information such as PaymentIntentId and ClientSecret.
    /// </returns>
    public Task<CustomerBasketDto> CreateOrUpdatePaymentIntentAsync (string basketId);

    /// <summary>
    ///     Handles and processes data received from Stripe webhooks to update the payment status of an order.
    /// </summary>
    /// <param name="request">
    ///     The raw JSON payload sent by Stripe's webhook containing event details.
    /// </param>
    /// <param name="stripeHeader">
    ///     The Stripe signature header used to verify the authenticity of the webhook request.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation of updating the order payment status.
    /// </returns>
    public Task UpdateOrderPaymentStatus (string request, string stripeHeader);

}
