using DomainLayer.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DB.Configuration.Order;

/// <summary>
///     Configures the entity <see cref="DeliveryMethod"/> using the Fluent API.
///     Defines property requirements such as data types, max lengths, and required fields.
/// </summary>
public class DeliveryMethodConfigurations : IEntityTypeConfiguration<DeliveryMethod>
{
    /// <summary>
    ///     Configures the properties of the <see cref="DeliveryMethod"/> entity.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.Property(c => c.Cost)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(n => n.ShortName)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.Property(d => d.DeliveryTime)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(50);

        builder.Property(d => d.Description)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(100);
    }
}
