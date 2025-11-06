using Microsoft.AspNetCore.Identity;

namespace DomainLayer.Models.Auth
{
    /// <summary>
    ///     Represents the application-specific user entity that extends
    ///     the built-in ASP.NET Core IdentityUser class.
    /// </summary>
    /// <remarks>
    ///     This class allows adding custom properties to the user entity,
    ///     such as display name or user address, which are stored alongside
    ///     the standard Identity fields (like Email, PasswordHash, etc.).
    /// </remarks>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        ///     Gets or sets the display name for the user.
        ///     This can be shown in the UI instead of the username.
        /// </summary>
        public string DisplayName { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the user's address information.
        ///     This establishes a one-to-one relationship with the UserAddress entity.
        /// </summary>
        public UserAddress? Address { get; set; } = null!;
    }
}