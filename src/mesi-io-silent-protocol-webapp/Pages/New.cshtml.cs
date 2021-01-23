using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Mesi.Io.SilentProtocol.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mesi.Io.SilentProtocol.WebApp.Pages
{
    [BindProperties]
    public class NewModel : PageModel
    {
        private readonly IAddSilentProtocolEntry _addSilentProtocolEntry;

        public NewModel(IAddSilentProtocolEntry addSilentProtocolEntry)
        {
            _addSilentProtocolEntry = addSilentProtocolEntry;
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

            var createdEntry = await _addSilentProtocolEntry.Add(new(Suspect, Entry, TimeStamp));

            if (createdEntry == null)
            {
                ModelState.AddModelError(string.Empty, "Ein unbekannter Fehler ist aufgetreten.");
                return Page();
            }
            
            return RedirectToPage("/New", new { a = true });
        }
    }
}