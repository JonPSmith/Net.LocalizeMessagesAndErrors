// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using LocalizeMessagesAndErrors;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests;

public class TestLocalizeKeyExtensions
{
    private readonly ITestOutputHelper _output;

    public TestLocalizeKeyExtensions(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestClassMethodMessageName_NoAttributes()
    {
        //SETUP

        //ATTEMPT
        var messageName = "test".ClassMethodMessageName(this);

        //VERIFY
        _output.WriteLine(messageName);
        messageName.ShouldEqual("Test.UnitTests.TestLocalizeKeyExtensions_TestClassMethodMessageName_NoAttributes_test");
    }

    [LocalizeSetClassName("UniqueClassName")]
    private class ClassWithAttribute {}

    [Fact]
    public void TestClassMethodMessageName_ClassAttribute()
    {
        //SETUP

        //ATTEMPT
        var messageName = "test".ClassMethodMessageName(new ClassWithAttribute());

        //VERIFY
        _output.WriteLine(messageName);
        messageName.ShouldEqual("UniqueClassName_TestClassMethodMessageName_ClassAttribute_test");
    }

    [Fact]
    [LocalizeSetMethodName("UniqueMethodName")]
    public void TestClassMethodMessageName_MethodAttributes()
    {
        //SETUP

        //ATTEMPT
        var messageName = "test".ClassMethodMessageName(this);

        //VERIFY
        _output.WriteLine(messageName);
        messageName.ShouldEqual("Test.UnitTests.TestLocalizeKeyExtensions_UniqueMethodName_test");
    }
}