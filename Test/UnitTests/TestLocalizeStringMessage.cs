﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using LocalizeMessagesAndErrors;
using LocalizeMessagesAndErrors.UnitTestingCode;
using Microsoft.Extensions.Logging;
using Test.StubClasses;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests;

public class TestLocalizeStringMessage
{
    private readonly ILogger<DefaultLocalizer<TestLocalizeStringMessage>> _logger;
    private List<LogOutput> _logs;


    public TestLocalizeStringMessage()
    {
        _logs = new List<LogOutput> (); //logs content is emptied before each test
        _logger = new LoggerFactory(
                new[] { new MyLoggerProviderActionOut(log => _logs.Add(log)) })
            .CreateLogger<DefaultLocalizer<TestLocalizeStringMessage>>();
    }

    [Theory]
    [InlineData("en", "Message from readable string")]
    [InlineData("en-US", "Message from resource file")]
    public void TestLocalizeStringMessage_CultureOfString(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeStringMessage>(
            new Dictionary<string, string>
            {
                { "test".ClassLocalizeKey(this, true).LocalizeKey, "Message from resource file" }
            });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new DefaultLocalizer<TestLocalizeStringMessage>(
            new StubDefaultLocalizerOptions(cultureOfMessage), _logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeStringMessage("test".ClassLocalizeKey(this, true), 
            "Message from readable string");

        //VERIFY
        message.ShouldEqual(expectedMessage);
    }

    [Fact]
    public void TestLocalizeStringMessage_AlreadyLocalized()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeStringMessage>(
            new Dictionary<string, string> {});
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new DefaultLocalizer<TestLocalizeStringMessage>(new StubDefaultLocalizerOptions(), _logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeStringMessage(this.AlreadyLocalized(),
            "This message is already localized");

        //VERIFY
        message.ShouldEqual("This message is already localized");
    }

    //-----------------------------------------------------------------
    //error situations

    [Fact]
    public void TestLocalizeStringMessage_MissingResource()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeStringMessage>(
            new Dictionary<string, string>(), false);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new DefaultLocalizer<TestLocalizeStringMessage>(
            new StubDefaultLocalizerOptions("fr")
            {
                SupportedCultures = new[] { "en", "fr" }
            }, _logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeStringMessage("test".ClassLocalizeKey(this, true),
            "Message from readable string");

        //VERIFY
        message.ShouldEqual("Message from readable string");
        _logs.Count.ShouldEqual(1);
        _logs.Single().Message.ShouldStartWith(
            "The message with the localizeKey name of 'TestLocalizeStringMessage_test' and culture of " +
            "'en-GB' was not found in the 'dummy searched location' resource. " +
            "The message came from TestLocalizeStringMessage.TestLocalizeStringMessage_MissingResource");
    }

    [Fact]
    public void TestLocalizeStringMessage_MissingResourceButNotSupported()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeStringMessage>(
            new Dictionary<string, string>(), false);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");

        var service = new DefaultLocalizer<TestLocalizeStringMessage>(
            new StubDefaultLocalizerOptions("fr")
            {
                SupportedCultures = new []{ "en", "fr" }
            }, _logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeStringMessage("test".ClassLocalizeKey(this, true),
            "Message from readable string");

        //VERIFY
        message.ShouldEqual("Message from readable string");
        _logs.Count.ShouldEqual(0);
    }

    [Fact]
    public void TestLocalizeStringMessage_MissingResourceButSupportedCulturesNull()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeStringMessage>(
            new Dictionary<string, string>(), false);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");

        var service = new DefaultLocalizer<TestLocalizeStringMessage>(
            new StubDefaultLocalizerOptions("fr")
            {
                SupportedCultures = null
            }, _logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeStringMessage("test".ClassLocalizeKey(this, true),
            "Message from readable string");

        //VERIFY
        message.ShouldEqual("Message from readable string");
        _logs.Count.ShouldEqual(1);
    }

    [Fact] public void TestLocalizeStringMessage_NullMessage()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeStringMessage>(
            new Dictionary<string, string>());
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new DefaultLocalizer<TestLocalizeStringMessage>(new StubDefaultLocalizerOptions(), _logger, stubLocalizer);

        //ATTEMPT
        var ex = Assert.Throws<ArgumentNullException>(() => 
            service.LocalizeStringMessage("test".ClassLocalizeKey(this, true), null));

        //VERIFY
        ex.Message.ShouldEqual("Value cannot be null. (Parameter 'message')");
    }

    [Fact]
    public void TestStubDefaultLocalizer()
    {
        //SETUP
        var defaultLoc = new StubDefaultLocalizer();

        //ATTEMPT
        var message = defaultLoc.LocalizeStringMessage(
            "MyLocalizeKey".MethodLocalizeKey(this),
            "My message");

        //VERIFY
        message.ShouldEqual("My message");
        defaultLoc.LastKeyData.LocalizeKey.ShouldEqual(
            "TestStubDefaultLocalizer_MyLocalizeKey");
    }
}