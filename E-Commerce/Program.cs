using System.Text.Json;
using E_Commerce.Extensions;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace E_Commerce
{
    public abstract class Program
    {
        public static async Task Main(string[] args)
        {
            #region Builder Configuration

            // ✅ Create and configure the web application builder
            var builder = WebApplication.CreateBuilder(args);

            // ✅ Add controllers to handle incoming HTTP requests
            builder.Services.AddControllers();

            // ✅ Register swagger services (AddEndpointsApiExplorer, AddSwaggerGen)
            builder.Services.AddSwaggerServices();

            #endregion

            #region Dependency Injection Services

            // In ConfigureServices or during service registration
            // ✅ Register the CORS service with the "AllowAngularLocalhost" policy
            // This adds the CORS configuration to the DI container so it can be applied later in the pipeline
            builder.Services.CorsServices();

            // ✅ Register Infrastructure services (e.g., repositories, DbContext)
            builder.Services.InfraStructureService(builder.Configuration);

            // ✅ Register Core (Domain/Business) services
            builder.Services.CoreServices();

            // ✅ Register Presentation layer services (Controllers, Filters, etc.)
            builder.Services.PresentationServices();

            // ✅ Add JWT Authentication configuration
            builder.Services.AddJwtService(builder.Configuration);

            #endregion

            #region Build App

            // ✅ Build the web application
            var app = builder.Build();
            app.UseDeveloperExceptionPage();

            #endregion

            #region Custom Middleware

            // In Program.cs or Startup.cs within the Middleware Pipeline
            // ✅ Enable CORS using the policy named "AllowAngularLocalhost"
            // This allows requests from http://localhost:4200 (Angular dev server) to access the API
            app.UseCors("AllowAngularLocalhost");

            // ✅ Register custom middleware (Exception handling, Logging, etc.)
            app.CustomMiddleWareExtensions();

            #endregion

            #region Data Seeding

            // ✅ Run database seeding (populate initial data)
            await app.SeedDbAsync();

            #endregion

            #region Middleware Pipeline

            // ✅ Enable Swagger only in development environment
            // Swagger provides interactive API documentation, so it is enabled only during development
            // to avoid exposing unnecessary details in production.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger ();

                app.UseSwaggerUI (options =>
                {
                    // Display execution duration for each API request
                    options.ConfigObject = new ConfigObject()
                    {
                        DisplayRequestDuration = true
                    };

                    // Title shown in the Swagger UI browser tab
                    options.DocumentTitle = "E-Commerce API";

                    // The endpoint where Swagger retrieves the OpenAPI specification (v1)
                    options.SwaggerEndpoint ("/swagger/v1/swagger.json", "E-Commerce API");

                    // Configure JSON serialization for displayed schemas (camelCase formatting)
                    options.JsonSerializerOptions = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    // Collapse all groups/endpoints by default for a cleaner UI
                    options.DocExpansion (DocExpansion.None);

                    // Enable search/filter inside Swagger UI
                    options.EnableFilter ();

                    // Validate request/response schema inside Swagger (OpenAPI validator)
                    options.EnableValidator ();

                    // Keep JWT Authorize token saved across UI refresh
                    options.EnablePersistAuthorization ();
                });
            }

            // ✅ Redirect all HTTP traffic to HTTPS
            app.UseHttpsRedirection();

            #region Static Files

            // ✅ Serve static files (wwwroot folder)
            app.UseStaticFiles();

            #endregion

            // ✅ Enable routing
            app.UseRouting();

            // ⚠️ Must come **before Authorization**
            // ✅ Enable JWT Authentication middleware to validate tokens in requests
            app.UseAuthentication();

            // ✅ Enable Authorization (after successful Authentication)
            app.UseAuthorization();

            // ✅ Map controller routes to endpoints
            app.MapControllers();

            #endregion

            // ✅ Run the application
            await app.RunAsync();
        }
    }
}
