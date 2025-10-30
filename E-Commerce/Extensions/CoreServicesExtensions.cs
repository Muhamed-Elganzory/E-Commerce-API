using Service.Mapping;
using Service.Service;
using serviceAbstraction.Contracts.Service;

namespace E_Commerce.Extensions;

/// <summary>
///
/// </summary>
public static class CoreServicesExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection CoreServices(this IServiceCollection services)
    {
        // Auto Mapper Service
        // Register AutoMapper with the DI container and add the MappingProfiles configuration.
        // This enables object-to-object mapping between domain models and DTOs using the defined profiles.
        services.AddAutoMapper(mapper => mapper.AddProfile(new MappingProfiles()));

        // Service Manager
        // To centralized manager for different services in the application.
        services.AddScoped<IServiceManager, ServiceManager>();

        return services;
    }
}