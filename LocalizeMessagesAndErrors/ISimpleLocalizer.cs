// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Runtime.CompilerServices;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// Interface for simple localizer
/// </summary>
public interface ISimpleLocalizer
{
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
    string LocalizeString<TThis>(string message, TThis callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0);

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
    string LocalizeFormatted<TThis>(FormattableString formatted, TThis callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0);

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
    string StaticLocalizeString(string message, Type callingClassType,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0);

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
    string StaticLocalizeFormatted(FormattableString formatted, Type callingClassType,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0);
}