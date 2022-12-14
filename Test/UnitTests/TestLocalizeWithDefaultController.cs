// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Globalization;
using System.Threading;
using LocalizedWebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Test.StubClasses;
using Xunit.Extensions.AssertExtensions;
using Xunit;

namespace Test.UnitTests;

public class TestLocalizeWithDefaultController
{
    //see https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/testing#unit-testing-controllers
    //for testing the return of a controller action

    [Fact]
    public void TestIndexAction()
    {
        //SETUP
        var logLocalizer = new StubLocalizeDefaultWithLogging<HomeController>();
        var controller = new LocalizeWithDefaultController(logLocalizer);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        //ATTEMPT
        var actionResult = controller.Index();

        //VERIFY
        var viewResult = Assert.IsType<ViewResult>(actionResult);
        var model = Assert.IsAssignableFrom<string>(viewResult.ViewData.Model);
        model.ShouldStartWith("Localized via IStringLocalizer service with culture 'en-GB' on");
    }

    [Fact]
    public void TestStringMessage()
    {
        //SETUP
        var logLocalizer = new StubLocalizeDefaultWithLogging<HomeController>();
        var controller = new LocalizeWithDefaultController(logLocalizer);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        //ATTEMPT
        var actionResult = controller.StringMessage();

        //VERIFY
        var viewResult = Assert.IsType<ViewResult>(actionResult);
        var model = Assert.IsAssignableFrom<string>(viewResult.ViewData.Model);
        model.ShouldEqual("Hello from me!");
    }

    [Fact]
    public void TestMissingResourceEntry()
    {
        //SETUP
        var logLocalizer = new StubLocalizeDefaultWithLogging<HomeController>();
        var controller = new LocalizeWithDefaultController(logLocalizer);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        //ATTEMPT
        var actionResult = controller.MissingResourceEntry();

        //VERIFY
        var viewResult = Assert.IsType<ViewResult>(actionResult);
        var model = Assert.IsAssignableFrom<string>(viewResult.ViewData.Model);
        model.ShouldStartWith("LocalizeStringMessage: I forget to set up the resource entry. Time:");
    }
}