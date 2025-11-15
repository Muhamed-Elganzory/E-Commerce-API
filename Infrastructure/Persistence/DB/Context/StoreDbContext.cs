using System.Reflection;
using DomainLayer.Models.Order;
using DomainLayer.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DB.Context;

/// <summary>
///     Represents the main Entity Framework Core database context for the store application.
/// <para>
///     This class is responsible for:
/// </para>
/// <list type="bullet">
///   <item><description>Managing the database connection.</description></item>
///   <item><description>Defining DbSet properties that represent database tables.</description></item>
///   <item><description>Applying entity configurations using the Fluent API.</description></item>
/// </list>
///
/// <remarks>
/// <b>NuGet Packages Required:</b>
/// <code>
///     Microsoft.EntityFrameworkCore.SqlServer   // SQL Server provider
///     Microsoft.EntityFrameworkCore.Tools       // For migrations
/// </code>
///
/// <b>Configuration Steps:</b>
/// <para>1️⃣ Add your connection string in <c>appsettings.json</c>:</para>
/// <code>
///     "ConnectionStrings": {
///         "DefaultConnection": "Server=localhost,1433;Database=E-Commerce;User id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
///     }
/// </code>
///
/// <para>2️⃣ Register the context in <c>Program.cs</c>:</para>
/// <code>
///     builder.Services.AddDbContext StoreDbContext (options =>
///     {
///         options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
///     });
/// </code>
///
/// <para>3️⃣ Any entity configuration class must implement:</para>
/// <code>
/// IEntityTypeConfiguration&lt;TEntity&gt;
/// </code>
/// EF Core will automatically apply these configurations via
/// <c>ApplyConfigurationsFromAssembly</c>.
/// </remarks>
/// </summary>
public class StoreDbContext : DbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="StoreDbContext"/> class.
    /// </summary>
    /// <param name="options">
    ///     The options to be used by this <see cref="DbContext"/>,
    ///     typically passed from dependency injection in <c>Program.cs</c>.
    /// </param>
    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
    {
    }

    /// <summary>
    ///     Configures the model using Fluent API and applies all configurations
    ///     defined in the current assembly.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure entities.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all IEntityTypeConfiguration<TEntity> classes from this assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Apply base EF Core configurations.
        base.OnModelCreating(modelBuilder);
    }

    // -----------------------------
    // DbSet Properties (Tables)
    // -----------------------------

    /// <summary>
    ///     Represents the collection of products in the store.
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    ///     Represents the collection of product types (categories or classifications).
    /// </summary>
    public DbSet<ProductType> ProductTypes { get; set; }

    /// <summary>
    ///     Represents the collection of product brands (e.g., Nike, Apple, etc.).
    /// </summary>
    public DbSet<ProductBrand> ProductBrands { get; set; }

    /// <summary>
    ///     Represents the collection of orders placed by customers.
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    ///     Represents the collection of order items associated with each order.
    /// </summary>
    public DbSet<OrderItems> OrderItems { get; set; }

    /// <summary>
    ///     Represents the collection of available delivery methods.
    /// </summary>
    public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
}