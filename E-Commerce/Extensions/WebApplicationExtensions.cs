using DomainLayer.Contracts.Seed;
using E_Commerce.Middleware.Exceptions;

namespace E_Commerce.Extensions;

/// <summary>
///     Contains extension methods for configuring middleware and seeding the database
///     when initializing the ASP.NET Core application.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    ///     Seeds the application's database with initial data.
    ///     This method is usually called during application startup.
    /// </summary>
    /// <param name="app">The current WebApplication instance.</param>
    /// <returns>The WebApplication instance after seeding completes.</returns>
    public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
    {
        // Create a new scope to access scoped services
        using (var scope = app.Services.CreateScope())
        {
            // Retrieve the IDataSeeding service instance from the scoped service provider.
            // This service is responsible for populating initial data into the database (e.g., default products, roles, or users).
            var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            // Seed application-specific data (e.g., products, categories, orders, etc.).
            // This ensures the main database has the necessary base data when the application starts.
            await seeder.DataSeedAsync();

            // Seed Identity-related data (e.g., default roles, admin user, permissions, etc.).
            // This populates the Identity database with essential authentication and authorization data.
            await seeder.IdentityDataSeedAsync();

            // Execute the data seeding logic to populate the database
            await seeder.DataSeedAsync();
        }

        // Return the app so the method can be chained fluently
        return app;
    }

    /// <summary>
    ///     Configures and applies custom middleware components to the application's request pipeline.
    /// </summary>
    /// <param name="app">The current WebApplication instance.</param>
    /// <returns>The WebApplication instance after middleware configuration.</returns>
    public static WebApplication CustomMiddleWareExtensions(this WebApplication app)
    {
        // Register the custom global exception handling middleware
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        // Return the app to allow for fluent chaining of other middleware registrations
        return app;
    }
}