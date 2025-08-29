using Microsoft.AspNetCore.Http;

namespace FHT.Infra.Data.Authorization
{
    public interface IAspNetUser
    {
        string Name { get; }
        string GetToken();
        bool IsAutenticated();
        bool IsInRole(string role);
        HttpContext GetHttpContext();
        long GetUserId();
        string GetCorrelationId();
        string GetUserAgent();
        string GetIpAddress();
    }
}
