using Shared.DTO.Auth;

namespace ServiceAbstraction.Contracts.Auth
{
    /// <summary>
    ///     Defines the contract for user authentication and account-related operations.
    /// </summary>
    public interface IAuthenticationService
    {
        // ================================================================
        // 🔹 LOGIN
        // ================================================================

        /// <summary>
        ///     Authenticates an existing user by verifying their email and password.
        /// </summary>
        /// <param name="loginDto">
        ///     The DTO containing user's login credentials (Email - Password).
        /// </param>
        /// <returns>
        ///     Returns a <see cref="UserResponseDto"/> with user info and JWT token upon successful authentication.
        /// </returns>
        public Task<UserResponseDto> LoginAsync(LoginDto loginDto);

        // ================================================================
        // 🔹 REGISTER
        // ================================================================

        /// <summary>
        ///     Registers a new user and returns the user's details with a generated JWT token.
        /// </summary>
        /// <param name="registerDto">
        ///     DTO containing new user's registration details (Email, Username, DisplayName, Password).
        /// </param>
        /// <returns>
        ///     Returns a <see cref="UserResponseDto"/> representing the newly created user.
        /// </returns>
        public Task<UserResponseDto> RegisterAsync(RegisterDto registerDto);

        // ================================================================
        // 🔹 EMAIL VALIDATION
        // ================================================================

        /// <summary>
        ///     Checks whether an email already exists in the system (used for registration validation).
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>
        ///     True if the email exists; otherwise, false.
        /// </returns>
        public Task<bool> CheckEmailAsync(string email);

        // ================================================================
        // 🔹 USER ADDRESS MANAGEMENT
        // ================================================================

        /// <summary>
        ///     Retrieves the current user's saved address information.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>
        ///     Returns an <see cref="AddressDto"/> containing the user's address details.
        /// </returns>
        public Task<AddressDto> GetCurrentUserAdressAsync(string email);

        /// <summary>
        ///     Updates the current user's address information.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="updateAddressDto">The updated address details.</param>
        /// <returns>
        ///     Returns an updated <see cref="AddressDto"/> after saving changes.
        /// </returns>
        public Task<AddressDto> UpdateOrCreateCurrentUserAdressAsync(string email, AddressDto updateAddressDto);

        // ================================================================
        // 🔹 CURRENT USER INFO
        // ================================================================

        /// <summary>
        ///     Retrieves the currently authenticated user's profile details.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>
        ///     Returns a <see cref="UserResponseDto"/> containing the user's basic profile info.
        /// </returns>
        public Task<UserResponseDto> GetCurrentUserAsync(string email);
    }
}