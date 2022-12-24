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
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests;

public class TestSimpleLocalizerFactory
{

    private IServiceProvider SetupServices()
    {
        var stubStringFactory = new StubStringLocalizerFactory(
            new Dictionary<string, string>
            {
                { "SimpleLocalizer(Good entry)", "Message from resource" },
            }, false);

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IStringLocalizerFactory>(options => stubStringFactory);
        services.RegisterDefaultLocalizer("en", null);
        services.RegisterSimpleLocalizer<TestSimpleLocalizerFactory>();
        return services.BuildServiceProvider();
    }

    [Fact]
    public void TestCreate_ValidInstance()
    {
        //SETUP
        var factory = new SimpleLocalizerFactory(SetupServices());

        //ATTEMPT
        var simpleLocalizer = factory.Create(typeof(TestSimpleLocalizerFactory));

        //VERIFY
        simpleLocalizer.ShouldNotBeNull();
        simpleLocalizer.ShouldNotBeType(typeof(StubDefaultLocalizer));
    }

    [Fact]
    public void TestCreate_MessageOk()
    {
        //SETUP
        var services = SetupServices();
        var factory = new SimpleLocalizerFactory(services);
        var simpleLocalizer = factory.Create(typeof(TestSimpleLocalizerFactory));

        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");

        //ATTEMPT
        var message = simpleLocalizer.LocalizeString("Good entry", this);

        //VERIFY
        message.ShouldEqual("Message from resource");
    }

    [Fact]
    public void TestCreate_MessageNotInLookup()
    {
        //SETUP
        var services = SetupServices();
        var factory = new SimpleLocalizerFactory(services);
        var simpleLocalizer = factory.Create(typeof(TestSimpleLocalizerFactory));

        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");

        //ATTEMPT
        var message = simpleLocalizer.LocalizeString("Bad entry", this);

        //VERIFY
        message.ShouldEqual("Bad entry");
    }
}