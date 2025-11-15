using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DomainLayer.Exceptions.Auth;
using DomainLayer.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper) : IAuthenticationService
    {
        /// <summary>
        ///     Instance of <see cref="UserManager{TUser}"/> used to interact with user data.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        /// <summary>
        ///     Provides access to application configuration settings.
        ///     Used primarily for retrieving JWT configuration values such as
        ///     <c>Issuer</c>, <c>Audience</c>, and <c>SecretKey</c>.
        /// </summary>
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        ///     Provides object-object mapping capabilities between entities and DTOs
        ///     using AutoMapper. Simplifies data transfer between layers.
        /// </summary>
        private readonly IMapper _mapper = mapper;

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
        ///     Checks whether a user with the given email exists in the system.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>
        ///     <c>true</c> if a user with the provided email exists; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> CheckEmailAsync(string email)
        {
            // 🔹 Try to find the user by email in the Identity store
            var user = await _userManager.FindByEmailAsync(email);

            // 🔹 Return true if the user exists, otherwise false
            return user is not null;
        }

        /// <summary>
        ///     Retrieves the address information of the currently logged-in user.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>
        ///     An <see cref="AddressDto"/> object containing the user's address information.
        /// </returns>
        /// <exception cref="UserNotFoundException">
        ///     Thrown if no user is found with the provided email.
        /// </exception>
        /// <exception cref="AddressNotFoundException">
        ///     Thrown if the user exists but has no address associated.
        /// </exception>
        public async Task<AddressDto> GetCurrentUserAdressAsync(string email)
        {
            // 🔹 Load user with their address (via eager loading)
            var user = await _userManager.Users
                .Include(a => a.Address)
                .FirstOrDefaultAsync(e => e.Email == email)
                ?? throw new UserNotFoundException(email); // ❌ User not found

            // 🔹 Ensure the user has an address record
            if (user.Address is null)
                throw new AddressNotFoundException(user.Email!);

            // 🔹 Map the UserAddress entity to an AddressDto and return it
            return _mapper.Map<UserAddress, AddressDto>(user.Address);
        }

        /// <summary>
        ///     Updates or creates the address record for the specified user.
        /// </summary>
        /// <param name="email">The user's email used to locate the user in the database.</param>
        /// <param name="updateAddressDto">The new address data to be saved.</param>
        /// <returns>
        ///     An updated <see cref="AddressDto"/> reflecting the user's current address.
        /// </returns>
        /// <exception cref="UserNotFoundException">
        ///     Thrown if no user exists with the specified email.
        /// </exception>
        public async Task<AddressDto> UpdateOrCreateCurrentUserAdressAsync(string email, AddressDto updateAddressDto)
        {
            // 🔹 Load the user and their current address
            var user = await _userManager.Users
                .Include(a => a.Address)
                .FirstOrDefaultAsync(e => e.Email == email)
                ?? throw new UserNotFoundException(email);

            // 🔹 If user already has an address → update fields
            if (user.Address is not null)
            {
                user.Address.City = updateAddressDto.City;
                user.Address.Street = updateAddressDto.Street;
                user.Address.Country = updateAddressDto.Country;
                user.Address.LastName = updateAddressDto.LastName;
                user.Address.FirstName = updateAddressDto.FirstName;
            } else
            {
                // 🔹 Otherwise, create a new address record
                user.Address = _mapper.Map<AddressDto, UserAddress>(updateAddressDto);
            }

            // 🔹 Persist the updated user and address to the database
            await _userManager.UpdateAsync(user);

            // 🔹 Return the updated address as a DTO
            return _mapper.Map<UserAddress, AddressDto>(user.Address);
        }

        /// <summary>
        ///     Retrieves the basic profile information of the currently authenticated user,
        ///     including a fresh JWT token.
        /// </summary>
        /// <param name="email">The email address of the authenticated user.</param>
        /// <returns>
        ///     A <see cref="UserResponseDto"/> containing user details and a new authentication token.
        /// </returns>
        /// <exception cref="UserNotFoundException">
        ///     Thrown if no user exists with the specified email.
        /// </exception>
        public async Task<UserResponseDto> GetCurrentUserAsync(string email)
        {
            // 🔹 Attempt to find the user by email
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new UserNotFoundException(email);

            // 🔹 Return the basic user info + a freshly generated JWT token
            return new UserResponseDto()
            {
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = await CreateTokenAsync(user)
            };
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