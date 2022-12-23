// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

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

public class TestLocalizeFormattedMessage
{
    private readonly ILogger<DefaultLocalizer<TestLocalizeFormattedMessage>> _logger;
    private List<LogOutput> _logs;

    public TestLocalizeFormattedMessage()
    {
        _logs = new List<LogOutput> (); //logs content is emptied before each test
        _logger = new LoggerFactory(
                new[] { new MyLoggerProviderActionOut(log => _logs.Add(log)) })
            .CreateLogger<DefaultLocalizer<TestLocalizeFormattedMessage>>();
    }

    [Fact]
    public void TestStubStringLocalizer()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeFormattedMessage>(
            new Dictionary<string, string>
            {
                { "constant string", "Message from resource file" },
                { "dynamic string", "Message {0} from resource file" }
            }, false);

        //ATTEMPT
        var message1 = stubLocalizer["constant string"];
        var message2 = stubLocalizer["dynamic string", 1];
        var message3 = stubLocalizer["missing"];

        //VERIFY
        message1.Value.ShouldEqual("Message from resource file");
        message2.Value.ShouldEqual("Message 1 from resource file");
        message3.ResourceNotFound.ShouldBeTrue();
    }



    [Theory]
    [InlineData("en", "Message 123 from readable string")]
    [InlineData("en-US", "Message 123 from resource file")]
    public void TestLocalizeFormattedMessage_CultureOfString(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeFormattedMessage>(
            new Dictionary<string, string> {
            {
                "test".ClassLocalizeKey(this, true).LocalizeKey, "Message {0} from resource file"
            } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new DefaultLocalizer<TestLocalizeFormattedMessage>(
            new StubDefaultLocalizerOptions(cultureOfMessage), _logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeFormattedMessage("test".ClassLocalizeKey(this, true), 
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
            new Dictionary<string, string>
            {
                { "test".ClassLocalizeKey(this, true).LocalizeKey, "Message1 {0} Message2 {1} from resource file" }
            });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new DefaultLocalizer<TestLocalizeFormattedMessage>(
            new StubDefaultLocalizerOptions(cultureOfMessage), _logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeFormattedMessage("test".ClassLocalizeKey(this, true),
            $"Message1 {123} ", $"Message2 {456}" , $" from readable string");

        //VERIFY
        message.ShouldEqual(expectedMessage);
    }

    [Fact]
    public void TestLocalizeStringMessage_AlreadyLocalized()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeFormattedMessage>(
            new Dictionary<string, string> { });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new DefaultLocalizer<TestLocalizeFormattedMessage>(new StubDefaultLocalizerOptions(), _logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeFormattedMessage(this.AlreadyLocalized(),
            $"This message is already localized");

        //VERIFY
        message.ShouldEqual("This message is already localized");
    }

    //-----------------------------------------------------------------
    //error situations

    [Fact]
    public void TestLocalizeStringMessage_MissingResource()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeFormattedMessage>(
            new Dictionary<string, string>(), false);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new DefaultLocalizer<TestLocalizeFormattedMessage>(
            new StubDefaultLocalizerOptions("fi-FI"), _logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeFormattedMessage("test".ClassLocalizeKey(this, true),
            $"Message {123} from readable string");

        //VERIFY
        message.ShouldEqual("Message 123 from readable string");
        
        _logs.Single().Message.ShouldStartWith(
            "The message with the localizeKey name of 'TestLocalizeFormattedMessage_test' and culture of 'en-GB' " +
            "was not found in the 'dummy searched location' resource. The message came from " +
            "TestLocalizeFormattedMessage.TestLocalizeStringMessage_MissingResource");
    }

    [Fact]
    public void TestLocalizeStringMessage_IncorrectResourceFormat()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestLocalizeFormattedMessage>(
            new Dictionary<string, string> { { "test".ClassLocalizeKey(this, true).LocalizeKey, "Message {0}{1} from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var service = new DefaultLocalizer<TestLocalizeFormattedMessage>(
            new StubDefaultLocalizerOptions("fi-FI"), _logger, stubLocalizer);

        //ATTEMPT
        var message = service.LocalizeFormattedMessage("test".ClassLocalizeKey(this, true),
            $"Message {123} from readable string");

        //VERIFY
        message.ShouldEqual("Message 123 from readable string");
        _logs.Single().Message.ShouldStartWith(
            "The resourced string 'Message {0}{1} from resource file' had the following FormatException error:");
    }
}