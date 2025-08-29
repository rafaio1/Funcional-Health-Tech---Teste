using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FHT.Api.Middleware
{
    /// <summary>
    /// 150 req/min por IP + rota. Retorna 429 quando estoura.
    /// Adiciona: X-RateLimit-Limit, X-RateLimit-Remaining, Retry-After.
    /// </summary>
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;

        private const int LIMIT = 150;
        private static readonly TimeSpan WINDOW = TimeSpan.FromMinutes(1);

        public RateLimitMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            if (ctx.Request.Path.StartsWithSegments("/swagger") &&
                ctx.Request.Path.Value?.EndsWith("swagger.json", StringComparison.OrdinalIgnoreCase) == true)
            {
                await _next(ctx);
                return;
            }

            var ip = GetClientIp(ctx);
            var route = ctx.Request.Path.Value?.ToLowerInvariant() ?? "/";
            var key = $"rl:{ip}:{route}";

            var entry = _cache.GetOrCreate(key, e =>
            {
                e.AbsoluteExpirationRelativeToNow = WINDOW;
                return new Counter { Count = 0, ResetAt = DateTimeOffset.UtcNow.Add(WINDOW) };
            });

            entry.Count++;

            var remaining = Math.Max(0, LIMIT - entry.Count);
            ctx.Response.Headers["X-RateLimit-Limit"] = LIMIT.ToString();
            ctx.Response.Headers["X-RateLimit-Remaining"] = remaining.ToString();

            if (entry.Count > LIMIT)
            {
                var retryAfter = (int)Math.Max(1, (entry.ResetAt - DateTimeOffset.UtcNow).TotalSeconds);
                ctx.Response.Headers["Retry-After"] = retryAfter.ToString();
                ctx.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await ctx.Response.WriteAsync("Too many requests.");
                return;
            }

            _cache.Set(key, entry, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = entry.ResetAt
            });

            await _next(ctx);
        }

        private static string GetClientIp(HttpContext ctx)
        {
            var forwarded = ctx.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(forwarded))
                return forwarded.Split(',')[0].Trim();

            return ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private sealed class Counter
        {
            public int Count { get; set; }
            public DateTimeOffset ResetAt { get; set; }
        }
    }
}
