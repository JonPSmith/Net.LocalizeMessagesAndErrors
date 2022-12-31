using LocalizeMessagesAndErrors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LocalizedWebApp.Controllers;

public class DefaultLocalizerController : Controller
{
    private readonly IDefaultLocalizer<HomeController> _defaultLocalizer;

    public DefaultLocalizerController(
        IDefaultLocalizer<HomeController> defaultLocalizer)
    {
        _defaultLocalizer = defaultLocalizer;
    }

    public IActionResult Index()
    {
        var nameOfService = nameof(IStringLocalizer);
        var cultureName = Thread.CurrentThread.CurrentUICulture.Name;

        return View((object)_defaultLocalizer.LocalizeFormattedMessage(
            "ExampleMessage".MethodLocalizeKey(this),
            $"Localized via {nameOfService} service with culture '{cultureName}' on {DateTime.Now:M}." //Message, using FormattableString
        ));
    }

    public IActionResult StringMessage()
    {
        return View((object)_defaultLocalizer.LocalizeStringMessage(
            "ExampleMessage".MethodLocalizeKey(this),
            "Hello from me!" //static Message, using string
        ));
    }

    public IActionResult MissingResourceEntry()
    {
        return View((object)_defaultLocalizer.LocalizeFormattedMessage(
            "MissingEntry".MethodLocalizeKey(this),
            $"LocalizeStringMessage: I forget to set up the resource entry. Time: {DateTime.Now:T}"
        ));
    }
}