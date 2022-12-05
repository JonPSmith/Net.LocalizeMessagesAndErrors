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
        var localizeData = "test".ClassLocalizeKey(this);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual("Test.UnitTests.TestLocalizeKeyExtensions_test");
    }


    [Fact]
    public void TestClassLocalizeKey_WithClassAttributes()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassLocalizeKey(new ClassWithAttribute());

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual("UniqueClassName_test");
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
    [LocalizeSetMethodName("UniqueMethodName")]
    public void TestClassMethodMessageName_MethodAttributes()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassMethodLocalizeKey(this);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual("Test.UnitTests.TestLocalizeKeyExtensions_UniqueMethodName_test");
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
    [LocalizeSetMethodName("UniqueMethodName")]
    public void TestMethodMessageKey_MethodAttributes()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".MethodLocalizeKey(this);

        //VERIFY
        _output.WriteLine(localizeData.LocalizeKey);
        localizeData.LocalizeKey.ShouldEqual("UniqueMethodName_test");
    }

    [Fact]
    public void TestGlobalLocalizeKey()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".GlobalLocalizeKey(this);

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
    //class data

    [Fact]
    public void TestClassLocalizeKey_Logging()
    {
        //SETUP

        //ATTEMPT
        var localizeData = "test".ClassLocalizeKey(this);

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