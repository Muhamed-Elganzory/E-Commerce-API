using DomainLayer.Contracts.Repository.Basket;
using E_Commerce.Factories;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository.Basket;
using StackExchange.Redis;

namespace E_Commerce.Extensions;

/// <summary>
///     Provides extension methods for configuring presentation-layer services,
///     such as API behavior customization and response formatting.
/// </summary>
public static class PresentationServicesExtensions
{
    /// <summary>
    ///     Adds and configures presentation-related services to the dependency injection container.
    ///     This method customizes how ASP.NET Core handles API model validation errors.
    /// </summary>
    /// <param name="services">The service collection used for dependency injection.</param>
    /// <returns>The updated IServiceCollection instance for fluent chaining.</returns>
    public static IServiceCollection PresentationServices(this IServiceCollection services)
    {
        // Configure options related to API behavior, such as model validation handling
        services.Configure<ApiBehaviorOptions>((options) =>
        {
            // Customize the response returned when model validation fails (HTTP 400)
            // Instead of the default response, use a factory that generates a standardized JSON error structure
            options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationErrorResponse;
        });

        // Return the service collection to allow chaining (fluent API style)
        return services;
    }
}