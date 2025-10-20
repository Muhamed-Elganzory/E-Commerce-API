using DomainLayer.Models.Shared;

namespace DomainLayer.Models.Product;

/// <summary>
///     Product Brand Entity
///     Relationships: One-to-Many with Product
///         - Product has one ProductBrand
/// <remarks>
///     TODO No Need For The Collection Navigation Property Here as we don't need to access Products from ProductBrand
/// </remarks>
/// </summary>
public class ProductBrand: BaseEntity<int>
{
    public string Name { get; set; } = null!;
}