# 🧱 E-Commerce Architecture Overview

This document explains the structure and flow between layers in the E-Commerce project.

---

## ① Database (DB Layer)
- The actual SQL Server database that stores all entities like **Products**, **Orders**, and **Users**.
- Acts as the data persistence layer.

---

## ② Generic Repository
- Handles **CRUD operations** (Create, Read, Update, Delete) for all entities.
- Interacts directly with the **DbContext**.
- Provides reusable generic methods such as:
  ```csharp
  GetAllAsync()
  FindByIdAsync()
  AddAsync()
  Update()
  Delete()
  ```
- Can be extended to specific repositories (e.g., `ProductRepository`) when custom queries or logic are needed.

---

## ③ Unit Of Work
- Centralizes all repositories in one place.
- Manages **transactions** across multiple repositories.
- Exposes repositories and provides a `SaveChangesAsync()` method.
  ```csharp
  await UnitOfWork.SaveChangesAsync();
  ```
- Example:
  ```csharp
  UnitOfWork.ProductRepository
  UnitOfWork.CategoryRepository
  ```

---

## ④ ProductService (Service Layer)
- Contains **business logic** and interacts with the **Unit Of Work** to access repositories.
- Example:
  ```csharp
  public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
  ```
- Decides how data should be processed before returning it to the controller.

---

## ⑤ ServiceManager
- Acts as a **centralized entry point** for all services.
- Instead of injecting each service individually, the controller injects only the `IServiceManager`.
- Contains references like:
  ```csharp
  public IProductService ProductService { get; }
  ```
- Handles **AutoMapper Mapping** between domain entities and DTOs:
    - Example: `Product → ProductDto`

---

## ⑥ Controller (Presentation Layer)
- The layer that handles **HTTP requests and responses**.
- Communicates only with the `ServiceManager` (not repositories directly).
- Example:
  ```csharp
  return Ok(await _serviceManager.ProductService.GetAllProductsAsync());
  ```

---

## 🔁 Overall Flow

```
DB
↑
Generic Repository  →  Like: ProductRepository
↑
Unit Of Work        →  Create Repository
↑
ProductService
↑
ServiceManager      →  Create Service (Like: ProductService)
↓
Mapping             →  Product → ProductDTO
↑
Controller          →  Like: ProductController
```

---

✅ **Summary**
- `Repository` → Data Access Logic
- `Unit Of Work` → Transaction & Repository Management
- `Service` → Business Logic
- `ServiceManager` → Service Aggregation
- `Controller` → HTTP Entry Point (API Layer)
