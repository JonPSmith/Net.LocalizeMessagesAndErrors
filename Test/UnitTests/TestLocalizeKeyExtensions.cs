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

    //----------------------------------------------------
    //classes to use in tests

    private static class StaticClass { }

    [LocalizeSetClassName("UniqueClassName")]
    private class ClassWithAttribute { }

    //----------------------------------------------------

    [Theory]
    [InlineData(true, "StaticClass_test")]
    [InlineData(false, "Test.UnitTests.TestLocalizeKeyExtensions+StaticClass_test")]
    public void TestStaticClassLocalizeKey_NoAttributes(bool nameIsUnique, string expectedKeyString)
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".StaticClassLocalizeKey(typeof(StaticClass), nameIsUnique);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual(expectedKeyString);
        localizeData.CallingClass.ShouldEqual(typeof(StaticClass));
    }

    [Theory]
    [InlineData(true, "TestLocalizeKeyExtensions_test")]
    [InlineData(false, "Test.UnitTests.TestLocalizeKeyExtensions_test")]
    public void TestClassLocalizeKey_NoAttributes(bool nameIsUnique, string expectedKeyString)
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassLocalizeKey(this, nameIsUnique);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual(expectedKeyString);
        localizeData.CallingClass.ShouldEqual(typeof(TestLocalizeKeyExtensions));
    }

    [Theory]
    [InlineData(true, "ClassWithAttribute_test")]
    [InlineData(false, "UniqueClassName_test")]
    public void TestClassLocalizeKey_WithClassAttributes(bool nameIsUnique, string expectedKeyString)
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassLocalizeKey(new ClassWithAttribute(), nameIsUnique);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual(expectedKeyString);
    }

    [Theory]
    [InlineData(true, "TestLocalizeKeyExtensions_TestClassMethodMessageName_NoAttributes_test")]
    [InlineData(false, "Test.UnitTests.TestLocalizeKeyExtensions_TestClassMethodMessageName_NoAttributes_test")]
    public void TestClassMethodMessageName_NoAttributes(bool nameIsUnique, string expectedKeyString)
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassMethodLocalizeKey(this, nameIsUnique);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual(expectedKeyString);
    }

    [Theory]
    [InlineData(true, "ClassWithAttribute_TestClassMethodMessageName_ClassAttribute_test")]
    [InlineData(false, "UniqueClassName_TestClassMethodMessageName_ClassAttribute_test")]
    public void TestClassMethodMessageName_ClassAttribute(bool nameIsUnique, string expectedKeyString)
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassMethodLocalizeKey(new ClassWithAttribute(), nameIsUnique);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual(expectedKeyString);
    }

    [Theory]
    [InlineData(true, "StaticClass_TestStaticClassMethodLocalizeKey_test")]
    [InlineData(false, "Test.UnitTests.TestLocalizeKeyExtensions+StaticClass_TestStaticClassMethodLocalizeKey_test")]
    public void TestStaticClassMethodLocalizeKey(bool nameIsUnique, string expectedKeyString)
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".StaticClassMethodLocalizeKey(typeof(StaticClass), nameIsUnique);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual(expectedKeyString);
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
    public void TestStaticMethodLocalizeKey()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".StaticMethodLocalizeKey(typeof(StaticClass));

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual("TestStaticMethodLocalizeKey_test");
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
    public void TestStaticJustThisLocalizeKey()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".StaticJustThisLocalizeKey(typeof(StaticClass));

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

    [Fact]
    public void TestStaticAlreadyLocalized()
    {
        //SETUP

        //ATTEMPT
        var localizeData = typeof(StaticClass).StaticAlreadyLocalized();

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

    //------------------------------------------------------------
    //class data

    [Fact]
    public void TestClassLocalizeKey_Logging()
    {
        //SETUP

        //ATTEMPT
        var key1 = "test".ClassLocalizeKey(this, true);
        var key2 = "test".StaticClassLocalizeKey(typeof(StaticClass), true);

        //VERIFY
        key1.CallingClass.ShouldEqual(GetType());
        key1.MethodName.ShouldEqual("TestClassLocalizeKey_Logging");
        key1.SourceLineNumber.ShouldNotEqual(0);
        key2.CallingClass.ShouldEqual(typeof(StaticClass));
        key2.MethodName.ShouldEqual("TestClassLocalizeKey_Logging");
        key2.SourceLineNumber.ShouldNotEqual(key1.SourceLineNumber);
    }

    [Fact]
    public void TestClassMethodLocalizeKey_Logging()
    {
        //SETUP

        //ATTEMPT
        var key1 = "test".ClassMethodLocalizeKey(this, true);
        var key2 = "test".StaticClassMethodLocalizeKey(typeof(StaticClass), true);

        //VERIFY
        key1.CallingClass.ShouldEqual(GetType());
        key1.MethodName.ShouldEqual("TestClassMethodLocalizeKey_Logging");
        key1.SourceLineNumber.ShouldNotEqual(0);
        key2.CallingClass.ShouldEqual(typeof(StaticClass));
        key2.MethodName.ShouldEqual("TestClassMethodLocalizeKey_Logging");
        key2.SourceLineNumber.ShouldNotEqual(key1.SourceLineNumber);
    }

    [Fact]
    public void TestMethodLocalizeKey_Logging()
    {
        //SETUP

        //ATTEMPT
        var key1 = "test".MethodLocalizeKey(this);
        var key2 = "test".StaticMethodLocalizeKey(typeof(StaticClass));

        //VERIFY
        key1.CallingClass.ShouldEqual(GetType());
        key1.MethodName.ShouldEqual("TestMethodLocalizeKey_Logging");
        key1.SourceLineNumber.ShouldNotEqual(0);
        key2.CallingClass.ShouldEqual(typeof(StaticClass));
        key2.MethodName.ShouldEqual("TestMethodLocalizeKey_Logging");
        key2.SourceLineNumber.ShouldNotEqual(key1.SourceLineNumber);
    }

    [Fact]
    public void TestJustThisLocalizeKey_Logging()
    {
        //SETUP

        //ATTEMPT
        var key1 = "test".JustThisLocalizeKey(this);
        var key2 = "test".StaticJustThisLocalizeKey(typeof(StaticClass));

        //VERIFY
        key1.CallingClass.ShouldEqual(GetType());
        key1.MethodName.ShouldEqual("TestJustThisLocalizeKey_Logging");
        key1.SourceLineNumber.ShouldNotEqual(0);
        key2.CallingClass.ShouldEqual(typeof(StaticClass));
        key2.MethodName.ShouldEqual("TestJustThisLocalizeKey_Logging");
        key2.SourceLineNumber.ShouldNotEqual(key1.SourceLineNumber);
    }

    [Fact]
    public void TestAlreadyLocalized_Logging()
    {
        //SETUP

        //ATTEMPT
        var key1 = this.AlreadyLocalized();
        var key2 = typeof(StaticClass).StaticAlreadyLocalized();

        //VERIFY
        key1.CallingClass.ShouldEqual(GetType());
        key1.MethodName.ShouldEqual("TestAlreadyLocalized_Logging");
        key1.SourceLineNumber.ShouldNotEqual(0);
        key2.CallingClass.ShouldEqual(typeof(StaticClass));
        key2.MethodName.ShouldEqual("TestAlreadyLocalized_Logging");
        key2.SourceLineNumber.ShouldNotEqual(key1.SourceLineNumber);
    }

}