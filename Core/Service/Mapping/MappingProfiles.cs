using AutoMapper;
using DomainLayer.Models.Product;
using Service.Images;
using Shared.DTO.Product;

namespace Service.Mapping;

/// <summary>
///     Mapping Profiles for AutoMapper to map between Domain Models and DTOs
/// </summary>
/// <remarks>
///     AutoMapper maps properties with the same names automatically.
///     For properties with different names, we use ForMember with MapFrom.
///     Here, BrandName and TypeName in ProductDto come from <c>ProductBrand.Name</c> and <c>ProductType.Name</c> in Product.
/// </remarks>
/// <example>
///     TODO: Install the required NuGet packages:
///     <code>
///         AutoMapper
///     </code>
///
///     TODO: After creating the MappingProfiles class, go to Program.cs and add the dependency injection for AutoMapper service:
///     <code>
///         builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
///     </code>
/// </example>
public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        // Map from Product to ProductDto
        // Destination Member: BrandName, TypeName from ProductDto
        // Source Member: ProductBrand.Name, ProductType.Name from Product
        CreateMap<DomainLayer.Models.Product.Product, ProductDto>()
            .ForMember(dist => dist.BrandName, options => options.MapFrom(src => src.ProductBrand.Name))
            .ForMember(dist => dist.TypeName, options => options.MapFrom(src => src.ProductType.Name))
            // Map PictureUrl with source => MapFrom(src => $"https://localhost:7067/{src.PictureUrl}") prefers use static BaseURL go to app
            // options.MapFrom<PictureUrlResolver>()
            .ForMember(dist => dist.PictureUrl, options => options.MapFrom<PictureUrlResolver>());

        // Map from ProductBrand to BrandDto
        CreateMap<ProductBrand, BrandDto>();

        // Map from ProductType to TypeDto
        CreateMap<ProductType, TypeDto>();
    }
}
