// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using LocalizeMessagesAndErrors;
using LocalizeMessagesAndErrors.UnitTestingCode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests;

public class TestSimpleLocalizer
{
    private StubStringLocalizer _stubStringLocalizer = null!;

    private IServiceProvider SetupServices()
    {
        var stubStringFactory = new StubStringLocalizerFactory(
            new Dictionary<string, string>
            {
                { "SimpleLocalizer(My message)", "Message from resource" },
            }, false);
        _stubStringLocalizer = stubStringFactory.StubStringLocalizer;

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IStringLocalizerFactory>(options => stubStringFactory);
        services.RegisterDefaultLocalizer("en", null);
        services.RegisterSimpleLocalizer<TestSimpleLocalizerFactory>();
        return services.BuildServiceProvider();
    }

    [Fact]
    public void TestCreateSimpleLocalizerService()
    {
        //SETUP
        var service = SetupServices();

        //ATTEMPT
        var simpleLocalizer = service.GetRequiredService<ISimpleLocalizer>();

        //VERIFY
        simpleLocalizer.ShouldNotBeNull();
    }

    [Fact]
    public void TestLocalizeString()
    {
        //SETUP
        var service = SetupServices();
        var simpleLocalizer = service.GetRequiredService<ISimpleLocalizer>();

        //ATTEMPT
        var message = simpleLocalizer.LocalizeString("My message", this);

        //VERIFY
        message.ShouldEqual("My message");
    }

    [Fact]
    public void TestLocalizeFormatted()
    {
        //SETUP
        var service = SetupServices();
        var simpleLocalizer = service.GetRequiredService<ISimpleLocalizer>();

        //ATTEMPT
        var message = simpleLocalizer.LocalizeFormatted($"Date is {DateTime.Now:M}.", this);

        //VERIFY
        message.ShouldEqual($"Date is {DateTime.Now:M}.");
        _stubStringLocalizer.LastLocalizeKey.ShouldEqual("SimpleLocalizer(Date is {0:M}.)");
    }

    private static class MyStaticClass {}

    [Fact]
    public void TestStaticLocalizeString()
    {
        //SETUP
        var service = SetupServices();
        var simpleLocalizer = service.GetRequiredService<ISimpleLocalizer>();

        //ATTEMPT
        var message = simpleLocalizer.StaticLocalizeString("My message", typeof(MyStaticClass));

        //VERIFY
        message.ShouldEqual("My message");
        _stubStringLocalizer.LastLocalizeKey.ShouldEqual("SimpleLocalizer(My message)");
    }

    [Fact]
    public void TestStaticLocalizeFormatted()
    {
        //SETUP
        var service = SetupServices();
        var simpleLocalizer = service.GetRequiredService<ISimpleLocalizer>();

        //ATTEMPT
        var message = simpleLocalizer.StaticLocalizeFormatted($"My {123} message", typeof(MyStaticClass));

        //VERIFY
        message.ShouldEqual("My 123 message");
        _stubStringLocalizer.LastLocalizeKey.ShouldEqual("SimpleLocalizer(My {0} message)");
    }

    //[Fact]
    //public void TestCheckDifferentPrefix()
    //{
    //    //SETUP
    //    var services = new ServiceCollection();
    //    services.AddSingleton(typeof(IDefaultLocalizer<>), typeof(StubDefaultLocalizer<>));
    //    var provider = services.BuildServiceProvider();
    //    var simpleLoc = new SimpleLocalizer(provider, 
    //        new StubSimpleLocalizerOptions<TestSimpleLocalizer>{PrefixKeyString = "XXX"});
    //    var stubDefaultLoc =
    //        (StubDefaultLocalizer<TestSimpleLocalizer>)provider
    //            .GetRequiredService<IDefaultLocalizer<TestSimpleLocalizer>>();

    //    //ATTEMPT
    //    var message = simpleLoc.LocalizeString("My message", this);

    //    //VERIFY
    //    message.ShouldEqual("My message");
    //    stubDefaultLoc.LastKeyData.LocalizeKey.ShouldEqual("XXX(My message)");
    //    stubDefaultLoc.LastKeyData.CallingClass.Name.ShouldEqual("TestSimpleLocalizer");
    //}
}