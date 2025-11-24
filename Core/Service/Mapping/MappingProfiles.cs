using AutoMapper;
using DomainLayer.Models.Auth;
using DomainLayer.Models.Basket;
using DomainLayer.Models.Order;
using DomainLayer.Models.Product;
using Service.Images;
using Service.Images.Order;
using Service.Images.Product;
using Shared.DTO.Auth;
using Shared.DTO.Basket;
using Shared.DTO.Order;
using Shared.DTO.Product;

namespace Service.Mapping;

/// <summary>
///     Defines mapping configurations between domain models and Data Transfer Objects (DTOs)
///     using AutoMapper. This profile handles all mappings for Product, Basket, Auth, and Order entities.
/// </summary>
/// <remarks>
///     AutoMapper automatically maps properties with identical names.
///     For properties with different names, <c>ForMember</c> and <c>MapFrom</c> are used.
/// </remarks>
/// <example>
///     Install the required NuGet packages:
///     To enable AutoMapper in your application: AutoMapper
///     After creating the MappingProfiles class, go to Program.cs and add the dependency injection for AutoMapper service:
///     <code>
///         builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
///     </code>
/// </example>
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        #region Product

        // Maps from Product entity to ProductDto.
        // Also maps nested navigation properties for ProductBrand and ProductType.
        CreateMap<DomainLayer.Models.Product.Product, ProductDto>()
            .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
            // Maps PictureUrl using a custom value resolver.
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());

        // Maps between ProductBrand entity and BrandDto.
        CreateMap<ProductBrand, BrandDto>();

        // Maps between ProductType entity and TypeDto.
        CreateMap<ProductType, TypeDto>();

        #endregion

        #region Basket

        // Maps between the domain model (used in backend logic)
        // and the DTO (used for API requests/responses).
        CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();

        // Maps between the basket item model and its DTO.
        CreateMap<BasketItem, BasketItemDto>().ReverseMap();

        #endregion

        #region Auth - Identity

        // Configures two-way mapping between UserAddress (domain model)
        // and AddressDto (data transfer object).
        CreateMap<UserAddress, AddressDto>().ReverseMap();

        #endregion

        #region Order

        // Maps data from ShippingAddressDto (DTO used in API layer)
        // to ShippingAddress (value object used in domain layer) and vice versa.
        // This mapping enables converting API request/response models
        // to domain models for business logic and persistence.
        CreateMap<ShippingAddressDto, ShippingAddress>().ReverseMap();

        // Maps from domain Order entity to OrderToReturnDto used in API responses.
        // Maps the DeliveryMethod property to the DeliveryMethod's ShortName string,
        // so that API consumers get a simple delivery method name instead of the full object.
        CreateMap<DomainLayer.Models.Order.Order, OrderToReturnDto>()
            .ForMember(dist => dist.DeliveryMethod,
                opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
            .ForMember(dist => dist.Total,// SubTotal + src.DeliveryCost
                opt => opt.MapFrom(src => src.GetTotal()));

        // Maps from domain OrderItems entity to OrderItemsDto used in API responses.
        // Maps ProductName from the nested ProductItemOrder entity.
        // Uses OrderItemPictureUrlResolver to build full URL for the product image.
        CreateMap<OrderItems, OrderItemsDto>()
            .ForMember(dist => dist.ProductId,
                opt => opt.MapFrom(src => src.ProductItemOrder.ProductId))
            .ForMember(dist => dist.ProductName,
                opt => opt.MapFrom(src => src.ProductItemOrder.ProductName))
            .ForMember(dist => dist.PictureUrl, opt =>
                opt.MapFrom<OrderItemPictureUrlResolver>())
            .ForMember(dist => dist.Price,
                opt => opt.MapFrom(src => src.Price))
            .ForMember(dist => dist.Quantity,
                opt => opt.MapFrom(src => src.Quantity));

        #endregion

        #region Delivery Method

        // Map DeliveryMethod entity to DeliveryMethodDto for data transfer
        CreateMap<DeliveryMethod, DeliveryMethodDto>();

        #endregion
    }
}
