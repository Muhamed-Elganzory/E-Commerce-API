# 🧩 Specification Pattern — Simple Guide

## 🔍 What is the Specification Pattern?
The **Specification Pattern** is used to **encapsulate query logic** (filters, includes, sorting, etc.) in reusable objects.  
Instead of writing LINQ queries directly inside repositories or services, we define *specifications* that describe **how data should be queried**.

This keeps the **repository clean**, improves **code reusability**, and makes **query logic testable and maintainable**.

---

## ⚙️ How It Works (Step-by-Step)

### 1️⃣ Define the Base Specification Interface
The `IBaseSpecification<TEntity, TKey>` interface defines the **structure** for a specification:
- A `Criteria` expression (filter condition)
- A collection of `Include` expressions (for eager loading related entities)

This allows us to describe queries *without executing them immediately.*

---

### 2️⃣ Create the BaseSpecification Class
File: `BaseSpecification.cs`

This abstract class implements `ISpecification` and provides:
- A constructor to set the **filtering logic** (`Criteria`)
- A protected method `AddInclude()` to define **related entities** that should be eagerly loaded

```csharp
protected BaseSpecification(Expression<Func<TEntity, bool>>? criteriaExpression)
{
    Criteria = criteriaExpression;
}

public void AddInclude(Expression<Func<TEntity, object>> includeExpression)
{
    IncludeExpression.Add(includeExpression);
}
```

💡 Why `AddInclude()`?
> It hides the internal list and keeps includes well-encapsulated.  
> Child classes can simply call `AddInclude()` to add navigation properties.

---

### 3️⃣ Implement the SpecificationEvaluator
File: `SpecificationEvaluator.cs`

This helper class **converts a specification into a real LINQ query**.

It:
- Applies the filtering (`WHERE`)
- Applies the includes (`INCLUDE`)

```csharp
public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(
    IQueryable<TEntity> inputQuery,
    ISpecification<TEntity, TKey> specification)
{
    var query = inputQuery;

    if (specification.Criteria is not null)
        query = query.Where(specification.Criteria);

    if (specification.IncludeExpression.Any())
        query = specification.IncludeExpression.Aggregate(
            query, (current, include) => current.Include(include));

    return query;
}
```

💡 Think of it as the **engine** that translates the specification into a query.

---

### 4️⃣ Extend BaseSpecification for Your Entity
File: `ProductWithBrandAndTypeSpecifications.cs`

You now create **custom specifications** for your domain entities by inheriting from `BaseSpecification`.

Example — a specification for `Product` that includes `ProductBrand` and `ProductType`:

```csharp
public class ProductWithBrandAndTypeSpecifications
    : BaseSpecification<Product, int>
{
    // Get all products with their related brand and type
    public ProductWithBrandAndTypeSpecifications()
        : base(null)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }

    // Get a single product by ID with includes
    public ProductWithBrandAndTypeSpecifications(int id)
        : base(p => p.Id == id)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);
    }
}
```

---

### 5️⃣ Use the Specification in the Repository
File: `GenericRepository.cs`

Instead of manually writing LINQ filters, you pass the specification to the repository methods:

```csharp
public async Task<List<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> spec)
{
    return await SpecificationEvaluator
        .CreateQuery(_dbContext.Set<TEntity>(), spec)
        .ToListAsync();
}

public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> spec)
{
    return await SpecificationEvaluator
        .CreateQuery(_dbContext.Set<TEntity>(), spec)
        .FirstOrDefaultAsync();
}
```

💡 This makes the repository completely **generic** and independent of entity-specific logic.

---

### 6️⃣ Consume It in the Service Layer
File: `ProductService.cs`

Now your service just picks the right specification and calls the repository — clean and readable!

```csharp
var specification = new ProductWithBrandAndTypeSpecifications();
var products = await (await _unitOfWork
    .CreateRepositoryAsync<Product, int>())
    .GetAllAsync(specification);

var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
```

---

## ✅ Advantages

| Benefit | Description |
|----------|-------------|
| 🧠 **Reusable Queries** | Write once, use across services and repositories |
| 🧱 **Separation of Concerns** | Keeps query logic out of repositories |
| 🚀 **Performance** | Allows fine control over eager loading |
| 🧪 **Testable** | You can easily test specifications independently |
| 🔄 **Consistent Behavior** | All repositories use the same evaluation logic |

---

## 🧩 Summary Diagram

```
Controller
   ↓
Service (ProductService)
   ↓
Specification (ProductWithBrandAndTypeSpecifications)
   ↓
SpecificationEvaluator (Applies Includes + Criteria)
   ↓
GenericRepository (Executes Query)
   ↓
EF Core / Database
```

---

## 📚 Example Query (Behind the Scenes)

```csharp
_dbContext.Products
    .Where(p => p.Id == id)
    .Include(p => p.ProductBrand)
    .Include(p => p.ProductType);
```
