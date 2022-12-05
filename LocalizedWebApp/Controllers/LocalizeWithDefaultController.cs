using LocalizeMessagesAndErrors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LocalizedWebApp.Controllers;

public class LocalizeWithDefaultController : Controller
{
    private readonly ILocalizeWithDefault<HomeController> _localizer;

    public LocalizeWithDefaultController(ILocalizeWithDefault<HomeController> localizer)
    {
        _localizer = localizer;
    }

    public IActionResult Index()
    {
        var nameOfService = nameof(IStringLocalizer);
        var cultureName = Thread.CurrentThread.CurrentUICulture.Name;

        return View((object)_localizer.LocalizeFormattedMessage(
            "ExampleMessage".MethodLocalizeKey(this),     //This creates a localizeKey of "Index_ExampleMessage"
            "en",                                      //This defines the culture of the default message
            $"Localized via {nameOfService} service with culture '{cultureName}' on {DateTime.Now:M}." //Message, using FormattableString
        ));
    }

    public IActionResult StringMessage()
    {
        return View((object)_localizer.LocalizeStringMessage(
            "ExampleMessage".MethodLocalizeKey(this),     //This creates a localizeKey of "StringMessage_ExampleMessage"
            "en",                                      //This defines the culture of the default message
            "Hello from me!" //static Message, using string
        ));
    }

    public IActionResult MissingResourceEntry()
    {
        return View((object)_localizer.LocalizeFormattedMessage(
            "MissingEntry".MethodLocalizeKey(this),
            "en",
            $"LocalizeStringMessage: I forget to set up the resource entry. Time: {DateTime.Now:T}"
        ));
    }
}