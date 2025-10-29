using Microsoft.AspNetCore.Mvc;
using Service.Spec.Product;
using serviceAbstraction.Contracts.Product;
using serviceAbstraction.Contracts.Service;
using Shared.DTO.Product;
using Shared.Enums.Product;
using Shared.Pagination;
using Shared.Queries;

namespace Presentation.Controllers.Product;

/// <summary>
///     The <see cref="ProductsController"/> is responsible for handling all HTTP requests related to products.
///     It defines RESTful API endpoints for managing and retrieving products, brands, and types.
///     This controller acts as a bridge between the client (API consumer) and the business logic layer (services),
///     delegating *how* product-related operations are performed through the <see cref="IServiceManager"/>.
/// </summary>
/// <remarks>
///     ⚙️ **Framework Reference:**
///     Ensure the following reference is added to your <c>Presentation.csproj</c> file:
///     <code>
///     <ItemGroup>
///         <FrameworkReference Include="Microsoft.AspNetCore.App" />
///     </ItemGroup>
///     </code>
///
///     ✅ **Controller Attributes:**
///     - <b>[ApiController]</b> → Marks the class as an API controller, enabling automatic model validation and parameter binding.
///     - <b>[Route]</b> → Defines the routing pattern for the controller.
///       The token <c>[controller]</c> is automatically replaced by the controller’s name (without the "Controller" suffix).
///     <code>
///     [ApiController]
///     [Route("api/[controller]")]
///     </code>
/// </remarks>
/// <example>
///     <b>Example Routes:</b>
///     <code>
///     [Route("[controller]")] → BaseURL/Products
///     [Route("api/[controller]")] → BaseURL/api/Products
///     </code>
/// </example>
/// <param name="serviceManager">
///     The injected <see cref="IServiceManager"/> instance used to access product-related business logic.
/// </param>
[ApiController]
// [Route("[controller]")]
[Route("api/[controller]")]
public class ProductsController(IServiceManager serviceManager) : ControllerBase
{
    /// <summary>
    ///     Reference to the Service Manager, which provides access to all business logic services.
    /// </summary>
    private readonly IServiceManager _serviceManager = serviceManager;

    /// <summary>
    ///     Handles HTTP GET requests to retrieve all available <see cref="ProductDto"/> objects.
    ///     This endpoint delegates data retrieval to the <see cref="IProductService"/>,
    ///     which internally applies the **Specification Pattern** for filtering, sorting, and eager loading
    ///     of related data such as <c>ProductBrand</c> and <c>ProductType</c>.
    /// </summary>
    /// <param name="queryParams">
    ///     Query parameters received from the request’s query string, encapsulated in a <see cref="ProductQueryParams"/> object. <br/>
    ///     This object contains:
    ///     <list type="bullet">
    ///         <item><description><c>BrandId</c> — Optional brand filter. If <c>null</c>, all brands are included.</description></item>
    ///         <item><description><c>TypeId</c> — Optional type filter. If <c>null</c>, all types are included.</description></item>
    ///         <item><description><c>SortingOptions</c> — Determines the sorting order using <see cref="ProductSortingOptions"/> (e.g., PriceDescending).</description></item>
    ///     </list>
    /// </param>
    /// <returns>
    ///     Returns an <see cref="ActionResult{T}"/> containing:
    ///     <list type="bullet">
    ///         <item><description><c>200 OK</c> — if one or more products are found.</description></item>
    ///         <item><description><c>404 Not Found</c> — if no products match the filtering criteria.</description></item>
    ///     </list>
    /// </returns>
    /// <remarks>
    ///     🧩 **Flow Overview:**
    ///     Controller → Service Layer → Repository → SpecificationEvaluator
    ///     - The **controller** receives filtering and sorting parameters via the <see cref="ProductQueryParams"/> model (from the query string).
    ///     - The **service layer** constructs a specification (<see cref="ProductWithBrandAndTypeBaseSpecifications"/>)
    ///       that encapsulates filtering, sorting, and eager loading logic.
    ///     - The **repository** uses the specification to build the actual EF Core query.
    ///     - The results are mapped to DTOs using AutoMapper and returned to the client.
    /// </remarks>
    /// <example>
    ///     Example usage:
    ///     <code>
    ///         // Retrieve all products
    ///         GET /api/Products
    ///
    ///         // Retrieve products filtered by brand
    ///         GET /api/Products?brandId=2
    ///
    ///         // Retrieve products filtered by brand and type
    ///         GET /api/Products?brandId=2&amp;typeId=3
    ///
    ///         // Retrieve products sorted by price descending
    ///         GET /api/Products?sortingOptions=PriceDescending
    ///     </code>
    /// </example>
    [HttpGet] // GET: /Products or /api/Products
    // Task<ActionResult<IEnumerable<BrandDto>>> Before apply pagination ==> PaginatedResult<ProductDto>
    public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProductsAsync([FromQuery] ProductQueryParams queryParams)
    {
        // Call ProductService to fetch all products asynchronously (optionally filtered)
        var products = await _serviceManager.ProductService.GetAllProductsAsync(queryParams);

        // Return 404 if no products found
        // if (!products.Any()) return NotFound();

        // Return 200 OK with the product list
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
