// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This provides adapter that allows you to put readable messages in your code,
/// while handling multiple languages. It uses your readable message if its culture
/// matches the user's culture, otherwise it will use <see cref="IStringLocalizer{TResource}"/>
/// to obtain the culture via resource files.
/// </summary>
/// <typeparam name="TResource"></typeparam>
public class LocalizeWithDefault<TResource> : ILocalizeWithDefault<TResource>
{
    private readonly IStringLocalizer<TResource> _localizer;
    private readonly ILogger<LocalizeWithDefault<TResource>> _logger;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="logger">_logger to report issues when using the <see cref="IStringLocalizer"/> service. Can be null for unit tests.</param>
    /// <param name="localizer">Optional: If no <see cref="IStringLocalizer"/> service, then readable string used.</param>
    public LocalizeWithDefault(ILogger<LocalizeWithDefault<TResource>> logger,
        IStringLocalizer<TResource> localizer = null)
    {
        _localizer = localizer;
        _logger = logger;
    }

    /// <summary>
    /// This is a localization adapter that allows you to have readable messages in your code via strings, 
    /// e.g. "This is my message".
    /// </summary>
    /// <param name="localizeData">This contains the localizeData and the calling class to log errors with a logger containing the called class.</param>
    /// <param name="cultureOfMessage">This defines the culture of provided readable message, and if the <see cref="CultureInfo.CurrentUICulture"/>
    ///     matches, then the readable message is returned. Otherwise it will try the <see cref="IStringLocalizer"/> service (if available).
    ///     NOTE: The cultureOfMessage parameter is matched to the <see cref="CultureInfo.CurrentUICulture"/>.Name via the StartsWith method.
    ///     This means to can define a subset of the culture name, e.g. "en" would match "en-US" and "en-GB".</param>
    /// <param name="message">This contains your default message for the culture defined by the cultureOfMessage parameter.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public string LocalizeStringMessage(LocalizeKeyData localizeData, string cultureOfMessage, string message)
    {
        if (localizeData == null) throw new ArgumentNullException(nameof(localizeData));
        if (cultureOfMessage == null) throw new ArgumentNullException(nameof(cultureOfMessage));
        if (message == null) throw new ArgumentNullException(nameof(message));

        if (Thread.CurrentThread.CurrentUICulture.Name.StartsWith(cultureOfMessage)
            || localizeData.LocalizeKey == null
            || _localizer == null)
            //Return given message if
            //a) the CurrentUICulture starts with the cultureOfMessage parameter, i.e. the message is already in the correct language
            //b) The localizeData is null, which means the message has already been localized.
            //c) if there is no localizer service, then we assume the application isn't (yet) using localization. 
            return message;

        var foundLocalization = _localizer[localizeData.LocalizeKey];
        if (!foundLocalization.ResourceNotFound)
            return foundLocalization.Value;

        LogWarningOnMissingResource(localizeData, foundLocalization);
        return message;
    }

    /// <summary>
    /// This is a localization adapter that allows you to have readable messages in your code using 
    /// <see cref="FormattableString"/>s to allow you to provide dynamic values in the message, e.g. $"The time is {DateTime.Now:T}"
    /// </summary>
    /// <param name="localizeData">This is the key for finding the localized message in your respective resources / cultures.</param>
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
    public string LocalizeFormattedMessage(LocalizeKeyData localizeData, string cultureOfMessage,
        params FormattableString[] formattableStrings)
    {
        if (localizeData == null) throw new ArgumentNullException(nameof(localizeData));
        if (cultureOfMessage == null) throw new ArgumentNullException(nameof(cultureOfMessage));
        if (formattableStrings == null) throw new ArgumentNullException(nameof(formattableStrings));

        string ReturnGivenMessage()
        {
            return string.Join(string.Empty, formattableStrings.Select(x => x.ToString()).ToArray());
        }

        if (Thread.CurrentThread.CurrentUICulture.Name.StartsWith(cultureOfMessage)
            || localizeData.LocalizeKey == null
            || _localizer == null)
            //Return given message if
            //a) the CurrentUICulture starts with the cultureOfMessage parameter, i.e. the message is already in the correct language
            //b) The localizeData is null, which means the message has already been localized.
            //c) if there is no localizer service, then we assume the application isn't (yet) using localization. 
            return ReturnGivenMessage();

        var args = formattableStrings.SelectMany(x => x.GetArguments()).ToArray();
        var foundLocalization = _localizer[localizeData.LocalizeKey];
        if (!foundLocalization.ResourceNotFound)
            try
            {
                return string.Format(foundLocalization.Value, args);
            }
            catch (FormatException e)
            {
                _logger?.LogError(e,
                    "The resourced string '{0}' had the following FormatException error: {1}. The message came from {2}.",
                    foundLocalization.Value, e.Message,
                    $"{localizeData.CallingClass.Name}.{localizeData.MethodName}, line {localizeData.SourceLineNumber}");
                return ReturnGivenMessage();
            }

        LogWarningOnMissingResource(localizeData, foundLocalization);
        return ReturnGivenMessage();
    }

    private void LogWarningOnMissingResource(LocalizeKeyData localizeData, LocalizedString foundLocalization)
    {
        //Entry not found in the resources, so log this and return the given message
        _logger?.LogWarning(
            "The message with the localizeKey name of '{0}' and culture of '{1}' was not found in the '{2}' resource. " +
            "The message came from {3}.",
            localizeData.LocalizeKey, Thread.CurrentThread.CurrentUICulture.Name,
            foundLocalization.SearchedLocation,
            $"{localizeData.CallingClass.Name}.{localizeData.MethodName}, line {localizeData.SourceLineNumber}");
    }
}