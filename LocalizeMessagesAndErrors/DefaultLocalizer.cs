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
public class DefaultLocalizer<TResource> : IDefaultLocalizer<TResource>, IDefaultLocalizerForSimpleLocalizer
{
    private readonly DefaultLocalizerOptions _options;
    private readonly IStringLocalizer<TResource> _localizer;
    private readonly ILogger<DefaultLocalizer<TResource>> _logger;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="options"></param>
    /// <param name="logger">logger to report issues when using the <see cref="IStringLocalizer"/> service. Can be null for unit tests.</param>
    /// <param name="localizer">Optional: If no <see cref="IStringLocalizer"/> service, then readable string used.</param>
    public DefaultLocalizer(DefaultLocalizerOptions options,
        ILogger<DefaultLocalizer<TResource>> logger,
        IStringLocalizer<TResource> localizer = null)
    {
        if (string.IsNullOrWhiteSpace(options.DefaultCulture))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(options.DefaultCulture));
        _localizer = localizer;
        _options = options;
        _logger = logger;
    }

    /// <summary>
    /// This is a localization adapter that allows you to have readable messages in your code via strings, 
    /// e.g. "This is my message".
    /// </summary>
    /// <param name="localizeData">This contains the localizeData and the calling class to log errors with a logger containing the called class.</param>
    /// <param name="message">This contains your default message for the culture defined by the cultureOfMessage parameter.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public string LocalizeStringMessage(LocalizeKeyData localizeData, string message)
    {
        if (localizeData == null) throw new ArgumentNullException(nameof(localizeData));
        if (message == null) throw new ArgumentNullException(nameof(message));

        if (_options.CultureMatches()
             || localizeData.LocalizeKey == null
             || _localizer == null)
            //Return given message if
            //a) the CurrentUICulture matches the options.DefaultCulture, i.e. the message is already in the correct language
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
    /// <param name="formattableStrings">This is the default message for the culture defined by the cultureOfMessage parameter.
    ///     This takes one or more <see cref="FormattableString"/>s. and concatenates them into one message.
    ///     This allowed you to have multiple <see cref="FormattableString"/>s to handle long messages.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public string LocalizeFormattedMessage(LocalizeKeyData localizeData,
        params FormattableString[] formattableStrings)
    {
        if (localizeData == null) throw new ArgumentNullException(nameof(localizeData));
        if (formattableStrings == null) throw new ArgumentNullException(nameof(formattableStrings));

        string ReturnGivenMessage()
        {
            return string.Join(string.Empty, formattableStrings.Select(x => x.ToString()).ToArray());
        }

        if (_options.CultureMatches()
            || localizeData.LocalizeKey == null
            || _localizer == null)
            //Return given message if
            //a) the CurrentUICulture matches the options.DefaultCulture, i.e. the message is already in the correct language
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