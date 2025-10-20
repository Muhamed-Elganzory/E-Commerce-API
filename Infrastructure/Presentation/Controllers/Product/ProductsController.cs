using Microsoft.AspNetCore.Mvc;
using serviceAbstraction.Contracts.Service;
using Shared.DTO.Product;

namespace Presentation.Controllers.Product;

/// <summary>
///     Products Controller — responsible for handling all HTTP requests related to products.
///     This class defines RESTful API endpoints for managing and retrieving products, brands, and types.
///     It acts as a bridge between the client and the business logic layer (services),
///     answering *how* product-related operations are performed through the service layer.
/// </summary>
/// <remarks>
///     ⚙️ Make sure to add the following Framework Reference inside **Presentation.csproj**:
///     <code>
///         ItemGroup
///             FrameworkReference Include="Microsoft.AspNetCore.App"
///         ItemGroup
///     </code>
///
///     ✅ Ensure these attributes are added above the controller:
///     - **[ApiController]** → Marks the class as an API controller, enabling features like automatic model validation and parameter binding.
///     - **[Route]** → Defines the routing pattern for the controller.
///       "[controller]" is replaced automatically by the controller’s name (without the "Controller" suffix).
///     <code>
///         [ApiController]
///         [Route("api/[controller]")]
///     </code>
/// </remarks>
/// <example>
///     [Route("[controller]")] → BaseURL/Products
///     [Route("api/[controller]")] → BaseURL/api/Products
/// </example>
/// <param name="serviceManager">Injected service manager used to access product-related business logic.</param>
[ApiController]
// [Route("[controller]")]
[Route("api/[controller]")]
public class ProductsController(IServiceManager serviceManager): ControllerBase
{
    /// <summary>
    ///     Reference to the Service Manager, which provides access to all business logic services.
    /// </summary>
    private readonly IServiceManager _serviceManager = serviceManager;

    /// <summary>
    ///     Handles GET requests to retrieve all products.
    ///     It calls the ProductService to fetch all products asynchronously and returns them as JSON.
    /// </summary>
    /// <returns>HTTP 200 OK with product list, or 404 Not Found if empty.</returns>
    [HttpGet] // GET: /Products or /api/Products
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        var products = await _serviceManager.ProductService.GetAllProductsAsync();

        if (!products.Any()) return NotFound();

        return Ok(products);
    }

    /// <summary>
    ///     Handles GET requests to retrieve a single product by ID.
    ///     It demonstrates how the controller distinguishes this action from other GET endpoints using a route parameter.
    /// </summary>
    /// <remarks>
    ///     Example route:
    ///     <code>
    ///         [HttpGet("{id:int}")]
    ///     </code>
    /// </remarks>
    /// <param name="id">Product ID to fetch. Must be an integer.</param>
    /// <returns>HTTP 200 OK with the product, or 404 Not Found if not found.</returns>
    [HttpGet("{id:int}")] // GET: /Products/1 or /api/Products/1
    public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id)
    {
        var product = await _serviceManager.ProductService.GetProductByIdAsync(id);

        if (product is null) return NotFound();

        return Ok(product);
    }

    /// <summary>
    ///     Handles GET requests to retrieve all product types.
    ///     It calls the ProductService to fetch type data and returns it as JSON.
    /// </summary>
    /// <returns>HTTP 200 OK with list of types, or 404 Not Found if none exist.</returns>
    [HttpGet("types")] // GET: /Products/types or /api/Products/types
    public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypesAsync()
    {
        var types = await _serviceManager.ProductService.GetAllTypesAsync();

        if (!types.Any()) return NotFound();

        return Ok(types);
    }

    /// <summary>
    ///     Handles GET requests to retrieve all product brands.
    ///     It calls the ProductService to fetch brand data and returns it as JSON.
    /// </summary>
    /// <returns>HTTP 200 OK with list of brands, or 404 Not Found if none exist.</returns>
    [HttpGet("brands")] // GET: /Products/brands or /api/Products/brands
    public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrandsAsync()
    {
        var brands = await _serviceManager.ProductService.GetAllBrandsAsync();

        if (!brands.Any()) return NotFound();

        return Ok(brands);
    }
}
