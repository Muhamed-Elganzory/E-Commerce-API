using AutoMapper;
using Microsoft.Extensions.Configuration;
using Shared.DTO.Product;

namespace Service.Images.Product;

/// <summary>
///     Resolves the full image URL for product images during the mapping process from <see cref="DomainLayer.Models.Product.Product"/>
///     to <see cref="ProductDto"/>.
///     <para>
///         It reads the base URL from <c>appsettings.json</c> via <see cref="IConfiguration"/> and concatenates it with the relative image path.
///     </para>
/// </summary>
/// <remarks>
///     Implements <see cref="IValueResolver{TSource, TDestination, TDestMember}"/> for:
///     <list type="bullet">
///         <item><description><b>TSource</b> → Product</description></item>
///         <item><description><b>TDestination</b> → ProductDto</description></item>
///         <item><description><b>TDestMember</b> → string (PictureUrl)</description></item>
///     </list>
///
///     ⚙️ Example usage in <c>MappingProfiles.cs</c>:
///     <code>
///     CreateMap&lt;Product, ProductDto&gt;()
///         .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom&lt;ProductPictureUrlResolver&gt;());
///     </code>
/// </remarks>
/// <example>
///     Example <c>appsettings.json</c>:
///     <code>
///     {
///         "URLS": {
///             "BaseUrl": "https://localhost:7067/"
///         }
///     }
///     </code>
/// </example>
/// <example>
///     Example final output:
///     <code>
///     Input:  PictureUrl = "Images/Products/PepperPasta.png"
///     Result: "https://localhost:7067/Images/Products/PepperPasta.png"
///     </code>
/// </example>
/// <dependency>
///     Register the resolver as a singleton service in <c>Program.cs</c>:
///     <code>
///     builder.Services.AddSingleton&lt;ProductPictureUrlResolver&gt;();
///     </code>
///     <para>
///         ✅ <b>Why Singleton:</b>
///         <list type="bullet">
///             <item><description><see cref="IConfiguration"/> is already registered as a singleton.</description></item>
///             <item><description>The resolver is stateless (no per-user or per-request data).</description></item>
///             <item><description>Efficient reuse — reduces memory allocations and improves performance.</description></item>
///         </list>
///     </para>
/// </dependency>
public class ProductPictureUrlResolver(IConfiguration configuration) : IValueResolver<DomainLayer.Models.Product.Product, ProductDto, string>
{
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    ///     Resolves the full product image URL by combining the base URL from configuration
    ///     with the product’s relative <see cref="DomainLayer.Models.Product.Product.PictureUrl"/>.
    /// </summary>
    /// <param name="source">The source <see cref="DomainLayer.Models.Product.Product"/> entity.</param>
    /// <param name="destination">The destination <see cref="ProductDto"/> object.</param>
    /// <param name="destMember">The destination member being mapped (PictureUrl).</param>
    /// <param name="context">The AutoMapper resolution context.</param>
    /// <returns>
    ///     The full image URL, combining the base URL and relative path.
    ///     Returns an empty string if <paramref name="source.PictureUrl"/> is null or empty.
    /// </returns>
    public string Resolve(DomainLayer.Models.Product.Product source, ProductDto destination, string destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.PictureUrl))
            return string.Empty;

        // GetSection is used to read values from appsettings.json
        // Example in appsettings.json: "URLS":
        // {
        //      "BaseURL": "https://localhost:7067/"
        // }
        // Expected final result example: // https://localhost:7067/Images/Products/PepperPasta.png
        var url = $"{_configuration.GetSection("URLS")["BaseUrl"]}{source.PictureUrl}";

        // Return the full image path
        return url;
    }
}