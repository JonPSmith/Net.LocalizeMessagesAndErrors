// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using LocalizeMessagesAndErrors;

namespace Test.StubClasses;

/// <summary>
/// This simply returns the default message
/// </summary>
/// <typeparam name="TResource"></typeparam>
public class StubLocalizeWithDefault<TResource> : ILocalizeWithDefault<TResource>
{

    public string LocalizeStringMessage(LocalizeKeyData localizeKey, string cultureOfMessage, string message)
    {
        return message;
    }

    public string LocalizeFormattedMessage(LocalizeKeyData localizeKey, string cultureOfMessage,
        params FormattableString[] formattableStrings)
    {
        return string.Join(string.Empty, formattableStrings.Select(x => x.ToString()).ToArray());
    }
}