using Service.Images;
using Service.Mapping;
using Service.Service;
using serviceAbstraction.Contracts.Service;

namespace E_Commerce.Extensions;

/// <summary>
///     Provides extension methods for registering core application services,
///     such as AutoMapper and the ServiceManager, into the dependency injection container.
/// </summary>
public static class CoreServicesExtensions
{
    /// <summary>
    ///     Adds and configures core-level services used across the application.
    ///     This includes AutoMapper for object mapping and the ServiceManager for business logic coordination.
    /// </summary>
    /// <param name="services">The service collection used for dependency injection.</param>
    /// <returns>The updated IServiceCollection instance for fluent chaining.</returns>
    public static IServiceCollection CoreServices(this IServiceCollection services)
    {
        // 🔹 AutoMapper Configuration
        // Registers AutoMapper and adds the defined mapping profiles.
        // This allows automatic mapping between domain models and DTOs.
        services.AddAutoMapper(mapper => mapper.AddProfile(new MappingProfiles()));

        // 🔹 Service Manager Registration
        // Registers IServiceManager and its concrete implementation ServiceManager.
        // The ServiceManager acts as a central point to access all business services.
        services.AddScoped<IServiceManager, ServiceManager>();

        // Register PictureUrlResolver as a Singleton service in the Dependency Injection (DI) container.
        // -----------------------------------------------------------------------------
        // 🧩 What it does:
        // The PictureUrlResolver class is used by AutoMapper to generate full image URLs
        // for ProductDto objects by reading the BaseURL value from appsettings.json (via IConfiguration).
        // It resolves image paths dynamically during object mapping (e.g., mapping Product → ProductDto).
        //
        // 🕓 Why Singleton:
        // - IConfiguration (used inside PictureUrlResolver) is already registered as a Singleton.
        // - PictureUrlResolver does not hold any user- or request-specific data (it's stateless).
        // - Therefore, registering it as a Singleton is efficient — only one instance is created and reused
        //   throughout the application's lifetime, reducing memory allocations and improving performance.
        //
        // ⚖️ Comparison with other lifetimes:
        // - 🔁 AddScoped → Creates one instance per HTTP request (useful for DbContext, but unnecessary here).
        // - ⚡ AddTransient → Creates a new instance every time it’s requested (adds overhead for stateless logic).
        // - ♾️ AddSingleton → Creates one instance for the entire app lifetime ( the best choice here ).

        // Register PictureUrlResolver as a Singleton service in the Dependency Injection (DI) container.
        services.AddSingleton<PictureUrlResolver>();

        // Return the service collection for fluent chaining
        return services;
    }
}