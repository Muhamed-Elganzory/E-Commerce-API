using DomainLayer.Contracts.Unit;
using Microsoft.EntityFrameworkCore;
using Persistence.DB;
using Persistence.Repository.Unit;

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

        // Register the Unit of Work pattern.
        // The IUnitOfWork interface is linked to its concrete implementation UnitOfWork,
        // allowing dependency injection to manage transactions and repository coordination.
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Return the modified service collection for chaining.
        return services;
    }
}