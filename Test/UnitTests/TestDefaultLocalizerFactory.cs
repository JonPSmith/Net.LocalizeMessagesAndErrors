// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using LocalizeMessagesAndErrors;
using LocalizeMessagesAndErrors.UnitTestingCode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests;

public class TestDefaultLocalizerFactory
{

    private IServiceProvider SetupServices()
    {
        var stubStringFactory = new StubStringLocalizerFactory(
            new Dictionary<string, string>
            {
                { "Test", "Message from resource" },
            }, false);

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IStringLocalizerFactory>(options => stubStringFactory);
        services.RegisterDefaultLocalizer("en", null);
        return services.BuildServiceProvider();
    }

    [Fact]
    public void TestCreate_ValidInstance()
    {
        //SETUP
        var factory = new DefaultLocalizerFactory(SetupServices());

        //ATTEMPT
        var defaultLocalizer = factory.Create(typeof(TestDefaultLocalizerFactory));

        //VERIFY
        defaultLocalizer.ShouldNotBeNull();
        defaultLocalizer.ShouldNotBeType(typeof(StubDefaultLocalizer));
    }

    [Fact]
    public void TestCreate_MessageOk()
    {
        //SETUP
        var services = SetupServices();
        var factory = new DefaultLocalizerFactory(services);
        var defaultLocalizer = factory.Create(typeof(TestDefaultLocalizerFactory));

        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");

        //ATTEMPT
        var message = defaultLocalizer.LocalizeStringMessage(
            "Test".JustThisLocalizeKey(this),
            "Test entry");

        //VERIFY
        message.ShouldEqual("Message from resource");
    }

    [Fact]
    public void TestCreate_MessageNotInLookup()
    {
        //SETUP
        var services = SetupServices();
        //var logger = services.GetRequiredService<ILoggerFactory>();
        var factory = new DefaultLocalizerFactory(services);
        var defaultLocalizer = factory.Create(typeof(TestDefaultLocalizerFactory));

        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");

        //ATTEMPT
        var message = defaultLocalizer.LocalizeStringMessage(
            "MissingKey".JustThisLocalizeKey(this),
            "Test entry");

        //VERIFY
        message.ShouldEqual("Test entry");
    }
}