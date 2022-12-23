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
    public void TestRegisterLocalizeDefault_Basic()
    {
        //SETUP
        var services = new ServiceCollection();
        services.AddLogging();

        //ATTEMPT
        services.RegisterDefaultLocalizer("en", new[] { "en", "fr" });

        //VERIFY
        var provider = services.BuildServiceProvider();
        var options = provider.GetService<DefaultLocalizerOptions>();
        options.ShouldNotBeNull();
        options!.DefaultCulture.ShouldEqual("en");
        options!.SupportedCultures.ShouldEqual(new[] { "en", "fr" });
        var localizer = provider.GetService<IDefaultLocalizer<HomeController>>();
        localizer.ShouldNotBeNull();
        var factory = provider.GetService<IDefaultLocalizerFactory>();
        factory.ShouldNotBeNull();
    }

    [Fact]
    public void TestRegisterSimpleLocalizer()
    {
        //SETUP
        var services = new ServiceCollection();
        services.AddLogging();
        services.RegisterDefaultLocalizer("en", new[] { "en", "fr" });

        //ATTEMPT
        services.RegisterSimpleLocalizer<HomeController>();

        //VERIFY
        var provider = services.BuildServiceProvider();
        var localizer = provider.GetService<ISimpleLocalizer>();
        localizer.ShouldNotBeNull();
        var factory = provider.GetService<ISimpleLocalizerFactory>();
        factory.ShouldNotBeNull();
    }
}