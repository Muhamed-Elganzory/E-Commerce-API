using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using serviceAbstraction.Contracts.Redis;

namespace Presentation.Controllers.Attributes;

/// <summary>
///     Caching attribute used to cache API responses at the action level.
///     <para>
///         When applied on an action method (GET usually), the filter checks whether
///         a cached response already exists for the incoming request.
///     </para>
///     <para>
///         If a cached version exists, the filter **returns it immediately** without executing the action.
///         Otherwise, it executes the action, captures the result, and caches it for future requests.
///     </para>
/// </summary>
/// <param name="durationInSecond">The time (in seconds) the response should stay cached in Redis.</param>
public class Cash(int durationInSecond = 90) : ActionFilterAttribute
{
    /// <summary>
    ///     Intercepts the request before and after the action executes to apply caching logic.
    /// </summary>
    /// <param name="context">Context containing HTTP request data.</param>
    /// <param name="next">Delegate used to execute the next filter or the action itself.</param>
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 1️⃣ Create a unique cache key representing this request (URL + query string)
        var cashKey = CreateCashKey(context.HttpContext.Request);

        // 2️⃣ Resolve caching service (ICashService) from the DI container
        var cashService = context.HttpContext.RequestServices.GetRequiredService<ICashService>();

        // 3️⃣ Try to get a cached response for this key
        var cashValue = await cashService.GetAsync(cashKey);

        // 4️⃣ If cached value exists → return it immediately (skip executing the action)
        if (cashValue is not null)
        {
            context.Result = new ContentResult()
            {
                Content = cashValue,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };

            return;
        }

        // 5️⃣ Execute the action if cache missed
        // ❗ explain line: next.Invoke() actually RUNS the controller action and returns its result
        var executedContext = await next.Invoke();

        // 6️⃣ Cache the action result (ONLY if action returned 200 OK AND has a non-null value)
        // ❗ explain line: We read OkObjectResult.Value because it contains the actual JSON body
        if (executedContext.Result is OkObjectResult okObjectResult && okObjectResult.Value != null)
        {
            await cashService.SetAsync(
                cashKey,
                okObjectResult.Value,
                TimeSpan.FromSeconds(durationInSecond)
            );
        }
    }

    /// <summary>
    ///     Creates a unique cache key based on the request path and ordered query string parameters.
    ///     <para>Example: baseUrl/api/products?brandId=3?typeid=1 → baseUrl/api/products?typeId=1?brandId=3</para>
    /// </summary>
    /// <param name="request">The incoming HTTP request.</param>
    /// <returns>The generated cache key.</returns>
    private static string CreateCashKey(HttpRequest request)
    {
        // ✔ Why StringBuilder?
        // Because repeatedly concatenating strings is slow.
        // StringBuilder is optimized for constructing dynamic text.
        StringBuilder keyBuilder = new StringBuilder();

        keyBuilder.Append(request.Path + "?");

        // ✔ Why OrderBy?
        // To avoid different query order producing different cache keys:
        // /api/products?typeId=1&brandId=3
        // /api/products?brandId=3&typeId=1
        // Both should produce SAME key → ensures cache HIT.
        foreach (var item in request.Query.OrderBy(o => o.Key))
        {
            keyBuilder.Append($"{item.Key}={item.Value}&");
        }

        return keyBuilder.ToString();
    }
}