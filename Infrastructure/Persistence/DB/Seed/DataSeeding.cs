using System.Text.Json;
using DomainLayer.Contracts.Seed;
using DomainLayer.Models.Auth;
using DomainLayer.Models.Order;
using DomainLayer.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.DB.Context;

namespace Persistence.DB.Seed;

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
/// <param name="userManager"></param>
/// <param name="roleManager"></param>
public class DataSeeding(StoreDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager): IDataSeeding
{
    // Property injection of DbContext
    private readonly StoreDbContext _dbContext = dbContext;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

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
            await _dbContext.Database.MigrateAsync();
        }

        // Seed Product Brands, if not already [seeded || data] add your seeding logic here
        if (!_dbContext.ProductBrands.Any())
        {
            // Read the JSON file
            // ReadAllText it used with sync streams to block the main thread
            // OpenRead it used with async streams to avoid blocking the main thread
            var brandsData = File.OpenRead("../Infrastructure/Persistence/DB/Seed/Data/Product/brands.json");

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
            var typesData = File.OpenRead("../Infrastructure/Persistence/DB/Seed/Data/Product/types.json");

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

        // ✅ Seed Products, if not already [seeded || data exists], add your seeding logic here
        if (!_dbContext.Products.Any())
        {
            // Read the JSON file asynchronously to avoid blocking the main thread
            await using var productsData = File.OpenRead("../Infrastructure/Persistence/DB/Seed/Data/Product/products.json");

            // Deserialize the JSON data to a list of Product objects
            var productsSerialized = await JsonSerializer.DeserializeAsync<List<DomainLayer.Models.Product.Product>>(productsData);

            // Check if deserialization was successful and the list is not empty
            if (productsSerialized is not null && productsSerialized.Any())
            {
                // Add the deserialized data to the DbContext asynchronously
                await _dbContext.AddRangeAsync(productsSerialized);
            }
            else
            {
                throw new Exception("❌ No Products Data Found in JSON file.");
            }

            // Save changes to the database
            try
            {
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("✅ Products seeding completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error while saving seeded products: {ex.Message}");
                throw;
            }
        }

        // ✅ Seed Delivery Methods if not already seeded
        if (!_dbContext.DeliveryMethods.Any())
        {
            // Read the JSON file asynchronously
            await using var deliveryData = File.OpenRead("../Infrastructure/Persistence/DB/Seed/Data/Order/delivery.json");

            if (deliveryData.Length == 0)
                throw new Exception("❌ Delivery JSON file is empty.");

            // Deserialize JSON to a list of DeliveryMethod objects
            var deliverySerialized = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(deliveryData);

            // Check if deserialization was successful
            if (deliverySerialized is not null && deliverySerialized.Any())
            {
                await _dbContext.DeliveryMethods.AddRangeAsync(deliverySerialized);
            }
            else
            {
                throw new Exception("❌ No delivery data found in JSON file.");
            }

            // Save changes to the database
            try
            {
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("✅ Delivery methods seeding completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error while saving seeded delivery data: {ex.Message}");
                throw;
            }
        }
    }

    /// <summary>
    ///     Seeds default roles and users into the Identity database.
    /// </summary>
    /// <remarks>
    ///     This method ensures that required roles (e.g., Admin, SuperAdmin)
    ///     and initial users are created when the application starts for the first time.
    ///
    /// Note:
    ///     - Both <see cref="_roleManager"/> and <see cref="_userManager"/> automatically
    ///     save their changes to the database when CreateAsync or AddToRoleAsync is called,
    ///     so manual SaveChanges() calls are not required.
    /// </remarks>
    public async Task IdentityDataSeedAsync()
    {
        try
        {
            // ✅ Seed default roles if they do not exist
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            // ✅ Seed default users if none exist
            if (!_userManager.Users.Any())
            {
                // Create the SuperAdmin user
                var user01 = new ApplicationUser
                {
                    DisplayName = "Mohamed Elganzory",
                    PhoneNumber = "01061650979",
                    Email = "mohamedelganzory621@gmail.com",
                    UserName = "superadmin",
                    EmailConfirmed = true
                };

                // Create the Admin user
                var user02 = new ApplicationUser
                {
                    DisplayName = "Ahmed Elganzory",
                    PhoneNumber = "01072635464",
                    Email = "ahmed@gmail.com",
                    UserName = "admin",
                    EmailConfirmed = true
                };

                // ✅ Add users to the database with default passwords
                var result01 = await _userManager.CreateAsync(user01, "P@ssw0rd");
                var result02 = await _userManager.CreateAsync(user02, "P@ssw0rd");

                if (result01.Succeeded && result02.Succeeded)
                {
                    // Reload user from DB after created
                    var createdUser1 = await _userManager.FindByNameAsync("superadmin");
                    var createdUser2 = await _userManager.FindByNameAsync("admin");

                    // ✅ Assign roles to the created users
                    if (createdUser1 != null) await _userManager.AddToRoleAsync(createdUser1, "SuperAdmin");
                    if (createdUser2 != null) await _userManager.AddToRoleAsync(createdUser2, "Admin");
                }

            }
        }
        catch (Exception e)
        {
            // ❌ Log the exception for troubleshooting
            Console.WriteLine(e);
            throw;
        }
    }
}