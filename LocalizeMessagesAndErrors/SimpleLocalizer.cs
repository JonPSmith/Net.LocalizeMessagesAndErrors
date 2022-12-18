// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Runtime.CompilerServices;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This is a simple localizer to handle simple messages, such as button names etc.
/// It uses the message as part of the localize key.
/// The limitation is this localizer are:
/// 1. It only uses the one resource file for all of the localizations, which you define on registration.
/// 2. The localize key is based on the message, i.e. {<see cref="SimpleLocalizer"/>}_{message} 
/// </summary>
public class SimpleLocalizer : ISimpleLocalizer
{
    private readonly IDefaultLocalizeForSimpleLocalizer _localizeDefault;
    private readonly string _cultureOfMessage;

    /// <summary>
    /// This ctor will create the <see cref="IDefaultLocalizer{TResource}"/> service
    /// using the resourceType that you provide.
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="resourceType"></param>
    /// <param name="cultureOfMessage"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SimpleLocalizer(IServiceProvider provider, Type resourceType, string cultureOfMessage)
    {
        if (resourceType == null) throw new ArgumentNullException(nameof(resourceType));
        if (string.IsNullOrWhiteSpace(cultureOfMessage))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(cultureOfMessage));
        _cultureOfMessage = cultureOfMessage;

        var myGeneric = typeof(IDefaultLocalizer<>);
        var genericType = myGeneric.MakeGenericType(resourceType);
        _localizeDefault = (IDefaultLocalizeForSimpleLocalizer) provider.GetService(genericType);
    }

    /// <summary>
    /// This provides the given message based on the culture give when registering the service
    /// If the current culture matches the register's culture, then the given message is returned.
    /// If the current culture does NOT match the register's culture, then it will look up
    /// in the resource file defined by the resource type give when registering the service
    /// using a localize key in the form {<see cref="SimpleLocalizer"/>}_{message} 
    /// </summary>
    /// <param name="message">The message to show when the app's culture matches </param>
    /// <param name="callingClass">The type of the class / struct you are calling from.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>localized string</returns>
    public string LocalizeString<TThis>(string message, TThis callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return _localizeDefault.LocalizeStringMessage(
            $"{nameof(SimpleLocalizer)}_{message}".JustThisLocalizeKey(callingClass, memberName, sourceLineNumber),
            _cultureOfMessage, message);
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
    public string LocalizeFormatted<TThis>(FormattableString formatted, TThis callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return _localizeDefault.LocalizeFormattedMessage(
            $"{nameof(SimpleLocalizer)}_{formatted.Format}"
            .JustThisLocalizeKey(callingClass, memberName, sourceLineNumber),
            _cultureOfMessage, formatted);
    }

    /// <summary>
    /// This is useful when working in static methods or where it hard to obtain this.
    /// It provides the given message based on the culture give when registering the service
    /// If the current culture matches the register's culture, then the given message is returned.
    /// If the current culture does NOT match the register's culture, then it will look up
    /// in the resource file defined by the resource type give when registering the service
    /// using a localize key in the form {<see cref="SimpleLocalizer"/>}_{message} 
    /// </summary>
    /// <param name="message">The message to show when the app's culture matches </param>
    /// <param name="callingClassType">The type of the class / struct you are calling from</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>localized string</returns>
    public string StaticLocalizeString(string message, Type callingClassType,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return _localizeDefault.LocalizeStringMessage(
            $"{nameof(SimpleLocalizer)}_{message}".StaticJustThisLocalizeKey(callingClassType, memberName, sourceLineNumber),
            _cultureOfMessage, message);
    }

    /// <summary>
    /// This is useful when working in static methods or where it hard to obtain this.
    /// It provides the given message based on the culture give when registering the service
    /// If the current culture matches the register's culture, then the given message is returned.
    /// If the current culture does NOT match the register's culture, then it will look up
    /// in the resource file defined by the resource type give when registering the service
    /// using a localize key in the form {<see cref="SimpleLocalizer"/>}_{message} 
    /// </summary>
    /// <param name="formatted">The formattable message to show when the app's culture matches </param>
    /// <param name="callingClassType">The type of the class / struct you are calling from</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>localized string</returns>
    public string StaticLocalizeFormatted(FormattableString formatted, Type callingClassType,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return _localizeDefault.LocalizeFormattedMessage(
            $"{nameof(SimpleLocalizer)}_{formatted.Format}".StaticJustThisLocalizeKey(callingClassType, memberName, sourceLineNumber),
            _cultureOfMessage, formatted);
    }
}