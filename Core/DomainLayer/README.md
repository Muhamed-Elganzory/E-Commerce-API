# 🧩 Domain Layer

## 📖 Overview
The **Domain Layer** represents the **core business logic** of the application.  
It defines **what the system does**, not **how it does it**.

This layer contains all the **entities**, **interfaces (contracts)**, and **exceptions** that describe the problem domain and its rules.  
It should be **completely independent** of frameworks, databases, and other infrastructure layers — meaning the Domain Layer never depends on external technologies, but they depend on it.

---

## 📂 Folder Structure

### 🗂️ 1. Contracts
- Contains **interfaces and abstractions** that define the expected behavior of repositories, services, or any domain-related component.
- These interfaces are later implemented in other layers such as **Persistence** or **Application**.
- ✅ Example:
  ```csharp
    public interface IGenericRepository<TEntity, TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TKey id);
    }
  ```

### 🗂️ 2. Exceptions

Contains custom exceptions that are specific to the domain logic.

These exceptions are used to handle domain-related errors in a clear and meaningful way.

- ✅ Example:
    ```csharp
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(int id)
        : base($"Product with ID {id} was not found.")
        {
        }
    }
    ```

### 🗂️ 3. Models

Contains domain entities that represent the main business objects in the system, like Product, ProductType, and ProductBrand.

Each entity defines its properties, relationships, and business rules.

- ✅ Example:
    ```csharp
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int ProductTypeId { get; set; }
        public int ProductBrandId { get; set; }
    }
    ```