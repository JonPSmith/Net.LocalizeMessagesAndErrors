// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using LocalizedWebApp.Controllers;
using LocalizeMessagesAndErrors;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests;

public class TestRegisterLocalizeExtensions
{
    [Fact]
    public void TestRegisterLocalizeDefault()
    {
        //SETUP
        var services = new ServiceCollection();
        services.AddLogging();

        //ATTEMPT
        services.RegisterDefaultLocalizer("en");

        //VERIFY
        var provider = services.BuildServiceProvider();
        var options = provider.GetService<DefaultLocalizerOptions>();
        options.ShouldNotBeNull();
        options!.DefaultCulture.ShouldEqual("en");
        var service = provider.GetService<IDefaultLocalizer<HomeController>>();
        service.ShouldNotBeNull();
    }

    [Fact]
    public void TestRegisterSimpleLocalizer()
    {
        //SETUP
        var services = new ServiceCollection();
        services.AddLogging();

        //ATTEMPT
        services.RegisterSimpleLocalizer<HomeController>();

        //VERIFY
        var provider = services.BuildServiceProvider();
        var service = provider.GetService<ISimpleLocalizer>();
        service.ShouldNotBeNull();
    }
}