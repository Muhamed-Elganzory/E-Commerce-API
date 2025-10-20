using AutoMapper;
using Shared.DTO.Product;
using Microsoft.Extensions.Configuration;

namespace Service.Images;

/// <summary>
///     PictureUrlResolver is responsible for handling image URLs and retrieving the base URL from appsettings.json using IConfiguration.
///     It implements IValueResolver to resolve the full image URL during the mapping process.
///
///     TODO:
///      1⃣  Install the required NuGet package:
///         - Microsoft.Extensions.Configuration (to work with appsettings.json and IConfiguration)
///
///      2⃣  In Program.cs, register the necessary DI service and enable static file serving:
///             <code>
///                 // Enables serving static files (e.g., images, CSS, JS) from the wwwroot folder.
///                 app.UseStaticFiles();
///             </code>
///
///      3⃣  In Program.cs, register the DI service PictureUrlResolver:
///             🧩 What it does:
///                 The PictureUrlResolver class is used by AutoMapper to generate full image URLs
///                 for ProductDto objects by reading the BaseURL value from appsettings.json (via IConfiguration).
///                 It resolves image paths dynamically during object mapping (e.g., mapping Product → ProductDto).
///
///             🕓 Why Singleton:
///                 - IConfiguration (used inside PictureUrlResolver) is already registered as a Singleton.
///                 - PictureUrlResolver does not hold any user- or request-specific data (it's stateless).
///                 - Therefore, registering it as a Singleton is efficient — only one instance is created and reused
///                 throughout the application's lifetime, reducing memory allocations and improving performance.
///
///             ⚖️ Comparison with other lifetimes:
///                 - 🔁 AddScoped → Creates one instance per HTTP request (useful for DbContext, but unnecessary here).
///                 - ⚡ AddTransient → Creates a new instance every time it’s requested (adds overhead for stateless logic).
///                 - ♾️ AddSingleton → Creates one instance for the entire app lifetime ( the best choice here ).
///         <code>
///             builder.Services.AddSingleton 'PictureUrlResolver' ();
///         </code>
/// </summary>
/// <remarks>
///     IValueResolver takes 3 type parameters:
///     <code>
///         TSource      :- Product
///         TDestination :- ProductDto
///         TDestMember  :- string
///     </code>
/// </remarks>
public class PictureUrlResolver(IConfiguration configuration) : IValueResolver<DomainLayer.Models.Product.Product, ProductDto, string>
{
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    ///     Resolve function to handle how the PictureUrl in ProductDto is generated.
    ///     It reads the BaseUrl from the appsettings.json file and concatenates it with the PictureUrl from the Product entity.
    ///
    ///     Params:
    ///         source       :- The source Product object.
    ///         destination  :- The target ProductDto object.
    ///         destMember   :- The destination member being resolved (PictureUrl).
    ///         context      :- The AutoMapper resolution context.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="destMember"></param>
    /// <param name="context"></param>
    /// <returns>The full image URL (base URL + picture path).</returns>
    public string Resolve(DomainLayer.Models.Product.Product source, ProductDto destination, string destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.PictureUrl))
        {
            return string.Empty;
        }

        // GetSection is used to read values from appsettings.json
        // Example in appsettings.json:
        // "URLS": {
        //     "BaseURL": "https://localhost:7067/"
        // }
        // Expected final result example:
        // https://localhost:7067/Images/Products/PepperPasta.png
        var url = $"{_configuration.GetSection("URLS")["BaseUrl"]}{source.PictureUrl}";

        // Return the full image path
        return url;
    }
}
