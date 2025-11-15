using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DB.Configuration.Order
{
    /// <summary>
    ///     Configures the entity <see cref="DomainLayer.Models.Order.Order"/> using Fluent API.
    ///     Defines property mappings, relationships, and owned types for the Order entity.
    /// </summary>
    public class OrderConfigurations : IEntityTypeConfiguration<DomainLayer.Models.Order.Order>
    {
        /// <summary>
        ///     Configures the properties and relationships of the <see cref="DomainLayer.Models.Order.Order"/> entity.
        /// </summary>
        /// <param name="builder">The builder to configure the entity.</param>
        public void Configure(EntityTypeBuilder<DomainLayer.Models.Order.Order> builder)
        {
            // Configure SubTotal property as required with decimal(8,2) type
            builder.Property(s => s.SubTotal)
                .IsRequired()
                .HasColumnType("decimal(8,2)");

            // Configure relationship: Order has many OrderItems
            builder.HasMany(o => o.OrderItems)
                .WithOne()  // Assuming OrderItems has navigation property back to Order, can specify here
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationship: Order has one DeliveryMethod
            builder.HasOne(d => d.DeliveryMethod)
                .WithMany() // Assuming DeliveryMethod can be used by many Orders
                .HasForeignKey(o => o.DeliveryMethodId);

            // Configure owned type: ShippingAddress is owned by Order
            builder.OwnsOne(o => o.ShippingAddress);
        }
    }
}