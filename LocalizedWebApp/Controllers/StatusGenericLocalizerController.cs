using GenericServices.AspNetCore;
using LocalizedWebApp.Models;
using LocalizeMessagesAndErrors;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;

namespace LocalizedWebApp.Controllers;

public class StatusGenericLocalizerController : Controller
{
    private readonly ExamplesOfStatusGenericsLoc<HomeController> _exampleMethods;

    public StatusGenericLocalizerController(ILocalizeWithDefault<HomeController> localizer)
    {
        _exampleMethods = new ExamplesOfStatusGenericsLoc<HomeController>(localizer);
    }

    public IActionResult Index(string? message)
    {
        if(message != null)
            ViewBag.Message = $"Message = {message}";
        return View();
    }

    public IActionResult CheckNull()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CheckNull(string? month)
    {
        var status = _exampleMethods.CheckNull(month);
        if (status.IsValid)
            return RedirectToAction(nameof(Index), new { message = status.Message });

        status.CopyErrorsToModelState(ModelState);
        return View();
    }

    public IActionResult CreateDate()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateDate(CreateDateDto dto)
    {
        var status = _exampleMethods.CreateDate(dto.Day, dto.Month, dto.Year);
        if (status.IsValid)
            return RedirectToAction(nameof(Index), new { message = status.Message });

        status.CopyErrorsToModelState(ModelState, dto);
        return View();
    }
}