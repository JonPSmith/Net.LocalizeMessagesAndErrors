using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LocalizedWebApp.Controllers
{
    public class StringLocalizerController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;

        public StringLocalizerController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View((object)_localizer["Index_ExampleMessage", 
                nameof(IStringLocalizer),
                Thread.CurrentThread.CurrentUICulture.Name].Value);
        }
    }
}
