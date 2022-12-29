// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.


namespace LocalizeMessagesAndErrors.UnitTestingCode;

/// <summary>
/// This stub simply returns the default message. Used in <see cref="SimpleLocalizerFactory"/>
/// if .NET localization is not set up and in unit testing.
/// </summary>
public class StubSimpleLocalizer : ISimpleLocalizer
{
    public StubSimpleLocalizer(string prefixKeyString = "SimpleLocalizer")
    {
        PrefixKeyString = prefixKeyString;
    }

    /// <summary>
    /// This holds a string which prefixes the localize key, i.e. $"{PrefixKeyString}({message})"
    /// If null, then just the message is used as the localize key, i.e. "message"
    /// </summary>
    private string PrefixKeyString { get; set; } 

    /// <summary>
    /// This holds the <see cref="LocalizeKeyData"/> of the last localize call
    /// </summary>
    public LocalizeKeyData LastKeyData { get; set; }

    /// <summary>
    /// This provides the given message based on the culture give when registering the service
    /// If the current culture matches the register's culture, then the given message is returned.
    /// If the current culture does NOT match the register's culture,  then it will look up
    /// in the resource file defined by the resource type give when registering the service
    /// using a localize key in the form {<see cref="SimpleLocalizer"/>}_{message} 
    /// </summary>
    /// <param name="message">The message to show when the app's culture matches </param>
    /// <param name="callingClass">The type of the class / struct you are calling from.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>localized string</returns>
    public string LocalizeString<TThis>(string message, TThis callingClass, string memberName = "", int sourceLineNumber = 0)
    {
        LastKeyData = new LocalizeKeyData(FormLocalizeKey(message), callingClass.GetType(),
            memberName, sourceLineNumber);
        return message;
    }

    /// <summary>
    /// This provides the given message based on the culture give when registering the service
    /// If the current culture matches the register's culture, then the given message is returned.
    /// If the current culture does NOT match the register's culture,  then it will look up
    /// in the resource file defined by the resource type give when registering the service
    /// using a localize key in the form {<see cref="SimpleLocalizer"/>}_{message} 
    /// </summary>
    /// <param name="formatted">The formattable message to show when the app's culture matches </param>
    /// <param name="callingClass">The type of the class / struct you are calling from.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>localized string</returns>
    public string LocalizeFormatted<TThis>(FormattableString formatted, TThis callingClass, string memberName = "",
        int sourceLineNumber = 0)
    {
        LastKeyData = new LocalizeKeyData(FormLocalizeKey(formatted.ToString()), 
            callingClass.GetType(), memberName, sourceLineNumber);
        return formatted.ToString();
    }

    /// <summary>
    /// This is useful when working in static methods or where it hard to obtain this.
    /// It provides the given message based on the culture give when registering the service
    /// If the current culture matches the register's culture, then the given message is returned.
    /// If the current culture does NOT match the register's culture,  then it will look up
    /// in the resource file defined by the resource type give when registering the service
    /// using a localize key in the form {<see cref="SimpleLocalizer"/>}_{message} 
    /// </summary>
    /// <param name="message">The message to show when the app's culture matches </param>
    /// <param name="callingClassType">The type of the class / struct you are calling from</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>localized string</returns>
    public string StaticLocalizeString(string message, Type callingClassType, string memberName = "", int sourceLineNumber = 0)
    {
        LastKeyData = new LocalizeKeyData(FormLocalizeKey(message), callingClassType,
            memberName, sourceLineNumber);
        return message;
    }

    /// <summary>
    /// This is useful when working in static methods or where it hard to obtain this.
    /// It provides the given message based on the culture give when registering the service
    /// If the current culture matches the register's culture, then the given message is returned.
    /// If the current culture does NOT match the register's culture,  then it will look up
    /// in the resource file defined by the resource type give when registering the service
    /// using a localize key in the form {<see cref="SimpleLocalizer"/>}_{message} 
    /// </summary>
    /// <param name="formatted">The formattable message to show when the app's culture matches </param>
    /// <param name="callingClassType">The type of the class / struct you are calling from</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>localized string</returns>
    public string StaticLocalizeFormatted(FormattableString formatted, Type callingClassType, string memberName = "",
        int sourceLineNumber = 0)
    {
        LastKeyData = new LocalizeKeyData(FormLocalizeKey(formatted.ToString()),
            callingClassType, memberName, sourceLineNumber);
        return formatted.ToString();
    }

    private string FormLocalizeKey(string message)
    {
        return PrefixKeyString == null
            ? message
            : $"{PrefixKeyString}({message})";
    }
}