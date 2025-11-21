using DomainLayer.Contracts.Repository.Basket;
using DomainLayer.Contracts.Repository.Redis;
using DomainLayer.Contracts.Seed;
using DomainLayer.Contracts.Unit;
using DomainLayer.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Auth.Context;
using Persistence.DB.Context;
using Persistence.DB.Seed;
using Persistence.Repository.Basket;
using Persistence.Repository.Redis;
using Persistence.Repository.Unit;
using Service.Redis;
using serviceAbstraction.Contracts.Redis;
using StackExchange.Redis;

namespace E_Commerce.Extensions;

/// <summary>
///     Provides extension methods for configuring infrastructure-related services,
///     such as the database context and the Unit of Work pattern.
/// </summary>
public static class InfraStructureServicesExtensions
{
    /// <summary>
    ///     Registers and configures the infrastructure layer services for the application.
    ///     This includes setting up the database context and dependency injection
    ///     for the Unit of Work pattern.
    /// </summary>
    /// <param name="services">
    ///     The service collection used for dependency injection in the application.
    /// </param>
    /// <param name="configuration">
    ///     The configuration object used to access application settings such as connection strings.
    /// </param>
    /// <returns>
    ///     The updated <see cref="IServiceCollection"/> instance containing the registered services.
    /// </returns>
    public static IServiceCollection InfraStructureService(this IServiceCollection services, IConfiguration configuration)
    {
        // Register the StoreDbContext and configure it to use SQL Server.
        // The connection string is retrieved from appsettings.json ("DefaultConnection").
        // This enables Entity Framework Core to interact with the SQL Server database.
        services.AddDbContext<StoreDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        // Register the Identity database context (for authentication & authorization)
        // The connection string is retrieved from appsettings.json ("IdentityConnection").
        // This enables Entity Framework Core to interact with the SQL Server database.
        services.AddDbContext<StoreIdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
        });

        // Register the Unit of Work pattern.
        // The IUnitOfWork interface is linked to its concrete implementation UnitOfWork,
        // allowing dependency injection to manage transactions and repository coordination.
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Data Seeding Service
        // Register the IDataSeeding interface with its implementation DataSeeding in the DI container.
        // This allows the application to use the DataSeeding service for seeding initial data into
        // the database when needed.
        services.AddScoped<IDataSeeding, DataSeeding>();

        // Register Redis connection multiplexer as a singleton (shared connection)
        // It manages the connection to Redis efficiently across all requests
        services.AddSingleton<IConnectionMultiplexer>((_) =>
        {
            return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
        });

        // Basket Repository (Register Redis-based Basket Repository)
        services.AddScoped<IBasketRepository, BasketRepository>();

        // ============================================================================
        //  REDIS CACHING LAYER REGISTRATION
        // ============================================================================

        // 🔹 ICashService (Application-Level Caching Logic)
        // Registers the high-level caching service responsible for:
        //   - Serializing objects before caching (JSON serialization)
        //   - Coordinating with the repository to store/retrieve data
        //   - Ensuring the application deals with typed objects instead of raw Redis data
        // Scoped lifetime is used because:
        //   - Caching operations may depend on request-specific context
        //   - Scoped ensures each HTTP request uses its own service instance
        services.AddScoped<ICashService, CashService>();

        // 🔹 ICashRepository (Low-Level Redis Operations)
        // Registers the repository that communicates directly with Redis using StackExchange.Redis.
        // Responsible for:
        //   - Executing StringGet / StringSet
        //   - Managing TTL
        //   - Handling raw Redis data operations
        // Also registered as Scoped because:
        //   - It uses IConnectionMultiplexer.GetDatabase(), which returns a logical DB
        //   - Scoped is the recommended lifetime for repository patterns
        //   - Keeps lifetime alignment with services that depend on it
        services.AddScoped<ICashRepository, CashRepository>();

        // ------------------------------------------------------------
        // Register ASP.NET Core Identity services in the dependency injection container.
        //
        // 1️⃣ AddIdentityCore<ApplicationUser>()
        //     - Registers the core Identity system using the custom ApplicationUser entity.
        //     - Provides services for managing users (UserManager, password hashing, validation, etc.).
        //
        // 2️⃣ AddRoles<IdentityRole>()
        //     - Adds support for Role-based authorization.
        //     - Enables RoleManager and allows assigning users to specific roles (e.g., Admin, User, etc.).
        //
        // 3️⃣ AddEntityFrameworkStores<StoreIdentityDbContext>()
        //     - Configures Identity to use Entity Framework Core for persistence.
        //     - Stores all Identity-related data (Users, Roles, Claims, Tokens, etc.)
        //       in the specified StoreIdentityDbContext connected to the Identity database.
        // ------------------------------------------------------------
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<StoreIdentityDbContext>();

        // Return the modified service collection for chaining.
        return services;
    }
}