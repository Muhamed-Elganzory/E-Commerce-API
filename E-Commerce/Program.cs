using System.Reflection;
using DomainLayer.Contracts.Seed;
using DomainLayer.Contracts.Unit;
using E_Commerce.Extensions;
using E_Commerce.Factories;
using E_Commerce.Middleware.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Persistence.DB;
using Persistence.DB.Seed.Product;
using Persistence.Repository.Unit;
using Service.Images;
using Service.Mapping;
using Service.Service;
using serviceAbstraction.Contracts.Service;
using Shared.Errors;

namespace E_Commerce;

public abstract class Program
{
    public static async Task Main(string[] args)
    {
        #region Builder Configuration

        // Create a builder for the web application host.
        // It initializes a new instance of the WebApplicationBuilder class,
        // which is used to configure and build a web application host.
        var builder = WebApplication.CreateBuilder(args);

        // Add controllers to the service collection for handling HTTP requests and responses
        // in an MVC or Web API application. It enables the application to use controller classes
        // to define endpoints and actions.
        builder.Services.AddControllers();

        // Add services to generate Swagger documentation for the API
        // and to explore the API endpoints via a web interface.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #endregion

        /* My Code */
        #region Dependency Injection Services

        // Separate all services related of InfraStructure
        builder.Services.InfraStructureService(builder.Configuration);

        // Separate all services related of Core
        builder.Services.CoreServices();

        // Separate all services related of Presentation || Controllers
        builder.Services.PresentationServices();

        // Data Seeding Service
        // Register the IDataSeeding interface with its implementation DataSeeding in the DI container.
        // This allows the application to use the DataSeeding service for seeding initial data into
        // the database when needed.
        builder.Services.AddScoped<IDataSeeding, DataSeeding>();


        // Register PictureUrlResolver as a Singleton service in the Dependency Injection (DI) container.
        builder.Services.AddSingleton<PictureUrlResolver>();



        #endregion

        #region Build App

        // Build the web application host using the configured services and middleware pipeline.
        var app = builder.Build();

        #endregion

        #region Custom Meddileware

        // Apply Custom Middleware
        app.UseMiddleware<CustomExceptionHandlerMiddleware>();

        #endregion

        /* My Code */
        #region Data Seeding

        // Create a scope to obtain a reference to the database context (StoreDbContext)
        using (var scope = app.Services.CreateScope())
        {
            // Get the IDataSeeding service from the scoped service provider
            var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeding>();

            // Call the DataSeed method to seed the database with initial data
            await seeder.DataSeedAsync();
        }

        #endregion

        #region Middleware Pipeline

        // Configure the HTTP request pipeline.
        // Enable Swagger UI only in development environment.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Enforce HTTPS by redirecting HTTP requests to HTTPS endpoints
        app.UseHttpsRedirection();

        /* My Code */

        #region Static Files

        // Enables serving static files (e.g., images, CSS, JS) from the wwwroot folder.
        app.UseStaticFiles();

        #endregion

        // Enable routing to match incoming requests to the appropriate endpoints
        app.UseRouting();

        // Enable authorization middleware to enforce access control policies
        app.UseAuthorization();

        // Map controller routes to the endpoints defined in the controllers.
        // This allows the application to handle requests using the defined controllers and their actions.
        app.MapControllers();

        #endregion

        // Run the web application and start listening for incoming HTTP requests.
        // This method blocks the calling thread until the application is shut down.
        await app.RunAsync();
    }
}
