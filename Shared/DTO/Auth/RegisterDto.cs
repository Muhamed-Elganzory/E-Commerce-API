using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Auth
{
    /// <summary>
    ///     Represents the data transfer object (DTO) used when registering a new user.
    ///     This class defines the required information for creating an account.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        ///     Gets or sets the user's email address.
        ///     The [EmailAddress] attribute ensures the value is in a valid email format.
        ///     This will also serve as the primary identifier for login.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the user's password.
        ///     The password must meet the security requirements defined in the Identity options.
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the username chosen by the user.
        ///     This is typically displayed within the application as a unique identifier.
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the user's display name.
        ///     This can be used as a friendly name shown in the UI instead of the username.
        /// </summary>
        public string DisplayName { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the user's phone number.
        ///     This can be used for two-factor authentication or contact purposes.
        /// </summary>
        public string PhoneNumber { get; set; } = null!;
    }
}