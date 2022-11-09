﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This provides a service that allows you to put readable messages in your code,
/// while handling multiple languages. It uses your readable message if its culture
/// matches the user's culture, otherwise it will use <see cref="IStringLocalizer{TResourceType}"/>
/// to obtain the culture via resource files.
/// </summary>
/// <typeparam name="TResourceType"></typeparam>
public class LocalizeWithDefault<TResourceType> : ILocalizeWithDefault<TResourceType>
{
    private readonly IStringLocalizer<TResourceType> _localizer;
    private readonly ILogger<LocalizeWithDefault<TResourceType>> _logger;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="logger">_logger to report issues when using the <see cref="IStringLocalizer"/> service. Can be null for unit tests.</param>
    /// <param name="localizer">Optional: If no <see cref="IStringLocalizer"/> service, then readable string used.</param>
    public LocalizeWithDefault(ILogger<LocalizeWithDefault<TResourceType>> logger,
        IStringLocalizer<TResourceType> localizer = null)
    {
        _localizer = localizer;
        _logger = logger;
    }

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
    public string LocalizeStringMessage(string localizeKey, string cultureOfMessage, string message)
    {
        if (cultureOfMessage == null) throw new ArgumentNullException(nameof(cultureOfMessage));
        if (message == null) throw new ArgumentNullException(nameof(message));

        if (localizeKey == null)
            //we assume that not setting the localizeKey means you don't want the message localized 
            return message;

        if (Thread.CurrentThread.CurrentUICulture.Name.StartsWith(cultureOfMessage)
            || _localizer == null)
            return message;

        var foundLocalization = _localizer[localizeKey];
        if (!foundLocalization.ResourceNotFound) 
            return foundLocalization.Value;

        //Entry not found in the resources, so log this and return the given message
        _logger?.LogWarning("The entry with the name '{0}' and culture of '{1}' was not found in the '{2}' resource.",
            localizeKey, foundLocalization.SearchedLocation, Thread.CurrentThread.CurrentUICulture.Name);
        return message;

    }

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
    public string LocalizeFormattedMessage(string localizeKey, string cultureOfMessage,
        params FormattableString[] formattableStrings)
    {
        string ReturnGivenMessage()
        {
            return string.Join(string.Empty, formattableStrings.Select(x => x.ToString()).ToArray());
        }

        if (cultureOfMessage == null) throw new ArgumentNullException(nameof(cultureOfMessage));
        if (formattableStrings == null) throw new ArgumentNullException(nameof(formattableStrings));

        if (localizeKey == null)
            //we assume that not setting the localizeKey means you don't want the message localized 
            return ReturnGivenMessage();

        if (Thread.CurrentThread.CurrentUICulture.Name.StartsWith(cultureOfMessage)
            || _localizer == null)
            return ReturnGivenMessage();

        var args = formattableStrings.SelectMany(x => x.GetArguments()).ToArray();
        var foundLocalization = _localizer[localizeKey];
        if (!foundLocalization.ResourceNotFound)
            try
            {
                return string.Format(foundLocalization.Value, args);
            }
            catch (FormatException e)
            {
                _logger?.LogError(e, "The resourced string '{0}' had the following FormatException error: {1}.",
                    foundLocalization.Value, e.Message);
                return ReturnGivenMessage();
            }

        //Entry not found in the resources, so log this and return the given message
        _logger?.LogWarning("The entry with the name '{0}' and culture of '{1}' was not found in the '{2}' resource.",
            localizeKey, Thread.CurrentThread.CurrentUICulture.Name, foundLocalization.SearchedLocation);
        return ReturnGivenMessage();
    }
}