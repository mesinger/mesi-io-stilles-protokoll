using Microsoft.AspNetCore.Mvc;

namespace Mesi.Io.SilentProtocol.WebApp.Controllers;

[Route("")]
public class AuthenticationController : Controller
{
    [HttpGet]
    [Route("logout")]
    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }
}