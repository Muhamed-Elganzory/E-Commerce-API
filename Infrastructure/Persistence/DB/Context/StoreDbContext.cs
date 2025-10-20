using System.Reflection;
using DomainLayer.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DB;

/// <summary>
///     Represents the database context for the store application.
///     This class is responsible for managing the connection to the database
///     - DbContextOptions: Options for configuring the context, such as the database provider and connection string.
/// <remarks>
///     TODO: Install the required NuGet packages:
///     <code>
///         Microsoft.EntityFrameworkCore.SqlServer // For SQL Server database provider
///         TODO Microsoft.EntityFrameworkCore.Tools // For Migrations
///     </code>
///
///     TODO: First, Go To appsettings.json and add a connection string
///     <code>
///         builder.Services.AddDbContext `StoreDbContext (options =>
///         {
///             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
///         });
///     </code>
///
///     TODO: Then, Go To Program.cs and configure the DbContext with the connection string
///     <code>
///         "ConnectionStrings": {
///             "DefaultConnection": "Server= localhost, 1433; Database= E-Commerce; User Id= sa; Password= YourStrong@Passw0rd; TrustServerCertificate= True;"
///         }
///     </code>
/// </remarks>
/// </summary>
/// <param name="options">
///     Options will be passed from Program.cs when configuring the services.
///     This class uses Entity Framework Core to interact with the database.
/// </param>
public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
{
    // Configure the model using Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all entity configurations from the current assembly
        // TODO Eny Entity Configuration Must Implement IEntityTypeConfiguration<TEntity> to be applied.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Apply the base EF Core configurations
        // TODO This is important to ensure that any configurations defined in the base DbContext are also applied.
        base.OnModelCreating(modelBuilder);
    }

    // DbSets to represent tables in the database
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductBrand> ProductBrands { get; set; }
}