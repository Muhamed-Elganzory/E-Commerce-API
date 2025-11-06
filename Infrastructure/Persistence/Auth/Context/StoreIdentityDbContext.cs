using DomainLayer.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Auth.Context
{
    /// <summary>
    ///     Represents the Identity database context used for managing
    ///     user authentication, authorization, and related entities.
    /// </summary>
    /// <remarks>
    ///     This DbContext inherits from <see cref="IdentityDbContext{TUser}"/>
    ///     and uses the ApplicationUser entity as the custom user model.
    /// </remarks>
    public class StoreIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="StoreIdentityDbContext"/> using the specified options.
        /// </summary>
        /// <param name="options">The options used to configure the context.</param>
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        ///     Configures the database model for Identity entities.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Always call the base to ensure Identity tables and relationships are created
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAddress>().ToTable("UserAddress");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            // ⚠️ Recommended: DO NOT ignore these unless you *really* want to remove those tables
            // These hold data for claims, logins, and tokens used by ASP.NET Identity
            // If you ignore them, features like external login, claims, and tokens will not work
            //
            // modelBuilder.Ignore<IdentityUserClaim<string>>();
            // modelBuilder.Ignore<IdentityUserLogin<string>>();
            // modelBuilder.Ignore<IdentityUserToken<string>>();

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Address)
                .WithOne(a => a.ApplicationUser)
                .HasForeignKey<UserAddress>(a => a.UserId);
        }
    }
}
