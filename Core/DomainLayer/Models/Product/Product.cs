using DomainLayer.Models.Shared;

namespace DomainLayer.Models.Product;

/// <summary>
///     Product Entity
/// <remarks>
///     Relationships: One-to-Many with ProductBrand and ProductType
///         - Product has one ProductBrand
///         - Product has one ProductType
/// </remarks>
/// </summary>
public class Product: BaseEntity<int>
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string PictureUrl { get; set; } = null!;

    public decimal Price { get; set; }

    // Foreign Keys
    public int ProductBrandId { get; set; }

    // Navigation Properties
    public ProductBrand ProductBrand { get; set; } = null!;

    // Foreign Keys
    public int ProductTypeId { get; set; }

    // Navigation Properties
    public ProductType ProductType { get; set; } = null!;
}