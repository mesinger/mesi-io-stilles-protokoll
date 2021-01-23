using Microsoft.AspNetCore.Mvc;

namespace Mesi.Io.SilentProtocol.WebApp.ViewComponents
{
    public class NavigatorViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int page, bool showNextButton)
        {
            return View(new NavigatorViewComponentModel(page, page > 1, showNextButton));
        }
    }

    public record NavigatorViewComponentModel(int Page, bool ShowPrevButton, bool ShowNextButton);
}