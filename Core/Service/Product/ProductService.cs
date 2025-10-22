using AutoMapper;
using DomainLayer.Contracts.Unit;
using serviceAbstraction.Contracts.Product;
using DomainLayer.Models.Product;
using Service.Spec.Product;
using Shared.DTO.Product;

namespace Service.Product;

/// <summary>
///     Represents the service layer responsible for handling product-related business logic,
///     including operations on products, brands, and types.
/// </summary>
/// <remarks>
///     This class defines how the business logic operates — how data is retrieved, processed,
///     and returned to the controller.
///     It uses the <see cref="IUnitOfWork"/> pattern to coordinate repository operations,
///     and <see cref="IMapper"/> (AutoMapper) to map between domain entities and DTOs.
/// </remarks>
/// <param name="unitOfWork">
///     The <see cref="IUnitOfWork"/> instance used to manage and coordinate repository transactions.
/// </param>
/// <param name="mapper">
///     The <see cref="IMapper"/> instance used for mapping between domain models and data transfer objects (DTOs).
/// </param>
public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    ///     Asynchronously retrieves all <see cref="Product"/> entities from the repository
    ///     using a specification that includes related <c>ProductBrand</c> and <c>ProductType</c> data,
    ///     then maps the results to <see cref="ProductDto"/> objects using AutoMapper.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains a collection of <see cref="ProductDto"/> objects representing the products.
    /// </returns>
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        // (Option 1) Create repository separately, then call GetAllAsync():
        // var repo = await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Product.Product, int>();
        // var products = await repo.GetAllAsync();

        // (Option 2) One-line approach to create repository and retrieve all products:
        // var repo = await (await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Product.Product, int>()).GetAllAsync();

        // (Option 3 - Recommended) Using Specification Pattern:
        // 1️⃣ Create a specification that defines relationships to include (ProductBrand, ProductType)
        var specification = new ProductWithBrandAndTypeSpecifications();

        // 2️⃣ Use the specification to retrieve all products including their related data
        var products = await (await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Product.Product, int>()).GetAllAsync(specification);

        // 3️⃣ Map the products to ProductDto objects using AutoMapper
        var productData = _mapper.Map<IEnumerable<DomainLayer.Models.Product.Product>, IEnumerable<ProductDto>>(products);

        // 4️⃣ Return the mapped product data
        return productData;
    }

    /// <summary>
    ///     Asynchronously retrieves a single <see cref="ProductDto"/> object by its unique identifier.
    ///     This method uses the Specification Pattern to include related entities (<c>ProductBrand</c>, <c>ProductType</c>)
    ///     and AutoMapper to map the domain entity to a DTO.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the product to retrieve.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation.
    ///     The task result contains a <see cref="ProductDto"/> if found, or <c>null</c> if the product does not exist.
    /// </returns>
    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        // 1️⃣ Create repository for Product entity asynchronously.
        //    The repository provides data access to Product entities through the Unit of Work pattern.
        var repo = await _unitOfWork.CreateRepositoryAsync<DomainLayer.Models.Product.Product, int>();

        // ─────────────────────────────────────────────
        // 🧾 (Old Approach - Without Specification)
        // ---------------------------------------------
        // This older method directly retrieves a product by its id without including any related entities.
        // It’s simpler but does not eager-load ProductBrand or ProductType,
        // which might cause additional database queries later (lazy loading).
        //
        // Example:
        // var product = await repo.GetByIdAsync(id);
        // ─────────────────────────────────────────────

        // ─────────────────────────────────────────────
        // ✅ (Recommended Approach - Using Specification Pattern)
        // ---------------------------------------------
        // 2️⃣ Create a specification that filters the product by id
        //     and includes its related entities (ProductBrand, ProductType).
        //     This ensures all related data is loaded in a single optimized query.
        var specification = new ProductWithBrandAndTypeSpecifications(id);

        // 3️⃣ Use the specification to retrieve the product that matches the criteria.
        var product = await repo.GetByIdAsync(specification);
        // ─────────────────────────────────────────────

        // 4️⃣ Map the domain entity (Product) to its DTO (ProductDto) using AutoMapper.
        var productData = _mapper.Map<DomainLayer.Models.Product.Product?, ProductDto?>(product);

        // 5️⃣ Return the mapped product data (or null if not found).
        return productData ?? null;
    }

    /// <summary>
    ///     Asynchronously retrieves all product brands from the repository
    ///     and maps them to <see cref="BrandDto"/> objects using AutoMapper.
    /// </summary>
    /// <returns>
    ///     A task representing the asynchronous operation.
    ///     The task result contains an <see cref="IEnumerable{BrandDto}"/> collection
    ///     of all brands available in the database.
    /// </returns>
    public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
    {
        // 1️⃣ Create repository for ProductBrand entity asynchronously.
        //    The repository is managed through the Unit of Work pattern.
        var repo = await _unitOfWork.CreateRepositoryAsync<ProductBrand, int>();

        // 2️⃣ Retrieve all ProductBrand entities from the repository.
        var brands = await repo.GetAllAsync();

        // 3️⃣ Map the domain entities (ProductBrand) to DTOs (BrandDto) using AutoMapper.
        var brandsData = _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(brands);

        // 4️⃣ Return the mapped data.
        return brandsData;
    }

    /// <summary>
    ///     Asynchronously retrieves all product types from the repository
    ///     and maps them to <see cref="TypeDto"/> objects using AutoMapper.
    /// </summary>
    /// <returns>
    ///     A task representing the asynchronous operation.
    ///     The task result contains an <see cref="IEnumerable{TypeDto}"/> collection
    ///     of all product types available in the database.
    /// </returns>
    public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
    {
        // ─────────────────────────────────────────────
        // 🧾 (Alternative Approach - Split into Steps)
        // ---------------------------------------------
        // This approach separates repository creation and data retrieval:
        // var repo = await _unitOfWork.CreateRepositoryAsync<ProductType, int>();
        // var types = await repo.GetAllAsync();
        // var typesData = _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(types);
        // return typesData;
        // ─────────────────────────────────────────────

        // ✅ (Optimized One-Line Version)
        // Combines repository creation and data retrieval in one line for brevity.
        var repoAndData = await (await _unitOfWork.CreateRepositoryAsync<ProductType, int>()).GetAllAsync();

        // 2️⃣ Map the retrieved ProductType entities to TypeDto using AutoMapper.
        var typesData = _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(repoAndData);

        // 3️⃣ Return the mapped collection of product types.
        return typesData;
    }
}