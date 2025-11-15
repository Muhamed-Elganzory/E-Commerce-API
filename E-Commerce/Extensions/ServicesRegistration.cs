using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace E_Commerce.Extensions
{
    /// <summary>
    ///     Provides extension methods for configuring JWT authentication in the application's service container.
    /// </summary>
    public static class ServicesRegistration
    {
        /// <summary>
        ///     Registers and configures JWT authentication using the options defined in appsettings.json.
        /// </summary>
        /// <param name="services">The application's service collection.</param>
        /// <param name="configuration">The application configuration (used to read JWT settings).</param>
        /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
        {
            // 🔹 Configure the authentication scheme to use JWT Bearer by default
            services.AddAuthentication(config =>
            {
                // Used when verifying authentication tokens
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // Used when the app needs to challenge (ask for authentication)
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // 🔹 Add and configure the JWT Bearer options
            .AddJwtBearer(options =>
            {
                // Configure how incoming JWT tokens will be validated
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // ✅ Ensure the token was issued by a trusted server (Issuer)
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtOptions:Issuer"],

                    // ✅ Ensure the token is meant for this audience (client/app)
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtOptions:Audience"],

                    // ✅ Ensure the token has not expired
                    ValidateLifetime = true,

                    // ✅ Ensure the token is properly signed with the correct key
                    ValidateIssuerSigningKey = true,

                    // 🔹 Specify the secret key used to sign and verify JWT tokens
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"]!)
                    ),

                    // Optional (recommended): Small clock skew to allow slight time differences between servers
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Return the service collection so you can chain more registrations
            return services;
        }

        /// <summary>
        ///     Registers and configures Swagger/OpenAPI services.
        ///     This includes enabling Swagger UI and adding JWT Bearer authentication
        ///     support to allow testing secured endpoints from within Swagger.
        /// </summary>
        /// <param name="services">The application's <see cref="IServiceCollection"/>.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // ✅ Enable Swagger services
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                // ✅ Add JWT Bearer security definition
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Description = "Enter JWT token with **Bearer** prefix. Example: `Bearer your_token_here`"
                });

                // ✅ Apply security requirement to all endpoints
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}
