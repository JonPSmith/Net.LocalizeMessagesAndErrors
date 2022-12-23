// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors.UnitTestingCode;

/// <summary>
/// This stub simply returns the default message. Used in <see cref="DefaultLocalizerFactory"/>
/// if .NET localization is not set up. 
/// </summary>
public class StubDefaultLocalizer : IDefaultLocalizer
{
    /// <summary>
    /// This holds the <see cref="LocalizeKeyData"/> of the last localize call
    /// </summary>
    public LocalizeKeyData LastKeyData { get; set;  } 

    /// <summary>
    /// Returns the default string
    /// </summary>
    /// <param name="localizeKey"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public string LocalizeStringMessage(LocalizeKeyData localizeKey, string message)
    {
        LastKeyData = localizeKey;
        return message;
    }

    /// <summary>
    /// Returns the default formatted string
    /// </summary>
    /// <param name="localizeKey"></param>
    /// <param name="formattableStrings"></param>
    /// <returns></returns>
    public string LocalizeFormattedMessage(LocalizeKeyData localizeKey,
        params FormattableString[] formattableStrings)
    {
        LastKeyData = localizeKey;
        return string.Join(string.Empty, formattableStrings.Select(x => x.ToString()).ToArray());
    }
}