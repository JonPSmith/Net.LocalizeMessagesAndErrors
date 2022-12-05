// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using LocalizeMessagesAndErrors;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using LocalizedWebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Test.StubClasses;
using Xunit.Extensions.AssertExtensions;
using Microsoft.Extensions.Logging;
using TestSupport.EfHelpers;
using Xunit;

namespace Test.UnitTests;

public class TestLocalizeWithDefaultController
{
    //see https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/testing#unit-testing-controllers
    //for testing the return of a controller action

    [Fact]
    public void TestControllerActionIndex()
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
        model.ShouldEqual("Localized via IStringLocalizer service with culture 'en-GB'."); 
    }
}