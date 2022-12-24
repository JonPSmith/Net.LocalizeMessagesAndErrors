// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using LocalizeMessagesAndErrors;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using LocalizeMessagesAndErrors.UnitTestingCode;
using Test.StubClasses;
using TestSupport.EfHelpers;
using Xunit.Extensions.AssertExtensions;
using Xunit;

namespace Test.UnitTests;

public class TestStatusGenericLocalizer
{
    private readonly ILogger<DefaultLocalizer<TestStatusGenericLocalizer>> _logger;
    private List<LogOutput> _logs;

    public TestStatusGenericLocalizer()
    {
        _logs = new List<LogOutput>(); //logs content is emptied before each test
        _logger = new LoggerFactory(
                new[] { new MyLoggerProviderActionOut(log => _logs.Add(log)) })
            .CreateLogger<DefaultLocalizer<TestStatusGenericLocalizer>>();
    }

    [Theory]
    [InlineData("en", "Error from readable string")]
    [InlineData("en-US", "Error from resource file")]
    public void TestLocalizeFormattedMessage_AddError_String(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizer>(
            new Dictionary<string, string>
            {
                { "test".ClassLocalizeKey(this, true).LocalizeKey, "Error from resource file" },
                { "StatusGenericLocalizer_MessageHasOneError", "Failed with 1 error."}
            });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
        var defaultLocalizer = new DefaultLocalizer<TestStatusGenericLocalizer>(
            new StubDefaultLocalizerOptions(cultureOfMessage), _logger, stubLocalizer);
        var status = new StatusGenericLocalizer(defaultLocalizer); 


        //ATTEMPT
        var errors = status.AddErrorString("test".ClassLocalizeKey(this, true), "Error from readable string");

        //VERIFY
        status.GetAllErrors().ShouldEqual(expectedMessage);
        status.HasErrors.ShouldBeTrue();
        status.IsValid.ShouldBeFalse();
        status.Message.ShouldEqual("Failed with 1 error.");
    }

    [Theory]
    [InlineData("en", "Error 123 from readable string")]
    [InlineData("en-US", "Error 123 from resource file")]
    public void TestLocalizeFormattedMessage_AddErrorFormatted(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizer>(
            new Dictionary<string, string>
            {
                { "test".ClassLocalizeKey(this, true).LocalizeKey, "Error {0} from resource file" },
                { "StatusGenericLocalizer_MessageHasOneError", "Failed with 1 error."}
            });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new DefaultLocalizer<TestStatusGenericLocalizer>(
            new StubDefaultLocalizerOptions(cultureOfMessage), _logger, stubLocalizer);
        var status = new StatusGenericLocalizer(defaultLocalizer);

        //ATTEMPT
        var errors = status.AddErrorFormatted("test".ClassLocalizeKey(this, true), $"Error {123} from readable string");

        //VERIFY
        status.GetAllErrors().ShouldEqual(expectedMessage);
        status.HasErrors.ShouldBeTrue();
        status.IsValid.ShouldBeFalse();
        status.Message.ShouldEqual("Failed with 1 error.");
    }

    [Theory]
    [InlineData("en", "Error 123 from readable string")]
    [InlineData("en-US", "Error 123 from resource file")]
    public void TestLocalizeFormattedMessage_AddErrorFormattedWithParams(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizer>(
            new Dictionary<string, string> { { "test".ClassLocalizeKey(this, true).LocalizeKey, "Error {0} from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new DefaultLocalizer<TestStatusGenericLocalizer>(
            new StubDefaultLocalizerOptions(cultureOfMessage), _logger, stubLocalizer);
        var status = new StatusGenericLocalizer(defaultLocalizer);

        //ATTEMPT
        var errors = status.AddErrorFormattedWithParams("test".ClassLocalizeKey(this, true), 
            $"Error {123} from readable string", "MyProperty");

        //VERIFY
        status.GetAllErrors().ShouldEqual(expectedMessage);
        status.Errors.Single().ErrorResult.ErrorMessage.ShouldEqual(expectedMessage);
        status.Errors.Single().ErrorResult.MemberNames.Single().ShouldEqual("MyProperty");
    }

    [Theory]
    [InlineData("en", "Error 123, 456 from readable string")]
    [InlineData("en-US", "Error 123, 456 from resource file")]
    public void TestLocalizeFormattedMessage_AddErrorFormattedWithParams_Multiple(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizer>(
            new Dictionary<string, string> { { "test".ClassLocalizeKey(this, true).LocalizeKey, "Error {0}, {1} from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new DefaultLocalizer<TestStatusGenericLocalizer>(
            new StubDefaultLocalizerOptions(cultureOfMessage), _logger, stubLocalizer);
        var status = new StatusGenericLocalizer(defaultLocalizer);

        //ATTEMPT
        var errors = status.AddErrorFormattedWithParams("test".ClassLocalizeKey(this, true), new FormattableString[]
        {
            $"Error {123},", $" {456} from readable string"
        }, "MyProperty");

        //VERIFY
        status.GetAllErrors().ShouldEqual(expectedMessage);
        status.Errors.Single().ErrorResult.ErrorMessage.ShouldEqual(expectedMessage);
        status.Errors.Single().ErrorResult.MemberNames.Single().ShouldEqual("MyProperty");
    }

    //------------------------------------------------------------
    //Message

    [Fact]
    public void TestLocalizeFormattedMessage_SetMessageDirectly()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizer>(new Dictionary<string, string>());
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new DefaultLocalizer<TestStatusGenericLocalizer>(new StubDefaultLocalizerOptions(), _logger, stubLocalizer);
        var status = new StatusGenericLocalizer(defaultLocalizer);

        //ATTEMPT
        var ex = Assert.Throws<InvalidOperationException>(() => status.Message = "test");

        //VERIFY
        ex.Message.ShouldEqual("You cannot set the Message when using StatusGenericLocalizer. Use the SetMessage methods to set the Message instead.");
    }

    [Theory]
    [InlineData("en", "Success from readable string")]
    [InlineData("en-US", "Success from resource file")]
    public void TestLocalizeFormattedMessage_SetMessageFormatted(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizer>(
            new Dictionary<string, string>
            {
                { "SuccessMessage".ClassLocalizeKey(this, true).LocalizeKey, "Success from resource file" },
                { "StatusGenericLocalizer_MessageHasErrors", "Failed with 1 error" }
            });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new DefaultLocalizer<TestStatusGenericLocalizer>(
            new StubDefaultLocalizerOptions(cultureOfMessage), _logger, stubLocalizer);
        var status = new StatusGenericLocalizer(defaultLocalizer);
        status.SetMessageFormatted("SuccessMessage".ClassLocalizeKey(this, true), $"Success from readable string");

        //VERIFY
        status.Message.ShouldEqual(expectedMessage);
        _logs.Any().ShouldBeFalse();
    }

    [Fact]
    public void TestAddOneError_Message()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizer>(
            new Dictionary<string, string>
            {
                { "test".ClassLocalizeKey(this, true).LocalizeKey, "Error {0} from resource file" },
                { "StatusGenericLocalizer_MessageHasOneError", "Failed with 1 error from resource file"}
            });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new DefaultLocalizer<TestStatusGenericLocalizer>(
            new StubDefaultLocalizerOptions("en-US"), _logger, stubLocalizer);
        var status = new StatusGenericLocalizer(defaultLocalizer);

        //ATTEMPT
        status.AddErrorString("test".ClassLocalizeKey(this, true), "Error1");

        //VERIFY
        status.Errors.Count.ShouldEqual(1);
        status.HasErrors.ShouldBeTrue();
        status.IsValid.ShouldBeFalse();
        status.Message.ShouldEqual("Failed with 1 error from resource file");
    }

    [Fact]
    public void TestAddTwoErrors_Message()
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizer>(
            new Dictionary<string, string>
            {
                { "test".ClassLocalizeKey(this, true).LocalizeKey, "Error {0} from resource file" },
                { "StatusGenericLocalizer_MessageHasManyErrors", "Failed with {0} errors from resource file"}
            });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new DefaultLocalizer<TestStatusGenericLocalizer>(
            new StubDefaultLocalizerOptions("en-US"), _logger, stubLocalizer);
        var status = new StatusGenericLocalizer(defaultLocalizer);

        //ATTEMPT
        status.AddErrorString("test".ClassLocalizeKey(this, true), "Error1");
        status.AddErrorString("test".ClassLocalizeKey(this, true), "Error2");

        //VERIFY
        status.Errors.Count.ShouldEqual(2);
        status.HasErrors.ShouldBeTrue();
        status.IsValid.ShouldBeFalse();
        status.Message.ShouldEqual("Failed with 2 errors from resource file");
    }
}