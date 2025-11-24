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
}
