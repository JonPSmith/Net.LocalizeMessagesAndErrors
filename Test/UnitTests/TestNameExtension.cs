// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using LocalizeMessagesAndErrors;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Test.UnitTests;

public class TestNameExtension
{
    [Theory]
    [InlineData("Ok", "Ok")]
    [InlineData("ok", "Ok")]
    [InlineData("", "")]
    public void TestCamelToPascal(string input, string expectedResult)
    {
        //SETUP

        //ATTEMPT
        var result = input.CamelToPascal();

        //VERIFY
        result.ShouldEqual(expectedResult);
    }
}