// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using StatusGeneric;
using System.ComponentModel.DataAnnotations;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This provides a status system where you can add errors and success message which are localized.
/// It based on the https://github.com/JonPSmith/GenericServices.StatusGeneric library.
/// </summary>
/// <typeparam name="TResourceType"></typeparam>
public class StatusGenericLocalizer<TResourceType> : IStatusGenericLocalizer
{
    private readonly ILocalizeWithDefault<TResourceType> _localizerWithDefault;
    private readonly string _cultureOfStrings;

    /// <summary>
    /// This is the default success message. This isn't localized
    /// </summary>
    private const string DefaultSuccessMessage = "Success";
    /// <summary>
    /// This contains the localized success message set by the <see cref="SetMessageFormatted"/> method.
    /// </summary>
    private string _successMessage = DefaultSuccessMessage;
    /// <summary>
    /// Holds the internal list of errors.
    /// </summary>
    protected readonly List<ErrorGeneric> _errors = new();

    /// <summary>
    /// Constructor to set up the StatusGenericLocalizer
    /// </summary>
    /// <param name="cultureOfStrings">The culture of the errors/message strings in this service</param>
    /// <param name="localizerWithDefault">Logger to return warnings/errors of there localization problems</param>
    /// <param name="header">Optional: this will prefix each error with this string, e.g.
    /// "MyClass" would produce error messages such as "MyClass: This is my error message."</param>
    /// <exception cref="ArgumentNullException"></exception>
    public StatusGenericLocalizer(string cultureOfStrings, ILocalizeWithDefault<TResourceType> localizerWithDefault,
        string header = "")
    {
        _localizerWithDefault = localizerWithDefault ?? throw new ArgumentNullException(nameof(localizerWithDefault));
        _cultureOfStrings = cultureOfStrings ?? throw new ArgumentNullException(nameof(cultureOfStrings));
        Header = header;
    }

    //------------------------------------------------------
    //properties

    /// <summary>
    /// The header provides a prefix to any errors you add. Useful if you want to have a general prefix to all your errors
    /// e.g. a header if "MyClass" would produce error messages such as "MyClass: This is my error message."
    /// </summary>
    public string Header { get; set; }

    /// <summary>
    /// This holds the list of ValidationResult errors. If the collection is empty, then there were no errors
    /// </summary>
    public IReadOnlyList<ErrorGeneric> Errors => _errors.AsReadOnly();

    /// <summary>
    /// This is true if there are no errors 
    /// </summary>
    public bool IsValid => !_errors.Any();

    /// <summary>
    /// This is true if any errors have been added 
    /// </summary>
    public bool HasErrors => _errors.Any();

    /// <summary>
    /// This defines the name for the localization entry for localize the Message when the status is "HasErrors"
    /// NOTE: The error message is the same for all StatusGenericLocalizer classes.
    /// </summary>
    public string LocalizeKeyFailedMessage { get; set; } = "StatusGenericLocalizer_MessageHasErrors";

    /// <summary>
    /// On success this returns the message as set by the business logic, or the default messages
    /// If there are errors it contains the message "Failed with NN errors"
    /// Both messages are localized, but the success Message is only localized if you use the <see cref="SetMessageFormatted"/> method.
    /// </summary>
    public string Message
    {
        get
        {
            if (HasErrors)
                return _localizerWithDefault.LocalizeFormattedMessage(
                    new LocalizeKeyData(LocalizeKeyFailedMessage, GetType(), "Message_Get", 0),
                    _cultureOfStrings,
                    $"Failed with {_errors.Count} error{(_errors.Count == 1 ? "" : "s")}");

            return _successMessage;
        }
        set =>
            throw new InvalidOperationException(
                "You cannot set the Message when using StatusGenericLocalizer. Use the SetMessage methods to set the Message instead.");
    }

    //------------------------------------------------------
    //methods

        /// <summary>
        /// This adds an error to the status using a string. Don't use this if you have dynamic values in the message.
        /// </summary>
        /// <param name="localizeKey">This is the key for finding the localized error in your respective resources / cultures.</param>
        /// <param name="errorMessage">The error message in the language / culture you defined when creating the
        /// StatusGenericLocalizer.</param>
        /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
        /// <returns>The StatusGenericLocalizer to allow fluent method calls.</returns>
    public IStatusGeneric AddErrorString(LocalizeKeyData localizeKey, string errorMessage,
        params string[] propertyNames)
    {
        var errorString = _localizerWithDefault.LocalizeStringMessage(localizeKey, _cultureOfStrings, errorMessage);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString, propertyNames)));
        return this;
    }

    /// <summary>
    /// This adds an error to the status using a <see cref="FormattableString"/>s, when you don't have and properties
    /// to add to the <see cref="ValidationResult"/>. Using <see cref="FormattableString"/> allows you to place dynamic
    /// values (e.g. $"The time is {DateTime.Now:T}") that are also sent to the localizer.
    /// </summary>
    /// <param name="localizeKey">This is the key for finding the localized error in your respective resources / cultures.</param>
    /// <param name="errorMessages">The error messages in the language / culture you defined when creating the
    /// StatusGenericLocalizer. NOTE: this allows multiple <see cref="FormattableString"/>s to handle long messages.</param>
    /// <returns>The StatusGenericLocalizer to allow fluent method calls.</returns>
    public IStatusGeneric AddErrorFormatted(LocalizeKeyData localizeKey, params FormattableString[] errorMessages)
    {
        var errorString = _localizerWithDefault.LocalizeFormattedMessage(localizeKey, _cultureOfStrings, errorMessages);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString)));
        return this;
    }

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
    public IStatusGeneric AddErrorFormattedWithParams(LocalizeKeyData localizeKey, FormattableString errorMessage,
        params string[] propertyNames)
    {
        var errorString = _localizerWithDefault.LocalizeFormattedMessage(localizeKey, _cultureOfStrings, errorMessage);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString, propertyNames)));
        return this;
    }

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
    public IStatusGeneric AddErrorFormattedWithParams(LocalizeKeyData localizeKey,
        FormattableString[] errorMessages, params string[] propertyNames)
    {
        var errorString = _localizerWithDefault.LocalizeFormattedMessage(localizeKey, _cultureOfStrings, errorMessages);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString, propertyNames)));
        return this;
    }

    /// <summary>
    /// This allows you to set the <see cref="IStatusGeneric.Message"/> with a localized string.
    /// </summary>
    /// <param name="localizeKey">This is the key for finding the localized message in your respective resources / cultures.</param>
    /// <param name="message">string that can be localized to set the <see cref="Message"/> property</param>
    public IStatusGeneric SetMessageString(LocalizeKeyData localizeKey, string message)
    {
        _successMessage = _localizerWithDefault.LocalizeStringMessage(localizeKey, _cultureOfStrings, message);
        return this;
    }

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
    public IStatusGeneric SetMessageFormatted(LocalizeKeyData localizeKey, params FormattableString[] formattableStrings)
    {
        _successMessage = _localizerWithDefault.LocalizeFormattedMessage(localizeKey, _cultureOfStrings, formattableStrings);
        return this;
    }

    //---------------------------------------------------
    //Normal StatusGeneric methods

    /// <summary>
    /// This allows statuses to be combined. Copies over any errors and replaces the Message if the current message is null
    /// If you are using Headers then it will combine the headers in any errors in combines
    /// e.g. Status1 with header "MyClass" combines Status2 which has header "MyProp" and status2 has errors..
    /// The result would be error message in status2 would be updates to start with "MyClass>MyProp: This is my error message."
    /// </summary>
    /// <param name="status"></param>
    public IStatusGeneric CombineStatuses(IStatusGeneric status)
    {
        if (!status.IsValid)
        {
            _errors.AddRange(string.IsNullOrEmpty(Header)
                ? status.Errors
                : status.Errors.Select(x => new ErrorGeneric(Header, x.ErrorResult)));
        }
        if (IsValid && status.Message != DefaultSuccessMessage)
            _successMessage = status.Message;

        return this;
    }

    /// <summary>
    /// This is a simple method to output all the errors as a single string - returns "No errors" if no errors.
    /// Useful for feeding back all the errors in a single exception (also nice in unit testing)
    /// </summary>
    /// <param name="separator">if null then each errors is separated by Environment.NewLine, otherwise uses the separator you provide</param>
    /// <returns>a single string with all errors separated by the 'separator' string, or "No errors" if no errors.</returns>
    public string GetAllErrors(string separator = null)
    {
        separator ??= Environment.NewLine;
        return _errors.Any()
            ? string.Join(separator, Errors)
            : "No errors";
    }
}