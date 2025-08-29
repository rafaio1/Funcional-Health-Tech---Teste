using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;

namespace FHT.Api.Middleware
{
    public class BasicGateAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var config = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));
            var expectedPrefix = config?["Auth"] ?? "Auth";

            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authValues))
            {
                context.Result = new UnauthorizedResult(); 
                return;
            }

            var auth = authValues.ToString();
            if (!auth.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new UnauthorizedResult(); 
                return;
            }

            var b64 = auth.Substring("Basic ".Length).Trim();
            string decoded;
            try
            {
                var bytes = Convert.FromBase64String(b64);
                decoded = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                context.Result = new UnprocessableEntityResult(); 
                return;
            }

            var parts = decoded.Split(':');
            if (parts.Length != 3)
            {
                context.Result = new UnprocessableEntityResult(); 
                return;
            }

            var prefix = parts[0];
            var yyyymmdd = parts[1];
            var third = parts[2];

            var todayUtc = DateTime.UtcNow.ToString("yyyyMMdd");

            if (string.Equals(prefix, expectedPrefix, StringComparison.Ordinal) &&
                string.Equals(yyyymmdd, todayUtc, StringComparison.Ordinal) &&
                string.Equals(third, "FHT", StringComparison.OrdinalIgnoreCase))
            {
                return; 
            }

            context.Result = new UnauthorizedResult(); 
        }
    }
}
