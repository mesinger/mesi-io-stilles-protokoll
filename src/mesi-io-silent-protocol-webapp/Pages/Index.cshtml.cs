using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mesi.Io.SilentProtocol.Application;
using Mesi.Io.SilentProtocol.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Mesi.Io.SilentProtocol.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGetSilentProtocolEntriesPaged _getSilentProtocolEntriesPaged;

        public IndexModel(IGetSilentProtocolEntriesPaged getSilentProtocolEntriesPaged)
        {
            _getSilentProtocolEntriesPaged = getSilentProtocolEntriesPaged;
        }

        public IEnumerable<SilentProtocolEntryViewModel> Entries { get; private set; } =
            Enumerable.Empty<SilentProtocolEntryViewModel>();
        public int PageNumber { get; private set; }
        public bool ShowNextButton { get; set; }

        public async Task<IActionResult> OnGet(int p = 1)
        {
            if (p < 1)
            {
                p = 1;
            }

            PageNumber = p;
            
            var entriesResponse = await _getSilentProtocolEntriesPaged.GetPaged(p);
            
            Entries = from entry in entriesResponse.Entries select entry.ToViewModel();
            ShowNextButton = entriesResponse.HasMoreEntries;

            return Page();
        }
    }
}