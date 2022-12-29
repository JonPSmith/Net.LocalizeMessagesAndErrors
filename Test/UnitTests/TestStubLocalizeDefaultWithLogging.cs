// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using LocalizeMessagesAndErrors;
using System.Linq;
using Test.StubClasses;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests;

public class TestStubLocalizeDefaultWithLogging
{
    private readonly ITestOutputHelper _output;

    public TestStubLocalizeDefaultWithLogging(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestStubLocalizeDefaultWithLogging_Logs()
    {
        //SETUP
        var stubLocalizer = new StubDefaultLocalizerWithLogging
            ("en", GetType());

        //ATTEMPT
        stubLocalizer.LocalizeStringMessage(
            "Test1".MethodLocalizeKey(this),
            "Hello Earth");
        stubLocalizer.LocalizeStringMessage(
            "Test2".MethodLocalizeKey(this),
            "Hello Mars");

        //VERIFY
        stubLocalizer.Logs.Count.ShouldEqual(2);
        stubLocalizer.Logs[0].ActualMessage.ShouldEqual("Hello Earth");
        stubLocalizer.Logs[0].LocalizeKey.ShouldEqual(
            "TestStubLocalizeDefaultWithLogging_Logs_Test1");
        stubLocalizer.Logs[1].ActualMessage.ShouldEqual("Hello Mars");
        stubLocalizer.Logs[1].LocalizeKey.ShouldEqual(
            "TestStubLocalizeDefaultWithLogging_Logs_Test2");
    }

    [Fact]
    public void TestAddErrorString()
    {
        //SETUP
        var stubLocalizer = new StubDefaultLocalizerWithLogging<TestStubLocalizeDefaultWithLogging>("en");

        //ATTEMPT
        var status = new StatusGenericLocalizer(stubLocalizer);
        status.AddErrorString("test".MethodLocalizeKey(this), "An Error");

        //VERIFY
        status.Errors.Single().ToString().ShouldEqual("An Error");
        stubLocalizer.PossibleError.ShouldBeNull();
    }

    [Fact]
    public void TestSetMessageString()
    {
        //SETUP
        var stubLocalizer = new StubDefaultLocalizerWithLogging<TestStubLocalizeDefaultWithLogging>("en");

        //ATTEMPT
        var status = new StatusGenericLocalizer(stubLocalizer);
        status.SetMessageString("test".MethodLocalizeKey(this), "Status Message1");

        //VERIFY
        status.Message.ShouldEqual("Status Message1");
        stubLocalizer.PossibleError.ShouldBeNull();
    }


    [Fact]
    public void TestSetMessageFormatted()
    {
        //SETUP
        var stubLocalizer = new StubDefaultLocalizerWithLogging<TestStubLocalizeDefaultWithLogging>("en");

        //ATTEMPT
        var status = new StatusGenericLocalizer(stubLocalizer);
        status.SetMessageFormatted("test".MethodLocalizeKey(this), $"Status Message{2}");

        //VERIFY
        status.Message.ShouldEqual("Status Message2");
        stubLocalizer.PossibleError.ShouldBeNull();
    }

    [Fact]
    public void TestSetMessage_SameKeyButDiffFormat()
    {
        //SETUP
        var stubLocalizer = new StubDefaultLocalizerWithLogging<TestStubLocalizeDefaultWithLogging>("en");

        //ATTEMPT
        var status = new StatusGenericLocalizer(stubLocalizer);
        status.AddErrorString("test".MethodLocalizeKey(this), "First Error message");
        status.AddErrorString("test".MethodLocalizeKey(this), "Second Error message");

        //VERIFY
        _output.WriteLine(stubLocalizer.PossibleError ?? "- no error -");
        stubLocalizer.PossibleError.ShouldNotBeNull();
    }
}