using DomainLayer.Models.Shared;

namespace DomainLayer.Models.Product;

/// <summary>
///     Product Type Entity
///
///     Relationships: One-to-Many with Product
///         - Product has one ProductType
/// <remarks>
///     TODO No Need For The Collection Navigation Property Here as we don't need to access Products from ProductType
/// </remarks>
/// </summary>
public class ProductType: BaseEntity<int>
{
    public string Name { get; set; } = null!;
}