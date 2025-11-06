using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Auth
{
    /// <summary>
    ///     Represents the data transfer object (DTO) used for user login.
    ///     This class defines the information required when a user attempts to sign in.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        ///     Gets or sets the user's email address.
        ///     This property is used as the username during authentication.
        ///     The [EmailAddress] attribute ensures the value follows a valid email format.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the user's password.
        ///     This is required to verify the user's identity during login.
        /// </summary>
        public string Password { get; set; } = null!;
    }
}