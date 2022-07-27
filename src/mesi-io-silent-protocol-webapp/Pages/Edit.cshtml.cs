using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Mesi.Io.SilentProtocol.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mesi.Io.SilentProtocol.WebApp.Pages;

public class EditModel : PageModel
{
    private readonly ISilentProtocolEntryRepository _entryRepository;

    public EditModel(ISilentProtocolEntryRepository entryRepository)
    {
        _entryRepository = entryRepository;
    }
    
    private SilentProtocolEntry? _protocolEntry;

    public async Task<IActionResult> OnGet(string id)
    {
        _protocolEntry = await _entryRepository.GetById(id);

        if (_protocolEntry is null)
        {
            return NotFound();
        }

        Id = id;
        Suspect = _protocolEntry.Suspect;
        Entry = _protocolEntry.Entry;
        TimeStamp = _protocolEntry.TimeStamp;

        return Page();
    }

    [Required(ErrorMessage = "Bitte gib einen Schuldigen ein.")]
    [MaxLength(50, ErrorMessage = "Der Name des Beschuldigten darf max. 50 Zeichen lang sein.")]
    [MinLength(3, ErrorMessage = "Der Name des Beschuldigten muss min. 3 Zeichen lang sein.")]
    [BindProperty]
    public string Suspect { get; set; } = null!;

    [Required(ErrorMessage = "Bitte gib einen Eintrag ein.")]
    [MaxLength(1800, ErrorMessage = "Der Eintrag ist zu lang.")]
    [MinLength(3, ErrorMessage = "Der Eintrag ist zu kurz.")]
    [BindProperty]
    public string Entry { get; set; } = null!;

    [Required(ErrorMessage = "Bitte gib eine Beschreibung des Zeitpunkts ein.")]
    [MaxLength(50, ErrorMessage = "Die Beschreibung des Zeitpunkts ist zu lang.")]
    [MinLength(3, ErrorMessage = "Die Beschreibung des Zeitpunkts ist zu kurz.")]
    [BindProperty]
    public string TimeStamp { get; set; } = null!;

    [Required]
    [HiddenInput]
    [BindProperty]
    public string Id { get; set; } = null!;

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existingEntry = await _entryRepository.GetById(Id);

        if (existingEntry is null)
        {
            return BadRequest();
        }

        await _entryRepository.Update(existingEntry with
        {
            Suspect = Suspect,
            Entry = Entry,
            TimeStamp = TimeStamp
        });

        return RedirectToPage("Index");
    }
}