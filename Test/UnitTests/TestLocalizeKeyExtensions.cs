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

    [Theory]
    [InlineData(true, "TestLocalizeKeyExtensions_test")]
    [InlineData(false, "Test.UnitTests.TestLocalizeKeyExtensions_test")]
    public void TestClassLocalizeKey_NoAttributes(bool nameIsUnique, string expectedKeyString)
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassLocalizeKey<TestLocalizeKeyExtensions>(nameIsUnique);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual(expectedKeyString);
    }


    [Theory]
    [InlineData(true, "TestLocalizeKeyExtensions_test")]
    [InlineData(false, "Test.UnitTests.TestLocalizeKeyExtensions_test")]
    public void TestClassLocalizeKey_WithClassAttributes(bool nameIsUnique, string expectedKeyString)
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassLocalizeKey<TestLocalizeKeyExtensions>(nameIsUnique);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual(expectedKeyString);
    }

    [Fact]
    public void TestClassMethodMessageName_NoAttributes()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassMethodLocalizeKey(this);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual("Test.UnitTests.TestLocalizeKeyExtensions_TestClassMethodMessageName_NoAttributes_test");
    }

    [LocalizeSetClassName("UniqueClassName")]
    private class ClassWithAttribute {}

    [Fact]
    public void TestClassMethodMessageName_ClassAttribute()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassMethodLocalizeKey(new ClassWithAttribute());

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual("UniqueClassName_TestClassMethodMessageName_ClassAttribute_test");
    }

    [Fact]
    public void TestMethodMessageKey()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".MethodLocalizeKey(this);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual("TestMethodMessageKey_test");
    }

    [Fact]
    public void TestJustThisLocalizeKey()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".JustThisLocalizeKey(this);

        //VERIFY
        localizeData.LocalizeKey.ShouldEqual("test");
    }

    [Fact]
    public void TestAlreadyLocalized()
    {
        //SETUP

        //ATTEMPT
        var localizeData = this.AlreadyLocalized();

        //VERIFY
        localizeData.LocalizeKey.ShouldBeNull();
    }

    //------------------------------------------------------------
    //Testing extensions when not in a method

    public LocalizeKeyData GetLocalised => "test".MethodLocalizeKey(this);


    [Fact]
    public void TestClassMethodLocalizeKey_InGet()
    {
        //SETUP

        //ATTEMPT
        var localizeData = GetLocalised;

        //VERIFY
        localizeData.CallingClass.ShouldEqual(GetType());
        localizeData.MethodName.ShouldEqual("GetLocalised");
        localizeData.SourceLineNumber.ShouldNotEqual(0);
    }


    public static class StaticClass
    {
        public static LocalizeKeyData TestKeyExtensions(object callerClass)
        {
            return "test".ClassLocalizeKey<TestLocalizeKeyExtensions>(true);
        }
    }


    [Fact]
    public void TestClassLocalizeKey_InStatic()
    {
        //SETUP

        //ATTEMPT
        var localizeData = StaticClass.TestKeyExtensions(this);

        //VERIFY
        localizeData.CallingClass.ShouldEqual(GetType());
        localizeData.MethodName.ShouldEqual("TestKeyExtensions");
        localizeData.SourceLineNumber.ShouldNotEqual(0);
    }
    //------------------------------------------------------------
    //class data

    [Fact]
    public void TestClassLocalizeKey_Logging()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassLocalizeKey<TestLocalizeKeyExtensions>(true);

        //VERIFY
        localizeData.CallingClass.ShouldEqual(GetType());
        localizeData.MethodName.ShouldEqual("TestClassLocalizeKey_Logging");
        localizeData.SourceLineNumber.ShouldNotEqual(0);
    }

    [Fact]
    public void TestClassMethodLocalizeKey_Logging()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassMethodLocalizeKey(this);

        //VERIFY
        localizeData.CallingClass.ShouldEqual(GetType());
        localizeData.MethodName.ShouldEqual("TestClassMethodLocalizeKey_Logging");
        localizeData.SourceLineNumber.ShouldNotEqual(0);
    }

    [Fact]
    public void TestMethodLocalizeKey_Logging()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".MethodLocalizeKey(this);

        //VERIFY
        localizeData.CallingClass.ShouldEqual(GetType());
        localizeData.MethodName.ShouldEqual("TestMethodLocalizeKey_Logging");
        localizeData.SourceLineNumber.ShouldNotEqual(0);
    }

    [Fact]
    public void TestJustThisLocalizeKey_Logging()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".JustThisLocalizeKey(this);

        //VERIFY
        localizeData.CallingClass.ShouldEqual(GetType());
        localizeData.MethodName.ShouldEqual("TestJustThisLocalizeKey_Logging");
        localizeData.SourceLineNumber.ShouldNotEqual(0);
    }

    [Fact]
    public void TestAlreadyLocalized_Logging()
    {
        //SETUP

        //ATTEMPT
        var localizeData = this.AlreadyLocalized();

        //VERIFY
        localizeData.CallingClass.ShouldEqual(GetType());
        localizeData.MethodName.ShouldEqual("TestAlreadyLocalized_Logging");
        localizeData.SourceLineNumber.ShouldNotEqual(0);
    }

}