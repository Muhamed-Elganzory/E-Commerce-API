namespace E_Commerce.Extensions
{
    /// <summary>
    ///     Provides extension methods for configuring Cross-Origin Resource Sharing (CORS) policies.
    /// </summary>
    public static class Cors
    {
        /// <summary>
        ///     Registers CORS services with a policy allowing requests from Angular running on localhost.
        /// </summary>
        /// <param name="services">The IServiceCollection to add CORS services to.</param>
        /// <returns>The updated IServiceCollection with CORS services added.</returns>
        public static IServiceCollection CorsServices(this IServiceCollection services)
        {
            // Add CORS policies to the service collection
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularLocalhost",
                    builder =>
                    {
                        builder
                            // Allow requests only from this origin (Angular dev server)
                            .WithOrigins("http://localhost:4200")
                            // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
                            .AllowAnyMethod()
                            // Allow any HTTP headers
                            .AllowAnyHeader()
                            // Allow credentials such as cookies or authentication headers
                            .AllowCredentials();
                    });
            });

            return services;
        }
    }
}
