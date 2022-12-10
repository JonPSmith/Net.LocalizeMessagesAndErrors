// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors.UnitTestingCode;

/// <summary>
/// This stub simply returns the default message
/// </summary>
/// <typeparam name="TResource"></typeparam>
public class StubLocalizeWithDefault<TResource> : ILocalizeWithDefault<TResource>
{
    /// <summary>
    /// Returns the default string
    /// </summary>
    /// <param name="localizeKey"></param>
    /// <param name="cultureOfMessage"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public string LocalizeStringMessage(LocalizeKeyData localizeKey, string cultureOfMessage, string message)
    {
        return message;
    }

    /// <summary>
    /// Returns the default formatted string
    /// </summary>
    /// <param name="localizeKey"></param>
    /// <param name="cultureOfMessage"></param>
    /// <param name="formattableStrings"></param>
    /// <returns></returns>
    public string LocalizeFormattedMessage(LocalizeKeyData localizeKey, string cultureOfMessage,
        params FormattableString[] formattableStrings)
    {
        return string.Join(string.Empty, formattableStrings.Select(x => x.ToString()).ToArray());
    }
}