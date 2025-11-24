using DomainLayer.Models.Shared;

namespace DomainLayer.Models.Order;

/// <summary>
///     Represents a customer order in the system.
/// </summary>
/// <remarks>
///     This entity contains details about the order itself, including:
///     <list type="bullet">
///         <item><description>The user who placed the order (by email).</description></item>
///         <item><description>The date and time the order was created (with timezone info).</description></item>
///         <item><description>The current status of the order.</description></item>
///         <item><description>Shipping address for delivery (Navigational Property).</description></item>
///         <item><description>Delivery method selected by the customer (Navigational Property).</description></item>
///         <item><description>The collection of order items (one-to-many relationship) (Navigational Property).</description></item>
///         <item><description>Calculations for order totals.</description></item>
///     </list>
/// </remarks>
public class Order : BaseEntity<Guid>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Order"/> class.
    /// </summary>
    public Order() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Order"/> class with specified values.
    /// </summary>
    /// <param name="userEmail">The email address of the user who placed the order.</param>
    /// <param name="shippingAddress">The shipping address associated with the order (Navigational Property).</param>
    /// <param name="deliveryMethod">The delivery method chosen by the customer (Navigational Property).</param>
    /// <param name="orderItems">The collection of items included in the order (Navigational Property).</param>
    /// <param name="subTotal">The subtotal amount for the order before adding delivery cost.</param>
    /// <param name="paymentIntentId"></param>
    public Order(string userEmail, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItems> orderItems, decimal subTotal, string paymentIntentId)
    {
        items = orderItems;
        SubTotal = subTotal;
        BuyerEmail = userEmail;
        DeliveryMethod = deliveryMethod;
        ShipToAddress = shippingAddress;
        PaymentIntentId = paymentIntentId;
        Status = nameof(OrderStatus.Pending);
    }

    /// <summary>
    ///     Email of the customer who placed the order.
    /// </summary>
    public string BuyerEmail { get; set; } = null!; // => UserEmail

    /// <summary>
    ///     Shipping address associated with this order.
    /// </summary>
    public ShippingAddress ShipToAddress { get; set; } = null!; // => ShippingAddress

    /// <summary>
    ///     Collection of items included in the order.
    /// </summary>
    public ICollection<OrderItems> items { get; set; } = new List<OrderItems>(); // => OrderItems

    /// <summary>
    ///     Current status of the order as a string
    ///     (e.g., "Pending", "PaymentReceived").
    /// </summary>
    public string Status { get; set; } = null!; // Enum mapped to string // => OrderStatus

    /// <summary>
    ///     Name of the selected delivery method.
    /// </summary>
    public DeliveryMethod DeliveryMethod { get; set; } = null!;

    /// <summary>
    ///     ID of the delivery method chosen for this order.
    /// </summary>
    public int DeliveryMethodId { get; set; }

    /// <summary>
    ///     Cost of delivery associated with this order.
    /// </summary>
    public decimal DeliveryCost { get; set; }

    /// <summary>
    ///     Order subtotal before applying delivery cost.
    /// </summary>
    public decimal SubTotal { get; set; }

    /// <summary>
    ///     Date and time when the order was placed (with timezone offset).
    /// </summary>
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

    /// <summary>
    ///     Stripe payment intent identifier used to track payment status.
    /// </summary>
    public string PaymentIntentId { get; set; } = null!;

    /// <summary>
    ///     Total cost of the order, including delivery.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    ///     Calculates the total order amount including the delivery cost.
    /// </summary>
    /// <returns>Total cost = SubTotal + DeliveryMethod.Cost</returns>
    public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
}
