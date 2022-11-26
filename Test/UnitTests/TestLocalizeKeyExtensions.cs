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
    public void TestClassLocalizeKey_NoAttributes()
    {
        //SETUP

        //ATTEMPT
        var localizeKey = "test".ClassLocalizeKey(this);

        //VERIFY
        _output.WriteLine(localizeKey.ToString());
        localizeKey.ToString().ShouldEqual("Test.UnitTests.TestLocalizeKeyExtensions_test");
    }


    [Fact]
    public void TestClassLocalizeKey_WithClassAttributes()
    {
        //SETUP

        //ATTEMPT
        var localizeKey = "test".ClassLocalizeKey(new ClassWithAttribute());

        //VERIFY
        _output.WriteLine(localizeKey.ToString());
        localizeKey.ToString().ShouldEqual("UniqueClassName_test");
    }

    [Fact]
    public void TestClassMethodMessageName_NoAttributes()
    {
        //SETUP

        //ATTEMPT
        var localizeKey = "test".ClassMethodLocalizeKey(this);

        //VERIFY
        _output.WriteLine(localizeKey.ToString());
        localizeKey.ToString().ShouldEqual("Test.UnitTests.TestLocalizeKeyExtensions_TestClassMethodMessageName_NoAttributes_test");
    }

    [LocalizeSetClassName("UniqueClassName")]
    private class ClassWithAttribute {}

    [Fact]
    public void TestClassMethodMessageName_ClassAttribute()
    {
        //SETUP

        //ATTEMPT
        var localizeKey = "test".ClassMethodLocalizeKey(new ClassWithAttribute());

        //VERIFY
        _output.WriteLine(localizeKey.ToString());
        localizeKey.ToString().ShouldEqual("UniqueClassName_TestClassMethodMessageName_ClassAttribute_test");
    }

    [Fact]
    [LocalizeSetMethodName("UniqueMethodName")]
    public void TestClassMethodMessageName_MethodAttributes()
    {
        //SETUP

        //ATTEMPT
        var localizeKey = "test".ClassMethodLocalizeKey(this);

        //VERIFY
        _output.WriteLine(localizeKey.ToString());
        localizeKey.ToString().ShouldEqual("Test.UnitTests.TestLocalizeKeyExtensions_UniqueMethodName_test");
    }
    
    [Fact]
    public void TestMethodMessageKey()
    {
        //SETUP

        //ATTEMPT
        var localizeKey = "test".MethodLocalizeKey(this);

        //VERIFY
        _output.WriteLine(localizeKey.ToString());
        localizeKey.ToString().ShouldEqual("TestMethodMessageKey_test");
    }

    [Fact]
    public void TestAlreadyLocalized()
    {
        //SETUP

        //ATTEMPT
        var localizeKey = LocalizeKeyExtensions.AlreadyLocalized();

        //VERIFY
        localizeKey.ToString().ShouldBeNull();
    }
}