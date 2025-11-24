using Service.Auth;
using Service.Basket;
using Service.Images;
using Service.Images.Order;
using Service.Images.Product;
using Service.Mapping;
using Service.Order;
using Service.Payment;
using Service.Product;
using Service.Service;
using ServiceAbstraction.Contracts.Auth;
using serviceAbstraction.Contracts.Basket;
using serviceAbstraction.Contracts.Order;
using serviceAbstraction.Contracts.Payment;
using serviceAbstraction.Contracts.Product;
using serviceAbstraction.Contracts.Service;

namespace E_Commerce.Extensions;

/// <summary>
///     Provides extension methods for registering core application services,
///     such as AutoMapper and the ServiceManagerWithLazyImplementation, into the dependency injection container.
/// </summary>
public static class CoreServicesExtensions
{
    /// <summary>
    ///     Adds and configures core-level services used across the application.
    ///     This includes AutoMapper for object mapping and the ServiceManagerWithLazyImplementation for business logic coordination.
    /// </summary>
    /// <param name="services">The service collection used for dependency injection.</param>
    /// <returns>The updated IServiceCollection instance for fluent chaining.</returns>
    public static IServiceCollection CoreServices(this IServiceCollection services)
    {
        // 🔹 AutoMapper Configuration
        // Registers AutoMapper and adds the defined mapping profiles.
        // This allows automatic mapping between domain models and DTOs.
        services.AddAutoMapper(mapper => mapper.AddProfile(new MappingProfiles()));

        #region Service Manager

        // Service Manager Registration
        // 🔹 Service Manager With Lazy Implementation
        // Registers IServiceManager and its concrete implementation ServiceManagerWithLazyImplementation.
        // The ServiceManagerWithLazyImplementation acts as a central point to access all business services.
        // services.AddScoped<IServiceManager, ServiceManagerWithLazyImplementation>();

        // ============================================================================
        //  SERVICE MANAGER REGISTRATION
        // ============================================================================

        // 🔹 Register the Service Manager that uses delegates (factory functions)
        //     This enables lazy resolution of services and avoids unnecessary allocations.
        services.AddScoped<IServiceManager, ServiceManagerWithDelegate>();

        // ============================================================================
        //  PRODUCT SERVICE REGISTRATION
        // ============================================================================

        // Main service registration
        services.AddScoped<IProductService, ProductService>();

        // Factory delegate (used by ServiceManagerWithDelegate for lazy loading)
        services.AddScoped<Func<IProductService>>(
            provider => provider.GetRequiredService<IProductService>
        );

        // ============================================================================
        //  BASKET SERVICE REGISTRATION
        // ============================================================================

        services.AddScoped<IBasketService, BasketService>();

        // Factory delegate
        services.AddScoped<Func<IBasketService>>(
            provider => provider.GetRequiredService<IBasketService>
        );

        // ============================================================================
        //  ORDER SERVICE REGISTRATION
        // ============================================================================

        services.AddScoped<IOrderService, OrderService>();

        // Factory delegate
        services.AddScoped<Func<IOrderService>>(
            provider => provider.GetRequiredService<IOrderService>
        );

        // ============================================================================
        //  AUTHENTICATION SERVICE REGISTRATION
        // ============================================================================

        services.AddScoped<IAuthenticationService, AuthenticationService>();

        // Factory delegate
        services.AddScoped<Func<IAuthenticationService>>(
            provider => provider.GetRequiredService<IAuthenticationService>
        );

        // ============================================================================
        //  PAYMENT SERVICE REGISTRATION
        // ============================================================================
        services.AddScoped<IPaymentService, PaymentService>();

        // Factory delegate
        services.AddScoped<Func<IPaymentService>>(
            provider => provider.GetRequiredService<IPaymentService>
        );

        #endregion

        #region PictureUrlResolver

        // Register ProductPictureUrlResolver as a Singleton service in the Dependency Injection (DI) container.
        // -----------------------------------------------------------------------------
        // 🧩 What it does:
        // The ProductPictureUrlResolver class is used by AutoMapper to generate full image URLs
        // for ProductDto objects by reading the BaseURL value from appsettings.json (via IConfiguration).
        // It resolves image paths dynamically during object mapping (e.g., mapping Product → ProductDto).
        //
        // 🕓 Why Singleton:
        // - IConfiguration (used inside ProductPictureUrlResolver) is already registered as a Singleton.
        // - ProductPictureUrlResolver does not hold any user- or request-specific data (it's stateless).
        // - Therefore, registering it as a Singleton is efficient — only one instance is created and reused
        //   throughout the application's lifetime, reducing memory allocations and improving performance.
        //
        // ⚖️ Comparison with other lifetimes:
        // - 🔁 AddScoped → Creates one instance per HTTP request (useful for DbContext, but unnecessary here).
        // - ⚡ AddTransient → Creates a new instance every time it’s requested (adds overhead for stateless logic).
        // - ♾️ AddSingleton → Creates one instance for the entire app lifetime ( the best choice here ).

        // Register ProductPictureUrlResolver as a Singleton service in the Dependency Injection (DI) container.
        services.AddSingleton<ProductPictureUrlResolver>();

        //
        services.AddSingleton<OrderItemPictureUrlResolver>();

        #endregion

        // Return the service collection for fluent chaining
        return services;
    }
}
