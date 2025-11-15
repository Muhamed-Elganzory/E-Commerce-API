using DomainLayer.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DB.Configuration.Order
{
    /// <summary>
    ///     Configures the entity <see cref="OrderItems"/> using Fluent API.
    ///     Defines property mappings and owned types for the OrderItems entity.
    /// </summary>
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItems>
    {
        /// <summary>
        ///     Configures the properties and owned entities of the <see cref="OrderItems"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            // Configure ProductItemOrder as an owned type (value object)
            builder.OwnsOne(p => p.ProductItemOrder);

            // Configure Price property with decimal type and precision
            builder.Property(p => p.Price)
                .HasColumnType("decimal(8,2)");
        }
    }
}