using System.Text.Json;
using DomainLayer.Contracts.Seed;
using DomainLayer.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DB.Seed.Product;

/// <summary>
///     Data seeding class to seed initial data into the database
/// <remarks>
///     dbContext is injected via constructor injection to access the database
///     Then go to Program.cs and register the service in the DI container
/// </remarks>
/// <remarks>
///     Async streams are used to avoid blocking the main thread, This is important for scalability
///     and responsiveness of the application, Async streams are used with I/O-bound operations
///     like file reading and database access, This ensures that the application can handle more
///     concurrent requests efficiently, without being blocked by long-running operations
/// </remarks>
/// </summary>
/// <param name="dbContext"></param>
public class DataSeeding(StoreDbContext dbContext): IDataSeeding
{
    // Property injection of DbContext
    private readonly StoreDbContext _dbContext = dbContext;

    // Data seeding method to seed initial data into the database
    public async Task DataSeedAsync()
    {
        // Check for any pending migrations
        // GetPendingMigrations it used with sync streams to block the main thread
        // GetPendingMigrationsAsync it used with async streams to avoid blocking the main thread
        var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

        // Apply any pending migrations before seeding data
        if (pendingMigrations.Any())
        {
            // Migrate it used in updating the database schema to the latest version
            _dbContext.Database.Migrate();
        }

        // Seed Product Brands, if not already [seeded || data] add your seeding logic here
        if (!_dbContext.ProductBrands.Any())
        {
            // Read the JSON file
            // ReadAllText it used with sync streams to block the main thread
            // OpenRead it used with async streams to avoid blocking the main thread
            var brandsData = File.OpenRead("../Infrastructure/Persistence/DB/Seed/Product/JSON/brands.json");

            {
                // Deserialize the JSON data to a list of ProductBrand objects, to add it to the database
                // Deserialize it used with sync streams to block the main thread
                // DeserializeAsync it used with async streams to avoid blocking the main thread
                var brandsSerialized = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(brandsData);

                // Check if deserialization was successful and the list is not empty
                if (brandsSerialized is not null && brandsSerialized.Any())
                {
                    // Add the deserialized data to the DbContext
                    // AddRange it used with sync streams to block the main thread
                    // AddRangeAsync it used with async streams to avoid blocking the main thread
                    await _dbContext.ProductBrands.AddRangeAsync(brandsSerialized);
                }
                else
                {
                    throw new Exception("No Product Brands Data Found");
                }
            }

            // Save changes to the database
            try
            {
                // SaveChanges it used with sync streams to block the main thread
                // SaveChangesAsync it used with async streams to avoid blocking the main thread
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error while saving seeded data: {ex.Message}");
                throw;
            }
        }

        // Seed Product Types, if not already [seeded || data] add your seeding logic here
        if (!_dbContext.ProductTypes.Any())
        {
            // Read the JSON file
            // ReadAllText it used with sync streams to block the main thread
            // OpenRead it used with async streams to avoid blocking the main thread
            var typesData = File.OpenRead("../Infrastructure/Persistence/DB/Seed/Product/JSON/types.json");

            {
                // Deserialize the JSON data to a list of ProductType objects, to add it to the database
                // Deserialize it used with sync streams to block the main thread
                // DeserializeAsync it used with async streams to avoid blocking the main thread
                var typesSerialized = await JsonSerializer.DeserializeAsync<List<ProductType>>(typesData);

                // Check if deserialization was successful and the list is not empty
                if (typesSerialized is not null && typesSerialized.Any())
                {
                    // Add the deserialized data to the DbContext
                    // AddRange it used with sync streams to block the main thread
                    // AddRangeAsync it used with async streams to avoid blocking the main thread
                    await _dbContext.ProductTypes.AddRangeAsync(typesSerialized);
                }
                else
                {
                    throw new Exception("No Product Types Data Found");
                }
            }

            // Save changes to the database
            try
            {
                // SaveChanges it used with sync streams to block the main thread
                // SaveChangesAsync it used with async streams to avoid blocking the main thread
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error while saving seeded data: {ex.Message}");
                throw;
            }
        }

        // Seed Products, if not already [seeded || data] add your seeding logic here
        if (!_dbContext.Products.Any())
        {
            // Read the JSON file
            // ReadAllText it used with sync streams to block the main thread
            // OpenRead it used with async streams to avoid blocking the main thread
            var productsData = File.OpenRead("../Infrastructure/Persistence/DB/Seed/Product/JSON/products.json");

            {
                // Deserialize the JSON data to a list of Product objects, to add it to the database
                // Deserialize it used with sync streams to block the main thread
                // DeserializeAsync it used with async streams to avoid blocking the main thread
                var productsSerialized = await JsonSerializer.DeserializeAsync<List<DomainLayer.Models.Product.Product>>(productsData);

                // Check if deserialization was successful and the list is not empty
                if (productsSerialized is not null && productsSerialized.Any())
                {
                    // Add the deserialized data to the DbContext
                    // AddRange it used with sync streams to block the main thread
                    // AddRangeAsync it used with async streams to avoid blocking the main thread
                    await _dbContext.AddRangeAsync(productsSerialized);
                }
                else
                {
                    throw new Exception("No Products Data Found");
                }
            }

            // Save changes to the database
            try
            {
                // SaveChanges it used with sync streams to block the main thread
                // SaveChangesAsync it used with async streams to avoid blocking the main thread
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error while saving seeded data: {ex.Message}");
                throw;
            }
        }
    }
}