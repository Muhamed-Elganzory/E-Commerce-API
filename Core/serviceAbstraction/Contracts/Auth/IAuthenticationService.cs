using Shared.DTO.Auth;

namespace ServiceAbstraction.Contracts.Auth
{
    /// <summary>
    ///     Defines the contract for user authentication operations.
    ///     This interface provides the core methods for logging in, registering, and logging out users.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        ///     Authenticates an existing user using their credentials.
        /// </summary>
        /// <param name="loginDto">
        ///     The login data transfer object that contains the user's email and password.
        /// </param>
        /// <returns>
        ///     A <see cref="UserResponseDto"/> containing the user's basic information and JWT token upon successful login.
        /// </returns>
        public Task<UserResponseDto> LoginAsync(LoginDto loginDto);

        /// <summary>
        ///     Registers a new user in the system.
        /// </summary>
        /// <param name="registerDto">
        ///     The registration data transfer object containing user details such as email, username, display name, and password.
        /// </param>
        /// <returns>
        ///     A <see cref="UserResponseDto"/> containing the newly created user's information and authentication token.
        /// </returns>
        public Task<UserResponseDto> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        ///     Logs out the currently authenticated user.
        /// </summary>
        /// <returns>
        ///     A boolean indicating whether the logout operation succeeded (<c>true</c>) or failed (<c>false</c>).
        /// </returns>
        public Task<bool> Logout();
    }
}