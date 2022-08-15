using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Mesi.Io.SilentProtocol.WebApp.Extensions;

public static class HttpContextExtensions
{
    public static string? GetUserName(this HttpContext? context)
    {
        var user = context?.User;
        return user?.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
    }
}