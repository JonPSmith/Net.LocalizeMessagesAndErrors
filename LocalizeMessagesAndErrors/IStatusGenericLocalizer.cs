// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using StatusGeneric;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// Interface for the <see cref="StatusGenericLocalizer{TResourceType}"/>
/// </summary>
public interface IStatusGenericLocalizer : IStatusGeneric
{
    /// <summary>
    /// This adds an error to the status using a string. Don't use this if you have dynamic values in the message.
    /// </summary>
    /// <param name="localizeKey">This is the key for finding the localized error in your respective resources / cultures.</param>
    /// <param name="errorMessage">The error message in the language / culture you defined when creating the
    /// StatusGenericLocalizer.</param>
    /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
    /// <returns>The StatusGenericLocalizer to allow fluent method calls.</returns>
    IStatusGeneric AddErrorString(LocalizeKeyClass localizeKey, string errorMessage,
        params string[] propertyNames);

    /// <summary>
    /// This adds an error to the status using a <see cref="FormattableString"/>s, when you don't have and properties
    /// to add to the <see cref="ValidationResult"/>. Using <see cref="FormattableString"/> allows you to place dynamic
    /// values (e.g. $"The time is {DateTime.Now:T}") that are also sent to the localizer.
    /// </summary>
    /// <param name="localizeKey">This is the key for finding the localized error in your respective resources / cultures.</param>
    /// <param name="errorMessages">The error messages in the language / culture you defined when creating the
    /// StatusGenericLocalizer. NOTE: this allows multiple <see cref="FormattableString"/>s to handle long messages.</param>
    /// <returns>The StatusGenericLocalizer to allow fluent method calls.</returns>
    IStatusGeneric AddErrorFormatted(LocalizeKeyClass localizeKey, params FormattableString[] errorMessages);

    /// <summary>
    /// This adds an error to the status using a <see cref="FormattableString"/>s, when you don't have and properties
    /// to add to the <see cref="ValidationResult"/>. Using <see cref="FormattableString"/> allows you to place dynamic
    /// values (e.g. $"The time is {DateTime.Now:T}") that are also sent to the localizer.
    /// </summary>
    /// <param name="localizeKey">This is the key for finding the localized error in your respective resources / cultures.</param>
    /// <param name="errorMessage">The error message in the language / culture you defined when creating the
    /// StatusGenericLocalizer.</param>
    /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
    /// <returns>The StatusGenericLocalizer to allow fluent method calls.</returns>
    IStatusGeneric AddErrorFormattedWithParams(LocalizeKeyClass localizeKey, FormattableString errorMessage,
        params string[] propertyNames);

    /// <summary>
    /// This adds an error to the status using a <see cref="FormattableString"/>s, when you don't have and properties
    /// to add to the <see cref="ValidationResult"/>. Using <see cref="FormattableString"/> allows you to place dynamic
    /// values (e.g. $"The time is {DateTime.Now:T}") that are also sent to the localizer.
    /// </summary>
    /// <param name="localizeKey">This is the key for finding the localized error in your respective resources / cultures.</param>
    /// <param name="errorMessages">This is an array of <see cref="FormattableString"/> containing the error message i
    /// n the language / culture you defined when creating the  StatusGenericLocalizer.
    /// NOTE: this allows multiple <see cref="FormattableString"/>s to handle long messages.</param>
    /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
    /// <returns>The StatusGenericLocalizer to allow fluent method calls.</returns>
    IStatusGeneric AddErrorFormattedWithParams(LocalizeKeyClass localizeKey,
        FormattableString[] errorMessages, params string[] propertyNames);

    /// <summary>
    /// This allows you to set the <see cref="IStatusGeneric.Message"/> with a localized string.
    /// </summary>
    /// <param name="localizeKey">This is the key for finding the localized message in your respective resources / cultures.</param>
    /// <param name="message">string that can be localized to set the <see cref="StatusGenericLocalizer{TResourceType}.Message"/> property</param>
    IStatusGeneric SetMessageString(LocalizeKeyClass localizeKey, string message);

    /// <summary>
    /// This allows you to set the <see cref="IStatusGeneric.Message"/> with localized FormattableStrings.
    /// NOTE: this allows multiple <see cref="FormattableString"/>s to handle long messages.
    /// </summary>
    /// <param name="localizeKey">This is the key for finding the localized message in your respective resources / cultures.</param>
    /// <param name="formattableStrings">A array of <see cref="FormattableString"/>s that can be localized to
    /// set the <see cref="IStatusGeneric.Message"/> property
    /// This takes one or more <see cref="FormattableString"/>s. and concatenates them into one message.
    /// This allowed you to have multiple <see cref="FormattableString"/>s to handle long messages.
    /// </param>
    IStatusGeneric SetMessageFormatted(LocalizeKeyClass localizeKey, params FormattableString[] formattableStrings);
}