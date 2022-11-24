// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using LocalizeMessagesAndErrors;
using Microsoft.Extensions.Logging;
using Test.StubClasses;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests;

public class TestLocalizeStringMessage
{
    private readonly ILogger<LocalizeWithDefault<TestLocalizeStringMessage>> _logger;
    private List<LogOutput> _logs;


    public TestLocalizeStringMessage()
    {
        _logs = new List<LogOutput> (); //logs content is emptied before each test
        _logger = new LoggerFactory(
                new[] { new MyLoggerProviderActionOut(log => _logs.Add(log)) })
            .CreateLogger<LocalizeWithDefault<TestLocalizeStringMessage>>();
    }

    [Theory]
    [InlineData("en", "Message from readable string")]
    [InlineData("en-US", "Message from resource file")]
    public void TestLocalizeStringMessage_CultureOfString(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeStringMessage>(
            new Dictionary<string, string> { { "test".ClassLocalizeKey(this).ToString(), "Message from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new LocalizeWithDefault<TestLocalizeStringMessage>(_logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeStringMessage("test".ClassLocalizeKey(this), cultureOfMessage, 
            "Message from readable string");

        //VERIFY
        message.ShouldEqual(expectedMessage);
    }

    [Fact]
    public void TestLocalizeStringMessage_NullMessageKey()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeStringMessage>(
            new Dictionary<string, string> { { "test", "Message from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new LocalizeWithDefault<TestLocalizeStringMessage>(_logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeStringMessage(null, "fi-FI",
            "Message from readable string");

        //VERIFY
        message.ShouldEqual("Message from readable string");
        _logs.Count.ShouldEqual(1);
        _logs.Single().Message.ShouldStartWith("The message 'Message from readable string' had no localizeKey. " +
                                               "This can happen if you set the StatusGeneric Message directly.");
    }

    //-----------------------------------------------------------------
    //error situations

    [Fact]
    public void TestLocalizeStringMessage_MissingResource()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeStringMessage>(
            new Dictionary<string, string>());
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new LocalizeWithDefault<TestLocalizeStringMessage>(_logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeStringMessage("test".ClassLocalizeKey(this), "fi-FI",
            "Message from readable string");

        //VERIFY
        message.ShouldEqual("Message from readable string");
        _logs.Single().Message.ShouldEqual(
            "The entry with the name 'Test.UnitTests.TestLocalizeStringMessage_test' and culture of " +
            "'en-GB' was not found in the 'TestLocalizeStringMessage' resource.");
    }

    [Fact] public void TestLocalizeStringMessage_NullMessage()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeStringMessage>(
            new Dictionary<string, string>());
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new LocalizeWithDefault<TestLocalizeStringMessage>(_logger, stubLocalizer);

        //ATTEMPT
        var ex = Assert.Throws<ArgumentNullException>(() => 
            service.LocalizeStringMessage("test".ClassLocalizeKey(this), "en-GB", null));

        //VERIFY
        ex.Message.ShouldEqual("Value cannot be null. (Parameter 'message')");
    }
}