// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace LocalizeMessagesAndErrors;

public class LocalizeWithDefault<TResourceType> : ILocalizeWithDefault<TResourceType>
{
    private readonly ILogger<LocalizeWithDefault<TResourceType>> _logger;
    private readonly IStringLocalizer<TResourceType>? _localizer;


    public LocalizeWithDefault(ILogger<LocalizeWithDefault<TResourceType>> logger,
        IStringLocalizer<TResourceType>? localizer = null)
    {
        _localizer = localizer;
        _logger = logger;
    }

    /// <summary>
    /// This is a localization adapter that allows you to have readable messages in your code.
    /// e.g. $"The time is {DateTime.Now:T}"
    /// It will use your readable messages the current <see cref="CultureInfo.CurrentUICulture"/> matches
    /// the <para>cultureOfString</para>, or if <see cref="IStringLocalizer"/> service hasn't been registered.
    /// If the current <see cref="CultureInfo.CurrentUICulture"/> doesn't match the <para>cultureOfString</para>,
    /// then it will use the <see cref="IStringLocalizer"/> service to try to obtain the string from the
    /// localization resources using the <para>messageKey</para>.
    /// If an entry is found it will build the message using the parameters in the provided readable message,
    /// but if the resource isn't found, then it will use the readable messages and log a warning that there
    /// isn't a resource with the given messageKey / ResourcesPath. 
    /// </summary>
    /// <param name="messageKey">This is a key for the localized message in the respective resource / culture.</param>
    /// <param name="cultureOfString">This defines the culture of provided readable message, and if the <see cref="CultureInfo.CurrentUICulture"/>
    /// matches, then the readable message is returned. Otherwise it will try the <see cref="IStringLocalizer"/> service (if available).
    /// NOTE: The <para>cultureOfString</para> is matched to the <see cref="CultureInfo.CurrentUICulture"/>.Name via the StartsWith method.
    /// This means to can define a subset of the culture name, e.g. "en" would match "en-US" and "en-GB".</param>
    /// <param name="formattableStrings">This takes one or more <see cref="FormattableString"/>s. which contains both a readable
    /// string and the dynamic variables. You are allowed multiple <see cref="FormattableString"/>s to handle long messages.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public string LocalizeMessage(string messageKey, string cultureOfString, params FormattableString[] formattableStrings)
    {
        if (messageKey == null) throw new ArgumentNullException(nameof(messageKey));
        if (cultureOfString == null) throw new ArgumentNullException(nameof(cultureOfString));
        if (formattableStrings == null) throw new ArgumentNullException(nameof(formattableStrings));

        string ReturnGivenMessage()
        {
            return string.Join(string.Empty, formattableStrings.Select(x => x.ToString()).ToArray());
        }

        if (Thread.CurrentThread.CurrentUICulture.Name.StartsWith(cultureOfString)
            || _localizer == null)
            return ReturnGivenMessage();

        var args = formattableStrings.SelectMany(x => x.GetArguments()).ToArray();
        var foundLocalization = _localizer[messageKey];
        if (foundLocalization.ResourceNotFound)
        {
            _logger?.LogWarning("The localization resource with the name '{0}' wasn't found in the '{1}' location.",
                messageKey, foundLocalization.SearchedLocation);
            return ReturnGivenMessage();
        }

        return args.Any() ? string.Format(foundLocalization.Value, args) : foundLocalization.Value;
    }
}