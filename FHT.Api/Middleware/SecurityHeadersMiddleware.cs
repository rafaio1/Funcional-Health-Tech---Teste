using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FHT.Api.Middleware
{
    public sealed class SecurityHeadersOptions
    {
        public string HeaderName { get; init; } = "X-Application-ID";
        public bool EnforceAppId { get; init; } = true;
        public HashSet<string>? AllowedAppIds { get; init; } = null;
        public string[] SkipPaths { get; init; } = new[] { "/swagger", "/health", "/healthz" };
    }

    /// <summary>
    /// Aplica headers de segurança 
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SecurityHeadersOptions _opt;

        public SecurityHeadersMiddleware(RequestDelegate next, IOptions<SecurityHeadersOptions> opt)
        {
            _next = next;
            _opt = opt.Value ?? new SecurityHeadersOptions();
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            var h = ctx.Response.Headers;
            h["X-Content-Type-Options"] = "nosniff";
            h["X-Frame-Options"] = "DENY";
            h["Referrer-Policy"] = "strict-origin-when-cross-origin";
            h["X-Download-Options"] = "noopen";
            h["X-Permitted-Cross-Domain-Policies"] = "none";
            h["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
            h["Content-Security-Policy"] = "default-src 'self'";
            if (ctx.Request.IsHttps)
                h["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";

            if (_opt.SkipPaths.Any(p => ctx.Request.Path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(ctx);
                return;
            }

            if (_opt.EnforceAppId)
            {
                var appId = ctx.Request.Headers[_opt.HeaderName].FirstOrDefault();

                var vazio = string.IsNullOrWhiteSpace(appId);
                var naoPermitido = _opt.AllowedAppIds is { Count: > 0 } && !_opt.AllowedAppIds.Contains(appId!);

                if (vazio && naoPermitido)
                {
                    ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await ctx.Response.WriteAsync("Necessário e/ou inválido o X-Application-ID.");
                    return;
                }
                else if (vazio)
                {
                    h["X-Application-ID"] = Guid.NewGuid().ToString();
                }

                ctx.Items["AplicacaoId"] = appId!;
            }

            await _next(ctx);
        }
    }
}
