// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This is the interface for the <see cref="DefaultLocalizer{TResource}"/> service
/// when used in <see cref="IDefaultLocalizerFactory"/>
/// </summary>
public interface IDefaultLocalizer
{
    /// <summary>
    /// This is a localization adapter that allows you to have readable messages in your code via strings, 
    /// e.g. "This is my message".
    /// </summary>
    /// <param name="localizeKey">This contains the localizeData and the calling class to log errors with a logger containing the called class.</param>
    /// <param name="message">This contains your default message for the culture defined in the <see cref="DefaultLocalizerOptions"/>.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    string LocalizeStringMessage(LocalizeKeyData localizeKey, string message);

    /// <summary>
    /// This is a localization adapter that allows you to have readable messages in your code using 
    /// <see cref="FormattableString"/>s to allow you to provide dynamic values in the message, e.g. $"The time is {DateTime.Now:T}"
    /// </summary>
    /// <param name="localizeKey">This is the key for finding the localized message in your respective resources / cultures.</param>
    /// <param name="formattableStrings">This is the default message for the culture defined in the <see cref="DefaultLocalizerOptions"/>.
    ///     This takes one or more <see cref="FormattableString"/>s. and concatenates them into one message.
    ///     This allowed you to have multiple <see cref="FormattableString"/>s to handle long messages.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    string LocalizeFormattedMessage(LocalizeKeyData localizeKey, params FormattableString[] formattableStrings);
}