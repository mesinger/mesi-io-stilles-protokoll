using System.Linq;
using System.Security.Claims;
using Mesi.Io.SilentProtocol.Application;
using Microsoft.AspNetCore.Http;

namespace Mesi.Io.SilentProtocol.WebApp;

public class RequestContext : IRequestContext
{
    private readonly IHttpContextAccessor _httpContext;

    public RequestContext(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }
    
    public User User()
    {
        var claims = _httpContext.HttpContext?.User.Claims;
        var id = claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var name = claims?.FirstOrDefault(claim => claim.Type == "name")?.Value;
        return new User(id ?? "anonymous", name ?? "anonymous");
    }
}