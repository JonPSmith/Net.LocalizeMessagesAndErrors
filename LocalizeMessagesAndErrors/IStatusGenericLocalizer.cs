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
    /// <param name="localizeKey">This is a key for the localized message in the respective resource / culture.
    /// If null, then the message won't get localized</param>
    /// <param name="errorMessage">The error message in the language / culture you defined when creating the
    /// StatusGenericLocalizer.</param>
    /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
    /// <returns>The StatusGenericLocalizer to allow fluent method calls.</returns>
    IStatusGenericLocalizer AddErrorString(string? localizeKey, string errorMessage, params string[] propertyNames);

    /// <summary>
    /// This adds an error to the status using a <see cref="FormattableString"/>s, when you don't have and properties
    /// to add to the <see cref="ValidationResult"/>. Using <see cref="FormattableString"/> allows you to place dynamic
    /// values (e.g. $"The time is {DateTime.Now:T}") that are also sent to the localizer.
    /// </summary>
    /// <param name="localizeKey">This is a key for the localized message in the respective resource / culture.
    /// If null, then the message won't get localized</param>
    /// <param name="errorMessages">The error messages in the language / culture you defined when creating the
    /// StatusGenericLocalizer. NOTE: this allows multiple <see cref="FormattableString"/>s to handle long messages.</param>
    /// <returns>The StatusGenericLocalizer to allow fluent method calls.</returns>
    IStatusGenericLocalizer AddErrorFormatted(string? localizeKey, params FormattableString[] errorMessages);

    /// <summary>
    /// This adds an error to the status using a <see cref="FormattableString"/>s, when you don't have and properties
    /// to add to the <see cref="ValidationResult"/>. Using <see cref="FormattableString"/> allows you to place dynamic
    /// values (e.g. $"The time is {DateTime.Now:T}") that are also sent to the localizer.
    /// </summary>
    /// <param name="localizeKey">This is a key for the localized message in the respective resource / culture.
    /// If null, then the message won't get localized</param>
    /// <param name="errorMessage">The error message in the language / culture you defined when creating the
    /// StatusGenericLocalizer.</param>
    /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
    /// <returns>The StatusGenericLocalizer to allow fluent method calls.</returns>
    IStatusGenericLocalizer AddErrorFormattedWithParams(string? localizeKey, FormattableString errorMessage,
        params string[] propertyNames);

    /// <summary>
    /// This adds an error to the status using a <see cref="FormattableString"/>s, when you don't have and properties
    /// to add to the <see cref="ValidationResult"/>. Using <see cref="FormattableString"/> allows you to place dynamic
    /// values (e.g. $"The time is {DateTime.Now:T}") that are also sent to the localizer.
    /// </summary>
    /// <param name="localizeKey">This is a key for the localized message in the respective resource / culture.
    /// If null, then the message won't get localized</param>
    /// <param name="errorMessages">This is an array of <see cref="FormattableString"/> containing the error message i
    /// n the language / culture you defined when creating the  StatusGenericLocalizer.
    /// NOTE: this allows multiple <see cref="FormattableString"/>s to handle long messages.</param>
    /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
    /// <returns>The StatusGenericLocalizer to allow fluent method calls.</returns>
    IStatusGenericLocalizer AddErrorFormattedWithParams(string? localizeKey, FormattableString[] errorMessages,
        params string[] propertyNames);

    /// <summary>
    /// This allows you to set the <see cref="StatusGenericLocalizer{TResourceType}.Message"/> with a formattable string.
    /// Your provided <see cref="FormattableString"/> will be localized 
    /// </summary>
    /// <param name="localizeKey">This is a key for the localized message in the respective resource / culture.</param>
    /// <param name="formattableStrings">This is the default message for the culture defined in <see cref="StatusGenericLocalizer{TResourceType}"/>.
    ///     This takes one or more <see cref="FormattableString"/>s. and concatenates them into one message.
    ///     This allowed you to have multiple <see cref="FormattableString"/>s to handle long messages.
    /// </param>
    IStatusGenericLocalizer SetMessageFormatted(string localizeKey, params FormattableString[] formattableStrings);
}