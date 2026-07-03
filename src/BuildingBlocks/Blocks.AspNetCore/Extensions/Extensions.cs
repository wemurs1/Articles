using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Blocks.AspNetCore;

public static class Extensions
{
    public static string? BaseUrl(this HttpRequest request)
    {
        if (request == null) return null;
        var uriBuilder = new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port ?? -1);
        if (uriBuilder.Uri.IsDefaultPort) uriBuilder.Port = -1;
        return uriBuilder.Uri.AbsoluteUri;
    }

    public static string GetClientIPAddress(this HttpContext context)
    {
        var forwardedFor = context.Request.Headers["X-forwarded-for"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwardedFor)) return forwardedFor.Split(',')[0].Trim();

        return context.Connection.RemoteIpAddress!.ToString() ?? "Unknown";
    }

    public static string? GetClaimValue(this IHttpContextAccessor httpContextAccessor, string claimName)
    {
        var user = httpContextAccessor.HttpContext?.User;
        return user?.FindFirstValue(claimName);
    }
}
