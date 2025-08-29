using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Security.Claims;

namespace FHT.Infra.Data.Authorization
{
    public class AspNetUser : IAspNetUser
    {
        private readonly IHttpContextAccessor _accessor;
        private const string CorrelationHeader = "X-Correlation-Id";
        private const string XForwardedFor = "X-Forwarded-For";
        private const string XRealIp = "X-Real-IP";

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        private HttpContext Http => _accessor.HttpContext;

        public string Name => Http?.User?.Identity?.Name ?? string.Empty;

        public string GetToken()
        {
            if (!IsAutenticated()) return string.Empty;
            var raw = Http?.Request?.Headers[HeaderNames.Authorization].ToString();
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;
            return raw.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? raw["Bearer ".Length..].Trim()
                : raw.Trim();
        }

        public bool IsAutenticated() => Http?.User?.Identity?.IsAuthenticated == true;

        public bool IsInRole(string role) => Http?.User?.IsInRole(role) == true;

        public HttpContext GetHttpContext() => Http;

        public long GetUserId()
        {
            var idClaim = Http?.User?.FindFirst(ClaimTypes.NameIdentifier)
                         ?? Http?.User?.FindFirst("sub")
                         ?? Http?.User?.FindFirst("uid")
                         ?? Http?.User?.FindFirst("user_id")
                         ?? Http?.User?.FindFirst("userid");

            if (idClaim == null || string.IsNullOrWhiteSpace(idClaim.Value))
                return 0;

            return long.TryParse(idClaim.Value, out var id) ? id : 0;
        }

        public string GetCorrelationId()
        {
            var h = Http?.Request?.Headers[CorrelationHeader].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(h)) return h!.Trim();
            return Http?.TraceIdentifier ?? string.Empty;
        }

        public string GetUserAgent()
        {
            return Http?.Request?.Headers[HeaderNames.UserAgent].ToString() ?? string.Empty;
        }

        public string GetIpAddress()
        {
            var xff = Http?.Request?.Headers[XForwardedFor].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(xff))
            {
                var first = xff!.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(first)) return first!;
            }

            var xrip = Http?.Request?.Headers[XRealIp].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(xrip)) return xrip!;

            var ip = Http?.Connection?.RemoteIpAddress?.ToString();
            return ip ?? string.Empty;
        }
    }

    internal static class ClaimsPrincipalExtensions
    {
        public static Claim FindFirst(this ClaimsPrincipal principal, string type)
            => principal?.Claims?.FirstOrDefault(c => string.Equals(c.Type, type, StringComparison.OrdinalIgnoreCase));
    }
}
