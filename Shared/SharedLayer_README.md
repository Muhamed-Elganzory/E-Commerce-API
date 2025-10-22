# 🧩 Shared Layer

## 📖 Overview
The **Shared Layer** contains all the **common classes** and **data structures** that are shared across multiple layers in the application.  
It ensures **consistency**, **reusability**, and **separation of concerns** between different parts of the system.

This layer is typically **independent** — it doesn’t depend on the **Domain**, **Persistence**, or **Application** layers.

---

## 📂 Folder Structure

### 🗂️ 1. DTOs (Data Transfer Objects)
DTOs are simple objects used to **transfer data** between layers (for example, between API and Application).  
They do not contain any business logic — only **properties** that represent the data structure needed by the client or another service.

✅ Example (Product DTO):
```csharp
public class ProductDto
{
    public int Id {get; set;}

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string PictureUrl { get; set; } = null!;

    public decimal Price { get; set; }

    public string BrandName { get; set; } = null!;

    public string TypeName { get; set; } = null!;
}
```

