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
    public Order(string userEmail, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItems> orderItems, decimal subTotal)
    {
        UserEmail = userEmail;
        ShippingAddress = shippingAddress;
        DeliveryMethod = deliveryMethod;
        OrderItems = orderItems;
        SubTotal = subTotal;
    }

    /// <summary>
    ///     Gets or sets the email of the user who placed the order.
    ///     <para>Scalar property (simple data type).</para>
    /// </summary>
    public string UserEmail { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the date and time when the order was placed, including offset from UTC.
    ///     <para>Scalar property.</para>
    /// </summary>
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

    /// <summary>
    ///     Gets or sets the current status of the order.
    ///     <para>Scalar property.</para>
    /// </summary>
    public OrderStatus OrderStatus { get; set; }

    /// <summary>
    ///     Gets or sets the shipping address where the order will be delivered.
    ///     <para><b>Navigational Property:</b> links to the ShippingAddress entity.</para>
    /// </summary>
    public ShippingAddress ShippingAddress { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the delivery method chosen for the order.
    ///     <para><b>Navigational Property:</b> links to the DeliveryMethod entity.</para>
    /// </summary>
    public DeliveryMethod DeliveryMethod { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the foreign key for the delivery method.
    ///     <para>Scalar property representing the foreign key.</para>
    /// </summary>
    public int DeliveryMethodId { get; set; }

    /// <summary>
    ///     Gets or sets the collection of order items included in this order.
    ///     <para><b>Navigational Property:</b> one-to-many relationship to OrderItems entities.</para>
    /// </summary>
    public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    /// <summary>
    ///     Gets or sets the subtotal amount for the order before delivery costs.
    ///     <para>Scalar property.</para>
    /// </summary>
    public decimal SubTotal { get; set; }

    /// <summary>
    ///     Calculates the total order amount including the delivery cost.
    /// </summary>
    /// <returns>Total cost = SubTotal + DeliveryMethod.Cost</returns>
    public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
}