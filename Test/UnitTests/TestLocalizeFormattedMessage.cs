// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

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

public class TestLocalizeFormattedMessage
{
    private readonly ILogger<LocalizeWithDefault<TestLocalizeFormattedMessage>> _logger;
    private List<LogOutput> _logs;


    public TestLocalizeFormattedMessage()
    {
        _logs = new List<LogOutput> (); //logs content is emptied before each test
        _logger = new LoggerFactory(
                new[] { new MyLoggerProviderActionOut(log => _logs.Add(log)) })
            .CreateLogger<LocalizeWithDefault<TestLocalizeFormattedMessage>>();
    }

    [Theory]
    [InlineData("en", "Message 123 from readable string")]
    [InlineData("en-US", "Message 123 from resource file")]
    public void TestLocalizeFormattedMessage_CultureOfString(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeFormattedMessage>(
            new Dictionary<string, string> { { "test", "Message {0} from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new LocalizeWithDefault<TestLocalizeFormattedMessage>(_logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeFormattedMessage("test", cultureOfMessage, 
            $"Message {123} from readable string");

        //VERIFY
        message.ShouldEqual(expectedMessage);
    }

    [Theory]
    [InlineData("en", "Message1 123 Message2 456 from readable string")]
    [InlineData("en-US", "Message1 123 Message2 456 from resource file")]
    public void TestLocalizeFormattedMessage_MultipleFormattedMessage(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeFormattedMessage>(
            new Dictionary<string, string> { { "test", "Message1 {0} Message2 {1} from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new LocalizeWithDefault<TestLocalizeFormattedMessage>(_logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeFormattedMessage("test", cultureOfMessage,
            $"Message1 {123} ", $"Message2 {456}" , $" from readable string");

        //VERIFY
        message.ShouldEqual(expectedMessage);
    }

    //-----------------------------------------------------------------
    //error situations

    [Fact]
    public void TestLocalizeStringMessage_MissingResource()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeFormattedMessage>(
            new Dictionary<string, string>());
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new LocalizeWithDefault<TestLocalizeFormattedMessage>(_logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeFormattedMessage("test", "fi-FI",
            $"Message {123} from readable string");

        //VERIFY
        message.ShouldEqual("Message 123 from readable string");
        _logs.Single().Message.ShouldEqual(
            "The entry with the name 'test' and culture of 'en-GB' was not found in the 'TestLocalizeFormattedMessage' resource.");
    }

    [Fact]
    public void TestLocalizeStringMessage_IncorrectResourceFormat()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeFormattedMessage>(
            new Dictionary<string, string> { { "test", "Message {0}{1} from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new LocalizeWithDefault<TestLocalizeFormattedMessage>(_logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeFormattedMessage("test", "fi-FI",
            $"Message {123} from readable string");

        //VERIFY
        message.ShouldEqual("Message 123 from readable string");
        _logs.Single().Message.ShouldStartWith(
            "The resourced string 'Message {0}{1} from resource file' had the following FormatException error:");
    }
}