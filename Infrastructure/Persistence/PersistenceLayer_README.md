# 🏗️ Persistence Layer

## 📖 Overview
The **Persistence Layer** handles all database operations.  
It implements the repository interfaces defined in the **Domain Layer** and provides real database access using **Entity Framework Core (EF Core)**.

---

## 🗄️ Database

### 🧩 DbContext
The `ApplicationDbContext` class represents the database session.  
It manages entity sets and is responsible for querying and saving data.

✅ Example:
```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductType> ProductTypes => Set<ProductType>();
    public DbSet<ProductBrand> ProductBrands => Set<ProductBrand>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
```

### 🗃️ Connection String
The connection string is defined in `appsettings.json`:
```json
"ConnectionStrings":{
  "DefaultConnection": "Server=.;Database=MyAppDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

To apply migrations:
```bash
dotnet ef migrations add InitialCreate -p Persistence -s API
dotnet ef database update -p Persistence -s API
```

---

## 🧩 Repository

Repositories encapsulate database logic and make data access independent from business logic.

### 🧱 Generic Repository
Provides reusable CRUD methods for all entities.

✅ Example:
```csharp
public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    protected readonly ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
        => await _context.Set<TEntity>().FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _context.Set<TEntity>().ToListAsync();

    public async Task AddAsync(TEntity entity)
        => await _context.Set<TEntity>().AddAsync(entity);

    public async Task UpdateAsync(TEntity entity)
        => _context.Set<TEntity>().Update(entity);

    public async Task DeleteAsync(TKey id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
            _context.Set<TEntity>().Remove(entity);
    }
}
```

### 🧩 Specific Repository Example
Implements additional logic specific to an entity.

✅ Example:
```csharp
public class ProductRepository : GenericRepository<Product, int>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetProductsWithDetailsAsync()
    {
        return await _context.Products
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductType)
            .ToListAsync();
    }
}
```

---

## 🧠 Summary
- **DbContext** connects EF Core to the database.
- **GenericRepository** handles CRUD operations for all entities.
- **Specific Repositories** extend functionality for particular entities.
- The Persistence Layer ensures all data access follows a clean, testable structure.
