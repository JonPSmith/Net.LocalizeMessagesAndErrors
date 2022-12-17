// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Resources.NetStandard;
using System.Globalization;
using System.Threading;
using LocalizedWebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Test.StubClasses;
using Xunit.Extensions.AssertExtensions;
using Xunit;
using System.Collections;
using System;
using TestSupport.Attributes;
using Xunit.Abstractions;

namespace Test.UnitTests;

public class TestLocalizeWithDefaultController
{
    private readonly ITestOutputHelper _output;

    //see https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/testing#unit-testing-controllers
    //for testing the return of a controller action

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    public TestLocalizeWithDefaultController(ITestOutputHelper output)
    {
        _output = output;
    }

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

    [RunnableInDebugOnly]
    public void TestAccessResourceInLocalizedWebApp()
    {
        //SETUP
        //see https://learn.microsoft.com/en-us/dotnet/core/extensions/work-with-resx-files-programmatically#retrieve-a-specific-resource
        var resourceFilePath =
            "C:\\Users\\JonPSmith\\source\\repos\\Net.LocalizeMessagesAndErrors\\LocalizedWebApp\\Resources\\Controllers.HomeController.fr.resx";


        //ATTEMPT
        using var resxSet = new ResXResourceReader(resourceFilePath);
        foreach (DictionaryEntry d in resxSet)
        {
            _output.WriteLine(d.Key.ToString() + ":\t" + d.Value.ToString());
        }

        //VERIFY
    }
}