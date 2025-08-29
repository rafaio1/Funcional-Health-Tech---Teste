using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FHT.Api.Middleware
{
    /// <summary>
    /// Mantém/gera X-Correlation-Id
    /// </summary>
    public class CorrelationIdMiddleware
    {
        public const string HeaderName = "X-Correlation-Id";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext ctx)
        {
            var incoming = ctx.Request.Headers[HeaderName].ToString();

            var correlationId = string.IsNullOrWhiteSpace(incoming)
                ? Guid.NewGuid().ToString("N")
                : incoming.Trim();

            ctx.Items[HeaderName] = correlationId;

            ctx.Request.Headers[HeaderName] = correlationId;

            ctx.Response.Headers[HeaderName] = correlationId;

            await _next(ctx);
        }
    }
}
