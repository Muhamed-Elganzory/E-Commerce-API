using E_Commerce.Factories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Extensions;

/// <summary>
///
/// </summary>
public static class PresentationServicesExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection PresentationServices(this IServiceCollection services)
    {
        // Configure how ASP.NET Core should handle model validation errors
        services.Configure<ApiBehaviorOptions>((options) =>
        {
            // Override the default response returned when model validation fails
            options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationErrorResponse;
        });

        return services;
    }
}