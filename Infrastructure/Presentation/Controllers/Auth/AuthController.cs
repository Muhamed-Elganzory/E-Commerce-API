using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Shared;
using serviceAbstraction.Contracts.Service;
using Shared.DTO.Auth;

namespace Presentation.Controllers.Auth;

/// <summary>
///
/// </summary>
/// <param name="serviceManager"></param>
public class AuthController(IServiceManager serviceManager): BaseController
{
    /// <summary>
    ///
    /// </summary>
    private readonly IServiceManager _serviceManager = serviceManager;

    /// <summary>
    ///
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<ActionResult<UserResponseDto>> Login(LoginDto loginDto)
    {
        var user = await _serviceManager.AuthenticationService.LoginAsync(loginDto);
        return Ok(user);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    [HttpPost("Register")]
    public async Task<ActionResult<UserResponseDto>> Register(RegisterDto registerDto)
    {
        var user = await _serviceManager.AuthenticationService.RegisterAsync(registerDto);
        return Ok(user);
    }
}