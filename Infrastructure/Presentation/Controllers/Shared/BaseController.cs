using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Shared
{
    /// <summary>
    ///     Serves as the base controller for all API controllers in the application.
    ///     <para>
    ///         Provides a common configuration and routing convention (e.g., <c>api/[controller]</c>),
    ///         ensuring consistent API behavior across all derived controllers.
    ///     </para>
    ///     <remarks>
    ///         This class can also be extended to include shared logic such as:
    ///         <list type="bullet">
    ///             <item>Standardized API responses</item>
    ///             <item>Common exception handling</item>
    ///             <item>Access to the authenticated user's claims or email</item>
    ///             <item>Integration with a shared <c>IServiceManager</c></item>
    ///         </list>
    ///     </remarks>
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        // Future extension point:
        // Protected properties for shared access (e.g., IServiceManager, ILogger, etc.)
        //
        // Example:
        // protected readonly IServiceManager _serviceManager;
        //
        // public BaseController(IServiceManager serviceManager)
        // {
        //     _serviceManager = serviceManager;
        // }

        // For now, it's an empty base class to provide a consistent routing convention.

        /// <summary>
        ///     Retrieves the authenticated user's email from the JWT access token.
        /// </summary>
        /// <returns>
        ///     The email address extracted from the token's claims,
        ///     or <c>null</c> if the claim is missing.
        /// </returns>
        /// <remarks>
        ///     This helper method centralizes claim extraction logic,
        ///     allowing derived controllers to easily access the authenticated
        ///     user's email without duplicating code.
        ///
        ///     <para>
        ///         It reads the <see cref="ClaimTypes.Email"/> claim from the
        ///         current request's <see cref="ControllerBase.User"/> principal.
        ///     </para>
        /// </remarks>
        protected string? GetEmailFromClaimsToken()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (email == null) return null;

            return email;
        }

    }
}