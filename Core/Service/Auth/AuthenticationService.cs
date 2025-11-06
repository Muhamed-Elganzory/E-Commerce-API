using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DomainLayer.Exceptions.Auth;
using DomainLayer.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction.Contracts.Auth;
using Shared.DTO.Auth;

namespace Service.Auth
{
    /// <summary>
    ///     Provides implementation for authentication-related operations such as
    ///     user login, registration, and logout.
    /// </summary>
    /// <remarks>
    ///     This service uses <see cref="UserManager{TUser}"/> to manage user identity
    ///     operations like verifying passwords, creating users, and handling tokens.
    /// </remarks>
    /// <param name="userManager">The ASP.NET Core Identity user manager service.</param>
    public class AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration) : IAuthenticationService
    {
        /// <summary>
        ///     Instance of <see cref="UserManager{TUser}"/> used to interact with user data.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        /// <summary>
        ///
        /// </summary>
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        ///     Authenticates a user by validating their email and password.
        /// </summary>
        /// <param name="loginDto">The login credentials provided by the user.</param>
        /// <returns>
        ///     A <see cref="UserResponseDto"/> containing basic user information and a generated token.
        /// </returns>
        /// <exception cref="UserNotFoundException">Thrown when no user is found with the given email.</exception>
        /// <exception cref="UnauthorizedException">Thrown when the provided password is invalid.</exception>
        public async Task<UserResponseDto> LoginAsync(LoginDto loginDto)
        {
            // 🔹 Find the user by email
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            // ❌ If not found, throw a custom exception
            if (user == null) throw new UserNotFoundException(loginDto.Email);;

            // 🔹 Validate password
            var passwordIsValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            // ✅ Return user data if valid
            if (passwordIsValid)
            {
                return new UserResponseDto()
                {
                    Token = await CreateTokenAsync(user), // TODO: Generate JWT token here
                    Email = loginDto.Email,
                    DisplayName = user.DisplayName
                };
            }

            // ❌ If password is invalid, throw unauthorized exception
            throw new UnauthorizedException();
        }

        /// <summary>
        ///     Registers a new user in the system using the provided information.
        /// </summary>
        /// <param name="registerDto">The registration data (email, password, username, etc.).</param>
        /// <returns>
        ///     A <see cref="UserResponseDto"/> containing the registered user info and JWT token.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     This method is not yet implemented.
        /// </exception>
        public async Task<UserResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
                DisplayName = registerDto.DisplayName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return new UserResponseDto()
                {
                    Email = registerDto.Email,
                    Token = await CreateTokenAsync(user),
                    DisplayName = registerDto.DisplayName
                };
            }

            var errors = result.Errors.Select(e => e.Description).ToList();

            throw new BadRequestException(errors);
        }

        /// <summary>
        ///     Logs out the currently authenticated user by invalidating their session or token.
        /// </summary>
        /// <returns>
        ///     A boolean indicating whether the logout operation succeeded (<c>true</c>) or failed (<c>false</c>).
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     This method is not yet implemented.
        /// </exception>
        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Generates a JWT (JSON Web Token) for the specified user.
        ///     The token includes user information, roles, and an expiration time.
        /// </summary>
        /// <param name="user">The authenticated user for whom the token will be created.</param>
        /// <returns>A signed JWT token string.</returns>
        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            // 🟢 1️⃣ Validate input
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            // 🟢 2️⃣ ---------------- HEADER ----------------
            // Define the signing algorithm and prepare the security key
            var secretKey = _configuration["JwtOptions:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException(nameof(secretKey), "JWT Secret Key is missing in configuration.");

            // Convert the secret key to bytes and create the symmetric key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Create signing credentials using the key and HMAC-SHA256 algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 🟢 3️⃣ ---------------- PAYLOAD ----------------
            // Define user-related claims (basic information)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            // Get user roles and add them as separate claims
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 🕒 Define token expiration (valid for 1 hour)
            var expirationTime = DateTime.UtcNow.AddHours(1);

            // 🟢 4️⃣ ---------------- SIGNATURE ----------------
            // Build the JWT token with header, payload, and signature
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],     // Token issuer
                audience: _configuration["JwtOptions:Audience"], // Token audience
                claims: claims,                                  // Payload (user info + roles)
                expires: expirationTime,                         // Expiration time
                signingCredentials: creds                        // Signature (Header + Key)
            );

            // 🟢 5️⃣ ---------------- FINAL TOKEN ----------------
            // Serialize the JWT into a compact string format: Header.Payload.Signature
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Return the complete token string
            return jwtToken;
        }
    }
}