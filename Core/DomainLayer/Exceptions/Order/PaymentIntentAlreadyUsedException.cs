namespace DomainLayer.Exceptions.Order;

/// <summary>
///     Exception thrown when an attempt is made to reuse a Stripe PaymentIntentId
///     that has already been associated with an existing order.
/// </summary>
/// <param name="paymentIntentId">
///     The PaymentIntentId that was already used to create another order.
/// </param>
public class PaymentIntentAlreadyUsedException(string paymentIntentId)
    : Exception($"PaymentIntentId '{paymentIntentId}' has already been used for another order.");
