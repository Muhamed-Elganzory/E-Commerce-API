using Service.Spec.Base;
using OrderClass = DomainLayer.Models.Order.Order;

namespace Service.Spec.Order;

/// <summary>
///     Specification used to filter <see cref="OrderClass"/> entities
///     based on a specific Stripe PaymentIntent ID.
///
///     This is typically used when you want to retrieve an order
///     associated with a given payment intent during payment updates,
///     confirmations, or when verifying the payment state.
/// </summary>
/// <param name="paymentIntentId">
///     The PaymentIntent ID returned from Stripe,
///     used to match the corresponding order.
/// </param>
public class OrderPaymentIntentSpecifications(string paymentIntentId) : BaseSpecification<OrderClass, Guid>(p => p.PaymentIntentId == paymentIntentId);
