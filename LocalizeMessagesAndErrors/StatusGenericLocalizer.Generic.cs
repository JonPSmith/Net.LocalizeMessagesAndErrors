﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.


using StatusGeneric;
using System.ComponentModel.DataAnnotations;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This contains the error handling part of the GenericBizRunner
/// </summary>
public class StatusGenericLocalizer<TReturn> : StatusGenericLocalizer, IStatusGeneric<TReturn>
{
    private readonly IDefaultLocalizer _defaultLocalizer;

    private TReturn _result;

    /// <summary>
    /// Constructor to set up the StatusGenericLocalizer with Result
    /// </summary>
    /// <param name="defaultLocalizer">Uses the <see cref="DefaultLocalizer{TResource}"/> to handle the localization.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public StatusGenericLocalizer(IDefaultLocalizer defaultLocalizer)
        : base(defaultLocalizer)
    {
        _defaultLocalizer = defaultLocalizer;
    }

    /// <summary>
    /// This is the returned result
    /// </summary>
    public TReturn Result => IsValid ? _result : default;

    /// <summary>
    /// This sets the result to be returned
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IStatusGeneric<TReturn> SetResult(TReturn result)
    {
        _result = result;
        return this;
    }

    /// <summary>
    /// This adds an error to the status using a string. Don't use this if you have dynamic values in the message.
    /// </summary>
    /// <param name="localizeKey">This is a key for the localized message in the respective resource / culture.
    /// If null, then the message won't get localized</param>
    /// <param name="errorMessage">The error message in the language / culture you defined when creating the
    /// StatusGenericLocalizer.</param>
    /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
    /// <returns>The StatusGenericLocalizer to allow fluent method calls.</returns>
    public new IStatusGeneric<TReturn> AddErrorString(LocalizeKeyData localizeKey, string errorMessage,
        params string[] propertyNames)
    {
        var errorString = _defaultLocalizer.LocalizeStringMessage(localizeKey, errorMessage);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString, propertyNames)));
        return this;
    }

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
    public new IStatusGeneric<TReturn> AddErrorFormatted(LocalizeKeyData localizeKey,
        params FormattableString[] errorMessages)
    {
        var errorString = _defaultLocalizer.LocalizeFormattedMessage(localizeKey, errorMessages);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString)));
        return this;
    }

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
    public new IStatusGeneric<TReturn> AddErrorFormattedWithParams(LocalizeKeyData localizeKey,
        FormattableString errorMessage,
        params string[] propertyNames)
    {
        var errorString = _defaultLocalizer.LocalizeFormattedMessage(localizeKey, errorMessage);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString, propertyNames)));
        return this;
    }

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
    public new IStatusGeneric<TReturn> AddErrorFormattedWithParams(LocalizeKeyData localizeKey,
        FormattableString[] errorMessages, params string[] propertyNames)
    {
        var errorString = _defaultLocalizer.LocalizeFormattedMessage(localizeKey, errorMessages);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString, propertyNames)));
        return this;
    }
}