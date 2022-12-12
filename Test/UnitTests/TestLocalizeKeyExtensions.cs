﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
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
        var localizeData = "test".ClassLocalizeKey(this, nameIsUnique);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual(expectedKeyString);
    }

    [LocalizeSetClassName("UniqueClassName")]
    private class ClassWithAttribute { }

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


    private static class StaticClass
    {
        public static LocalizeKeyData TestStaticKey(bool addClassPart, bool addMethodPart, bool nameIsUnique)
        {
            return "test".LocalizeKeyBuilder(typeof(StaticClass), addClassPart, addMethodPart, nameIsUnique);
        }
    }

    [Theory]
    [InlineData(false, false, false, "test")]
    [InlineData(true, false, false, "Test.UnitTests.TestLocalizeKeyExtensions+StaticClass_test")]
    [InlineData(true, false, true, "StaticClass_test")]
    [InlineData(true, true, false, "Test.UnitTests.TestLocalizeKeyExtensions+StaticClass_TestStaticKey_test")]
    [InlineData(true, true, true, "StaticClass_TestStaticKey_test")]
    public void TestLocalizeKeyBuilder_InStatic(bool addClassPart, bool addMethodPart, bool nameIsUnique, string expectedKey)
    {
        //SETUP

        //ATTEMPT
        var localizeData = StaticClass.TestStaticKey(addClassPart, addMethodPart, nameIsUnique);

        //VERIFY
        localizeData.LocalizeKey.ShouldEqual(expectedKey);
    }
    //------------------------------------------------------------
    //class data

    [Fact]
    public void TestClassLocalizeKey_Logging()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassLocalizeKey(this, true);

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
        var localizeData = "test".ClassMethodLocalizeKey(this, true);

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