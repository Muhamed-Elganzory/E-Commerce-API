using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DB.Configuration.Product;

/// <summary>
///     Configuration for the Product entity
/// </summary>
public class ProductConfiguration: IEntityTypeConfiguration <DomainLayer.Models.Product.Product>
{
    // Configure the Product entity
    public void Configure(EntityTypeBuilder<DomainLayer.Models.Product.Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.PictureUrl).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(500);

        // Setting precision for Price to handle currency values accurately in the database
        builder.Property(p => p.Price).IsRequired().HasPrecision(18, 2);

        // Relationships
        builder.HasOne(p => p.ProductBrand)
            .WithMany()
            .HasForeignKey(p => p.ProductBrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.ProductType)
            .WithMany()
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}