using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Mesi.Io.SilentProtocol.Application;
using Mesi.Io.SilentProtocol.Options;
using Mesi.Io.SilentProtocol.WebApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Mesi.Io.SilentProtocol.WebApp.Pages
{
    [BindProperties]
    public class NewModel : PageModel
    {
        private readonly IAddSilentProtocolEntry _addSilentProtocolEntry;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DiscordOptions _discord;

        public NewModel(IAddSilentProtocolEntry addSilentProtocolEntry, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<DiscordOptions> discord)
        {
            _addSilentProtocolEntry = addSilentProtocolEntry;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _discord = discord.Value;
        }
        
        public void OnGet(bool a)
        {
            SuccessfullyAdded = a;
        }

        public bool SuccessfullyAdded { get; private set; } = false;

        [Required(ErrorMessage = "Bitte gib einen Schuldigen ein.")]
        [MaxLength(50, ErrorMessage = "Der Name des Beschuldigten darf max. 50 Zeichen lang sein.")]
        [MinLength(3, ErrorMessage = "Der Name des Beschuldigten muss min. 3 Zeichen lang sein.")]
        public string Suspect { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte gib einen Eintrag ein.")]
        [MaxLength(1800, ErrorMessage = "Der Eintrag ist zu lang.")]
        [MinLength(3, ErrorMessage = "Der Eintrag ist zu kurz.")]
        public string Entry { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte gib eine Beschreibung des Zeitpunkts ein.")]
        [MaxLength(50, ErrorMessage = "Die Beschreibung des Zeitpunkts ist zu lang.")]
        [MinLength(3, ErrorMessage = "Die Beschreibung des Zeitpunkts ist zu kurz.")]
        public string TimeStamp { get; set; } = string.Empty;

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var reporter = _httpContextAccessor.HttpContext.GetUserName();
            if (reporter is null)
            {
                return Unauthorized();
            }

            var createdEntry = await _addSilentProtocolEntry.Add(new(Suspect, Entry, TimeStamp, reporter));

            if (createdEntry == null)
            {
                ModelState.AddModelError(string.Empty, "Ein unbekannter Fehler ist aufgetreten.");
                return Page();
            }

            var nvc = new List<KeyValuePair<string, string>>
            {
                new("content", $"Neuer Eintrag für **{createdEntry.Suspect}**```{createdEntry.Entry}```")
            };

            var req = new HttpRequestMessage(HttpMethod.Post, $"https://discord.com/api/webhooks/{_discord.WebhookId}/{_discord.WebhookToken}") { Content = new FormUrlEncodedContent(nvc) };
            await _httpClient.SendAsync(req);
            
            return RedirectToPage("/New", new { a = true });
        }
    }
}