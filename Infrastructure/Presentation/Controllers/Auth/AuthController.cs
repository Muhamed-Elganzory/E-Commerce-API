using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Shared;
using serviceAbstraction.Contracts.Service;
using Shared.DTO.Auth;

namespace Presentation.Controllers.Auth;

/// <summary>
///     Handles user authentication and profile management operations such as
///     login, registration, and address management.
/// </summary>
/// <remarks>
///     All endpoints returning user data use JWT-based authentication to ensure secure access.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IServiceManager serviceManager) : BaseController
{
    /// <summary>
    ///     Provides access to all authentication-related application services.
    /// </summary>
    private readonly IServiceManager _serviceManager = serviceManager;

    /// <summary>
    ///     Authenticates a user using their email and password.
    /// </summary>
    /// <param name="loginDto">
    ///     Contains user credentials (email and password) for login.
    /// </param>
    /// <returns>
    ///     Returns a <see cref="UserResponseDto"/> containing basic user details and a JWT token.
    /// </returns>
    /// <response code="200">Login successful — token and user info returned.</response>
    /// <response code="401">Invalid credentials — unauthorized access.</response>
    [HttpPost("Login")] // Route => {{BaseURL}}/api/Auth/Login
    public async Task<ActionResult<UserResponseDto>> Login(LoginDto loginDto)
    {
        var user = await _serviceManager.AuthenticationService.LoginAsync(loginDto);
        return Ok(user);
    }

    /// <summary>
    ///     Registers a new user with the system.
    /// </summary>
    /// <param name="registerDto">
    ///     Contains the user’s registration data (email, password, username, display name, etc.).
    /// </param>
    /// <returns>
    ///     Returns a <see cref="UserResponseDto"/> with the registered user's info and JWT token.
    /// </returns>
    /// <response code="200">Registration successful.</response>
    /// <response code="400">Validation failed — bad request (e.g., weak password, email in use).</response>
    [HttpPost("Register")] // Route => {{BaseURL}}/api/Auth/Register
    public async Task<ActionResult<UserResponseDto>> Register(RegisterDto registerDto)
    {
        var user = await _serviceManager.AuthenticationService.RegisterAsync(registerDto);
        return Ok(user);
    }

    /// <summary>
    ///     Checks whether a user exists with the given email address.
    /// </summary>
    /// <param name="email">
    ///     The email address to check — passed as a query parameter.
    /// </param>
    /// <returns>
    ///     Returns <c>true</c> if the email is already registered; otherwise, <c>false</c>.
    /// </returns>
    [HttpGet("CheckEmail")] // Route => {{BaseURL}}/api/Auth/CheckEmail
    public async Task<ActionResult<bool>> CheckEmail(string email)
    {
        var result = await _serviceManager.AuthenticationService.CheckEmailAsync(email);
        return Ok(result);
    }

    /// <summary>
    ///     Retrieves the currently authenticated user's profile information.
    /// </summary>
    /// <remarks>
    ///     Requires a valid JWT token in the Authorization header.
    /// </remarks>
    /// <returns>
    ///     A <see cref="UserResponseDto"/> containing the user's email, display name, and token.
    /// </returns>
    /// <response code="200">User data successfully retrieved.</response>
    /// <response code="401">Unauthorized — missing or invalid token.</response>
    [Authorize]
    [HttpGet("GetCurrentUser")] // Route => {{BaseURL}}/api/Auth/GetCurrentUser
    public async Task<ActionResult<UserResponseDto>> GetCurrentUser()
    {
        // Auth Type => Bearer Token [TOKEN]
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (email == null)
            return Unauthorized();

        var user = await _serviceManager.AuthenticationService.GetCurrentUserAsync(email);
        return Ok(user);
    }

    /// <summary>
    ///     Retrieves the address associated with the currently authenticated user.
    /// </summary>
    /// <remarks>
    ///     Requires a valid JWT token in the Authorization header.
    /// </remarks>
    /// <returns>
    ///     Returns an <see cref="AddressDto"/> containing the user's saved address details.
    /// </returns>
    /// <response code="200">Address successfully retrieved.</response>
    /// <response code="401">Unauthorized — missing or invalid token.</response>
    [Authorize]
    [HttpGet("GetAddress")] // Route => {{BaseURL}}/api/Auth/GetAddress
    public async Task<ActionResult<AddressDto>> GetCurrentUserAddress ()
    {
        // Auth Type => Bearer Token [TOKEN]
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (email == null)
            return Unauthorized();

        var address = await _serviceManager.AuthenticationService.GetCurrentUserAdressAsync(email);
        return Ok(address);
    }

    /// <summary>
    ///     Updates or adds the address for the currently authenticated user.
    /// </summary>
    /// <param name="addressDto">
    ///     The updated address information.
    /// </param>
    /// <remarks>
    ///     Requires authentication (JWT token).
    /// </remarks>
    /// <returns>
    ///     Returns the updated <see cref="AddressDto"/> after saving changes.
    /// </returns>
    /// <response code="200">Address successfully updated.</response>
    /// <response code="401">Unauthorized — missing or invalid token.</response>
    [Authorize]
    [HttpPut("Address")] // Route => {{BaseURL}}/api/Auth/Address
    public async Task<ActionResult<AddressDto>> UpdateOrCreateCurrentUserAdress(AddressDto addressDto)
    {
        // Auth Type => Bearer Token [TOKEN]
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (email == null)
            return Unauthorized();

        var updatedAddress = await _serviceManager.AuthenticationService.UpdateOrCreateCurrentUserAdressAsync(email, addressDto);
        return Ok(updatedAddress);
    }
}