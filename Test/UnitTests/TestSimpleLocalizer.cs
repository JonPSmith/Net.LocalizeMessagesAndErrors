// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using LocalizeMessagesAndErrors;
using LocalizeMessagesAndErrors.UnitTestingCode;
using Microsoft.Extensions.DependencyInjection;
using System;
using Test.StubClasses;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests;

public class TestSimpleLocalizer
{
    private readonly ISimpleLocalizer _simpleLoc;
    private readonly StubDefaultLocalizer _stubDefaultLoc;

    public TestSimpleLocalizer()
    {
        _stubDefaultLoc = new StubDefaultLocalizer();
        var stubDefLocFactory = new StubDefaultLocalizerFactory(_stubDefaultLoc);
        _simpleLoc = new SimpleLocalizer(stubDefLocFactory, new StubSimpleLocalizerOptions<TestSimpleLocalizer>());
    }

    [Fact]
    public void TestCreateSimpleLocalizerService()
    {
        //SETUP

        //ATTEMPT

        //VERIFY
        _simpleLoc.ShouldNotBeNull();
        _stubDefaultLoc.ShouldNotBeNull();
        _stubDefaultLoc.LastKeyData.ShouldBeNull();
    }

    [Fact]
    public void TestLocalizeString()
    {
        //SETUP

        //ATTEMPT
        var message = _simpleLoc.LocalizeString("My message", this);

        //VERIFY
        message.ShouldEqual("My message");
        _stubDefaultLoc.LastKeyData.LocalizeKey.ShouldEqual("SimpleLocalizer(My message)");
        _stubDefaultLoc.LastKeyData.CallingClass.Name.ShouldEqual("TestSimpleLocalizer");
    }

    [Fact]
    public void TestLocalizeFormatted()
    {
        //SETUP

        //ATTEMPT
        var message = _simpleLoc.LocalizeFormatted($"Date is {DateTime.Now:M}.", this);

        //VERIFY
        message.ShouldEqual($"Date is {DateTime.Now:M}.");
        _stubDefaultLoc.LastKeyData.LocalizeKey.ShouldEqual("SimpleLocalizer(Date is {0:M}.)");
        _stubDefaultLoc.LastKeyData.CallingClass.Name.ShouldEqual("TestSimpleLocalizer");
    }

    private static class MyStaticClass { }

    [Fact]
    public void TestStaticLocalizeString()
    {
        //SETUP

        //ATTEMPT
        var message = _simpleLoc.StaticLocalizeString("My message", typeof(MyStaticClass));

        //VERIFY
        message.ShouldEqual("My message");
        _stubDefaultLoc.LastKeyData.LocalizeKey.ShouldEqual("SimpleLocalizer(My message)");
        _stubDefaultLoc.LastKeyData.CallingClass.Name.ShouldEqual("MyStaticClass");
    }

    [Fact]
    public void TestStaticLocalizeFormatted()
    {
        //SETUP

        //ATTEMPT
        var message = _simpleLoc.StaticLocalizeFormatted($"My {123} message", typeof(MyStaticClass));

        //VERIFY
        message.ShouldEqual("My 123 message");
        _stubDefaultLoc.LastKeyData.LocalizeKey.ShouldEqual("SimpleLocalizer(My {0} message)");
        _stubDefaultLoc.LastKeyData.CallingClass.Name.ShouldEqual("MyStaticClass");
    }

    [Fact]
    public void TestCheckDifferentPrefix()
    {
        //SETUP
        var stubDefaultLoc = new StubDefaultLocalizer();
        var stubDefLocFactory = new StubDefaultLocalizerFactory(stubDefaultLoc);
        var simpleLoc = new SimpleLocalizer(stubDefLocFactory,
            new StubSimpleLocalizerOptions<TestSimpleLocalizer> { PrefixKeyString = "XXX" });

        //ATTEMPT
        var message = simpleLoc.LocalizeString("My message", this);

        //VERIFY
        message.ShouldEqual("My message");
        stubDefaultLoc.LastKeyData.LocalizeKey.ShouldEqual("XXX(My message)");
        stubDefaultLoc.LastKeyData.CallingClass.Name.ShouldEqual("TestSimpleLocalizer");
    }
}