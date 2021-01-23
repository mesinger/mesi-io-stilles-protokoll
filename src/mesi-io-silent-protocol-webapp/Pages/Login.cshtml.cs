using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Mesi.Io.SilentProtocol.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Mesi.Io.SilentProtocol.WebApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SilentProtocolOptions _options;
        
        public LoginModel(IOptions<SilentProtocolOptions> options)
        {
            _options = options.Value;
        }
        
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        [BindProperty] public string ReturnUrl { get; set; } = null!;

        [BindProperty]
        [Required(ErrorMessage = "Bitte gib das Passwort ein.")]
        [MinLength(3, ErrorMessage = "Das Passwort ist zu kurz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Password != _options.Password)
            {
                ModelState.AddModelError(nameof(Password), "Falsches Passwort");
                return Page();
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, "Protokollerweiterer")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return !string.IsNullOrWhiteSpace(ReturnUrl) 
                ? Redirect(ReturnUrl)
                : RedirectToPage("/Index");
        }
    }
}