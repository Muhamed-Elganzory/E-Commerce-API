
# Redis Caching Implementation Explanation

---

## ICashRepository Interface

```csharp
namespace serviceAbstraction.Contracts.Redis;

/// <summary>
/// Interface for basic cache repository operations.
/// </summary>
public interface ICashRepository
{
    /// <summary>
    /// Saves a string value to cache with a specific key and time to live.
    /// </summary>
    /// <param name="key">Cache key identifier.</param>
    /// <param name="value">String value to cache.</param>
    /// <param name="timeToLive">Duration to keep the cache.</param>
    /// <returns>A Task representing the async operation.</returns>
    public Task SetAsync(string key, string value, TimeSpan timeToLive);

    /// <summary>
    /// Retrieves a cached string value by its key.
    /// </summary>
    /// <param name="cashKey">The cache key to retrieve value for.</param>
    /// <returns>The cached string if exists, otherwise null.</returns>
    public Task<string?> GetAsync(string cashKey);
}
```

---

## CashRepository Implementation

```csharp
using DomainLayer.Contracts.Repository.Redis;
using StackExchange.Redis;

namespace Persistence.Repository.Redis;

/// <summary>
/// Concrete implementation of ICashRepository using StackExchange.Redis.
/// </summary>
/// <param name="connectionMultiplexer">Redis connection multiplexer instance.</param>
public class CashRepository (IConnectionMultiplexer connectionMultiplexer): ICashRepository
{
    private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    public async Task SetAsync(string cashKey, string cashValue, TimeSpan timeToLive)
    {
        // Save value with expiration to Redis database
        await _database.StringSetAsync(cashKey, cashValue, timeToLive);
    }

    public async Task<string?> GetAsync(string cashKey)
    {
        // Retrieve cached value by key
        var cashingValues = await _database.StringGetAsync(cashKey);
        return cashingValues.HasValue ? cashingValues.ToString() : null;
    }
}
```

---

## ICashService Interface

```csharp
namespace serviceAbstraction.Contracts.Redis;

/// <summary>
/// Service interface for cache operations handling object serialization.
/// </summary>
public interface ICashService
{
    /// <summary>
    /// Stores an object in cache with a specified key and TTL.
    /// </summary>
    /// <param name="cashKey">Cache key.</param>
    /// <param name="cashValue">Object to serialize and cache.</param>
    /// <param name="timeToLive">Cache duration.</param>
    /// <returns>Async Task.</returns>
    public Task SetAsync(string cashKey, object cashValue, TimeSpan timeToLive);

    /// <summary>
    /// Retrieves a cached string value by key.
    /// </summary>
    /// <param name="cashKey">Cache key.</param>
    /// <returns>Cached string or null.</returns>
    public Task<string?> GetAsync(string cashKey);
}
```

---

## CashService Implementation

```csharp
using System.Text.Json;
using DomainLayer.Contracts.Repository.Redis;
using serviceAbstraction.Contracts.Redis;

namespace Service.Redis;

/// <summary>
/// Service implementation that serializes objects and delegates caching to repository.
/// </summary>
/// <param name="cashRepository">Cache repository dependency.</param>
public class CashService (ICashRepository cashRepository): ICashService
{
    private readonly ICashRepository _cashRepository = cashRepository;

    public async Task SetAsync(string cashKey, object cashValue, TimeSpan timeToLive)
    {
        // Serialize object to JSON string before caching
        var valueSerialized = JsonSerializer.Serialize(cashValue);
        await _cashRepository.SetAsync(cashKey, valueSerialized, timeToLive);
    }

    public async Task<string?> GetAsync(string cashKey)
    {
        // Get raw cached string
        return await _cashRepository.GetAsync(cashKey);
    }
}
```

---

## Dependency Injection Registration

```csharp
services.AddScoped<ICashService, CashService>();
services.AddScoped<ICashRepository, CashRepository>();
```

---

## Cash Attribute for Response Caching

```csharp
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using serviceAbstraction.Contracts.Redis;

namespace Presentation.Controllers.Attributes;

/// <summary>
/// Action filter attribute to cache API responses for specified duration.
/// </summary>
/// <param name="durationInSecond">Cache duration in seconds (default 90s).</param>
public class Cash (int durationInSecond = 90): ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 1- Create cache key based on request path and query parameters
        string? cashKey = CreateCashKey(context.HttpContext.Request);

        // Retrieve cache service from DI
        ICashService cashService = context.HttpContext.RequestServices.GetRequiredService<ICashService>();

        // Check if response is cached
        var cashValue = await cashService.GetAsync(cashKey);

        if (cashValue is not null)
        {
            // Return cached response directly
            context.Result = new ContentResult()
            {
                Content = cashValue,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
            return;
        };

        // Proceed to action execution
        var executedContext = await next.Invoke();

        // Cache the response if it is OK and has a value
        if (executedContext.Result is OkObjectResult okObjectResult && okObjectResult.Value != null)
        {
            await cashService.SetAsync(cashKey, okObjectResult.Value, TimeSpan.FromSeconds(durationInSecond));
        }
    }

    /// <summary>
    /// Creates a unique cache key string based on HTTP request path and ordered query parameters.
    /// </summary>
    /// <param name="request">HttpRequest object.</param>
    /// <returns>Cache key string.</returns>
    private static string CreateCashKey(HttpRequest request)
    {
        // Using StringBuilder for efficient string concatenation
        StringBuilder keyBuilder = new StringBuilder();

        // Append request path and question mark
        keyBuilder.Append(request.Path + "?");

        // Sort query parameters alphabetically for consistent keys
        foreach (var item in request.Query.OrderBy(o => o.Key))
        {
            // Append each query key-value pair
            keyBuilder.Append($"{item.Key}={item.Value}&");
        }

        return keyBuilder.ToString();
    }
}
```

---

# Explanation of Key Concepts

### `next.Invoke()` in Action Filter
- This calls the next delegate/middleware in the pipeline (usually the actual controller action).
- It executes the action and returns the result (`executedContext`) for further processing.

### Caching the Response
- After the action executes and returns a successful result (`OkObjectResult`), we serialize and store the result in Redis.
- This way, subsequent identical requests can serve cached data, reducing load and latency.

### Why Use `StringBuilder` in `CreateCashKey`
- Efficient string concatenation compared to string `+` operator in loops.
- Helps in building cache keys dynamically from request data.

### Why Use `OrderBy` on Query Parameters
- Ensures the cache key is consistent regardless of query parameter order.
- Prevents cache misses due to parameter ordering differences.

---

# Summary

This implementation uses:

- **Repository pattern** (`ICashRepository`) for low-level Redis operations.
- **Service layer** (`ICashService`) to handle serialization and business logic.
- **Action Filter attribute** (`Cash`) for automatic response caching on API controllers.
- **Dependency Injection** to register and inject dependencies properly.

The approach improves API performance by caching frequent identical responses and reduces unnecessary database or service calls.

---
