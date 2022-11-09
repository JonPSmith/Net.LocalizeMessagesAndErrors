﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Globalization;
using Microsoft.Extensions.Localization;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// Interface for the <see cref="LocalizeWithDefault{TResourceType}"/>
/// </summary>
/// <typeparam name="TResourceType"></typeparam>
public interface ILocalizeWithDefault<TResourceType>
{
    /// <summary>
    /// This is a localization adapter that allows you to have readable messages in your code via strings, 
    /// e.g. "This is my message".
    /// NOTE: If your message contains dynamic values then use the <see cref="LocalizeFormattedMessage"/> instead.
    /// This method will use your readable messages if the the current <see cref="CultureInfo.CurrentUICulture"/> matches
    /// the cultureOfMessage, or if <see cref="IStringLocalizer"/> service hasn't been registered.
    /// If the current <see cref="CultureInfo.CurrentUICulture"/> doesn't match the cultureOfMessage parameter,
    /// then it will use the <see cref="IStringLocalizer"/> service to try to obtain the string from the
    /// localization resources using the localizeKey parameter.
    /// If an entry is found it will build the message using the parameters in the provided readable message,
    /// but if the resource isn't found, then it will use the readable messages and log a warning that there
    /// isn't a resource with the given localizeKey / ResourcesPath. 
    /// </summary>
    /// <param name="localizeKey">This is a key for the localized message in the respective resource / culture.
    /// If null, then the message won't get localized</param>
    /// <param name="cultureOfMessage">This defines the culture of provided readable message, and if the <see cref="CultureInfo.CurrentUICulture"/>
    ///     matches, then the readable message is returned. Otherwise it will try the <see cref="IStringLocalizer"/> service (if available).
    ///     NOTE: The cultureOfMessage parameter is matched to the <see cref="CultureInfo.CurrentUICulture"/>.Name via the StartsWith method.
    ///     This means to can define a subset of the culture name, e.g. "en" would match "en-US" and "en-GB".</param>
    /// <param name="message">This contains your default message for the culture defined by the cultureOfMessage parameter.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    string LocalizeStringMessage(string localizeKey, string cultureOfMessage, string message);

    /// <summary>
    /// This is a localization adapter that allows you to have readable messages in your code using 
    /// <see cref="FormattableString"/>s to allow you to provide dynamic values in the message, e.g. $"The time is {DateTime.Now:T}"
    /// This method will use your readable messages if the the current <see cref="CultureInfo.CurrentUICulture"/> matches
    /// the cultureOfMessage, or if <see cref="IStringLocalizer"/> service hasn't been registered.
    /// If the current <see cref="CultureInfo.CurrentUICulture"/> doesn't match the cultureOfMessage parameter,
    /// then it will use the <see cref="IStringLocalizer"/> service to try to obtain the string from the
    /// localization resources using the localizeKey parameter.
    /// If an entry is found it will build the message using the parameters in the provided readable message,
    /// but if the resource isn't found, then it will use the readable messages and log a warning that there
    /// isn't a resource with the given localizeKey / ResourcesPath. 
    /// </summary>
    /// <param name="localizeKey">This is a key for the localized message in the respective resource / culture.
    /// If null, then the message won't get localized</param>
    /// <param name="cultureOfMessage">This defines the culture of provided readable message, and if the <see cref="CultureInfo.CurrentUICulture"/>
    ///     matches, then the readable message is returned. Otherwise it will try the <see cref="IStringLocalizer"/> service (if available).
    ///     NOTE: The cultureOfMessage parameter is matched to the <see cref="CultureInfo.CurrentUICulture"/>.Name via the StartsWith method.
    ///     This means to can define a subset of the culture name, e.g. "en" would match "en-US" and "en-GB".</param>
    /// <param name="formattableStrings">This is the default message for the culture defined by the cultureOfMessage parameter.
    ///     This takes one or more <see cref="FormattableString"/>s. and concatenates them into one message.
    ///     This allowed you to have multiple <see cref="FormattableString"/>s to handle long messages.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    string LocalizeFormattedMessage(string localizeKey, string cultureOfMessage,
        params FormattableString[] formattableStrings);
}