using LocalizeMessagesAndErrors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LocalizedWebApp.Controllers
{
    public class LocalizeWithDefaultController : Controller
    {
        private readonly ILocalizeWithDefault<HomeController> _localizer;

        public LocalizeWithDefaultController(ILocalizeWithDefault<HomeController> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View((object)_localizer.LocalizeFormattedMessage(
                "ExampleMessage".MethodLocalizeKey(this),
                "en",
                $"Localized via {nameof(IStringLocalizer)} with culture '{Thread.CurrentThread.CurrentUICulture.Name}'."
            ));
        }
    }
}
