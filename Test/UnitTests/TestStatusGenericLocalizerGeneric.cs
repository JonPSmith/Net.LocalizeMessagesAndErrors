// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using LocalizeMessagesAndErrors;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Test.StubClasses;
using TestSupport.EfHelpers;
using Xunit.Extensions.AssertExtensions;
using Xunit;

namespace Test.UnitTests;

public class TestStatusGenericLocalizerGeneric
{
    private readonly ILogger<LocalizeWithDefault<TestStatusGenericLocalizerGeneric>> _logger;
    private List<LogOutput> _logs;

    public TestStatusGenericLocalizerGeneric()
    {
        _logs = new List<LogOutput>(); //logs content is emptied before each test
        _logger = new LoggerFactory(
                new[] { new MyLoggerProviderActionOut(log => _logs.Add(log)) })
            .CreateLogger<LocalizeWithDefault<TestStatusGenericLocalizerGeneric>>();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TestLocalizeFormattedMessage_SetGetValue(bool addError)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizerGeneric>(
            new Dictionary<string, string> { { "test", "Error from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new LocalizeWithDefault<TestStatusGenericLocalizerGeneric>(_logger, stubLocalizer);
        var status = new StatusGenericLocalizer<string, TestStatusGenericLocalizerGeneric>("en-GB", defaultLocalizer);

        //ATTEMPT
        status.SetResult("some data");
        if (addError) status.AddErrorString(null, "An error");

        //VERIFY
        status.Result.ShouldEqual( addError ? null : "some data");
    }

    [Theory]
    [InlineData("en", "Error from readable string")]
    [InlineData("en-US", "Error from resource file")]
    public void TestLocalizeFormattedMessage_AddError_String(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizerGeneric>(
            new Dictionary<string, string> { { "test", "Error from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new LocalizeWithDefault<TestStatusGenericLocalizerGeneric>(_logger, stubLocalizer);
        var status = new StatusGenericLocalizer<string, TestStatusGenericLocalizerGeneric>(cultureOfMessage, defaultLocalizer); 

        //ATTEMPT
        var errors = status.AddErrorString("test", "Error from readable string");

        //VERIFY
        status.GetAllErrors().ShouldEqual(expectedMessage);
    }

    [Theory]
    [InlineData("en", "Error 123 from readable string")]
    [InlineData("en-US", "Error 123 from resource file")]
    public void TestLocalizeFormattedMessage_AddErrorFormatted(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizerGeneric>(
            new Dictionary<string, string> { { "test", "Error {0} from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new LocalizeWithDefault<TestStatusGenericLocalizerGeneric>(_logger, stubLocalizer);
        var status = new StatusGenericLocalizer<string, TestStatusGenericLocalizerGeneric>(cultureOfMessage, defaultLocalizer);

        //ATTEMPT
        var errors = status.AddErrorFormatted("test", $"Error {123} from readable string");

        //VERIFY
        status.GetAllErrors().ShouldEqual(expectedMessage);
    }

    [Theory]
    [InlineData("en", "Error 123 from readable string")]
    [InlineData("en-US", "Error 123 from resource file")]
    public void TestLocalizeFormattedMessage_AddErrorFormattedWithParams(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizerGeneric>(
            new Dictionary<string, string> { { "test", "Error {0} from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new LocalizeWithDefault<TestStatusGenericLocalizerGeneric>(_logger, stubLocalizer);
        var status = new StatusGenericLocalizer<string, TestStatusGenericLocalizerGeneric>(cultureOfMessage, defaultLocalizer);

        //ATTEMPT
        var errors = status.AddErrorFormattedWithParams("test", 
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
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizerGeneric>(
            new Dictionary<string, string> { { "test", "Error {0}, {1} from resource file" } });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new LocalizeWithDefault<TestStatusGenericLocalizerGeneric>(_logger, stubLocalizer);
        var status = new StatusGenericLocalizer<string, TestStatusGenericLocalizerGeneric>(cultureOfMessage, defaultLocalizer);

        //ATTEMPT
        var errors = status.AddErrorFormattedWithParams("test", new FormattableString[]
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

    [Theory]
    [InlineData("en", "Success from readable string")]
    [InlineData("en-US", "Success from resource file")]
    public void TestLocalizeFormattedMessage_SetMessageFormatted(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizerGeneric>(
            new Dictionary<string, string>
            {
                { "SuccessMessage", "Success from resource file" },
                { "StatusGenericLocalizer_MessageHasErrors", "Failed with 1 error" }
            });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new LocalizeWithDefault<TestStatusGenericLocalizerGeneric>(_logger, stubLocalizer);
        var status = new StatusGenericLocalizer<string, TestStatusGenericLocalizerGeneric>(cultureOfMessage, defaultLocalizer);
        status.SetMessageFormatted("SuccessMessage", $"Success from readable string");

        //VERIFY
        status.Message.ShouldEqual(expectedMessage);
    }

    [Theory]
    [InlineData("en", "Failed with 1 error")]
    [InlineData("en-US", "Resource file: Failed with 1 error")]
    public void TestLocalizeFormattedMessage_MessageErrors(string cultureOfMessage, string expectedMessage)
    {
        //SETUP
        var stubLocalizer = new StubStringLocalizer<TestStatusGenericLocalizerGeneric>(
            new Dictionary<string, string>
            {
                { "SuccessMessage", "Success from resource file" },
                { "StatusGenericLocalizer_MessageHasErrors", "Resource file: Failed with {0} error{1}" }
            });
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

        var defaultLocalizer = new LocalizeWithDefault<TestStatusGenericLocalizerGeneric>(_logger, stubLocalizer);
        var status = new StatusGenericLocalizer<string, TestStatusGenericLocalizerGeneric>(cultureOfMessage, defaultLocalizer);
        status.AddErrorString("test", "there is an error");

        //VERIFY
        status.Message.ShouldEqual(expectedMessage);
    }
}