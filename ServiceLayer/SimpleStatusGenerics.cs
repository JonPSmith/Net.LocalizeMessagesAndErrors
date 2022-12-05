// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using LocalizedWebApp.Controllers;
using LocalizeMessagesAndErrors;
using StatusGeneric;

namespace ServiceLayer;

public class SimpleStatusGenerics
{
    private readonly ILocalizeWithDefault<HomeController> _localizeDefault;

    public SimpleStatusGenerics(ILocalizeWithDefault<HomeController> localizeDefault)
    {
        _localizeDefault = localizeDefault;
    }

    /// <summary>
    /// This the basic 
    /// </summary>
    /// <param name="someString"></param>
    /// <returns></returns>
    public IStatusGeneric BasicUsage(string? someString)
    {
        var status = new StatusGenericLocalizer<HomeController>("en", _localizeDefault);

        //add error and return immediately
        if (someString == null)
            //You can return just an error message, but adding the property name
            //will improve the error feedback in ASP.NET Core etc.
            return status.AddErrorString("NullParam".GlobalLocalizeKey(this), 
                "input must not be null", nameof(someString));

        //This 
        status.SetMessageString("Success".MethodLocalizeKey(this), "That went well");

        //If no errors were added then its returns a IsValid status with the message you set.
        //If there are errors then the Message says something like "Failed with 1 error".
        //HasErrors will be true and IsValid will be false if there are errors, otherwise opposite
        return status;
    }
}