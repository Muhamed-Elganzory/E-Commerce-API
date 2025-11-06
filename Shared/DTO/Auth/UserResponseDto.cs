namespace Shared.DTO.Auth
{
    /// <summary>
    ///     Represents the data returned to the client after a successful
    ///     authentication or registration process.
    ///     This DTO typically contains basic user information and an access token.
    /// </summary>
    public class UserResponseDto
    {
        /// <summary>
        ///     Gets or sets the user's email address.
        ///     This identifies the user and may be used in further requests.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the user's display name.
        ///     This value is typically shown in the UI as the user's friendly name.
        /// </summary>
        public string DisplayName { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the JSON Web Token (JWT) assigned to the user.
        ///     This token is used for authentication and authorization in subsequent API requests.
        /// </summary>
        public string Token { get; set; } = null!;
    }
}