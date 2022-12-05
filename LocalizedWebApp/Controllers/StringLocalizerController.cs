using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LocalizedWebApp.Controllers;

public class StringLocalizerController : Controller
{
    private readonly IStringLocalizer<HomeController> _localizer;

    public StringLocalizerController(IStringLocalizer<HomeController> localizer)
    {
        _localizer = localizer;
    }

    public IActionResult Index()
    {
        var nameOfService = nameof(IStringLocalizer);
        var cultureName = Thread.CurrentThread.CurrentUICulture.Name;

        return View((object)_localizer["Index_ExampleMessage",
            nameOfService,
            cultureName,
            DateTime.Now].Value);
    }

    public IActionResult StringMessage()
    {
        return View((object)_localizer["StringMessage_ExampleMessage"].Value);
    }

    public IActionResult MissingResourceEntry()
    {
        return View((object)_localizer["MissingEntry_ExampleMessage", DateTime.Now].Value);
    }
}