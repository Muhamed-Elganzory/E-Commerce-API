namespace Shared.DTO.Product;

/// <summary>
///     Data Transfer Object for Product Entity
/// </summary>
public class ProductDto
{
    public int Id {get; set;}

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string PictureUrl { get; set; } = null!;

    public decimal Price { get; set; }

    public string BrandName { get; set; } = null!;

    public string TypeName { get; set; } = null!;
}