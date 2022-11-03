// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using StatusGeneric;
using System.ComponentModel.DataAnnotations;

namespace LocalizeMessagesAndErrors;

public class StatusGenericLocalizer<TResourceType> : IStatusGenericLocalizer
{
    private readonly ILocalizeWithDefault<TResourceType> _localizerWithDefault;
    private readonly string _cultureOfStrings;

    /// <summary>
    /// This is the default success message.
    /// </summary>
    public const string DefaultSuccessMessage = "Success";
    protected readonly List<ErrorGeneric> _errors = new();
    private string _successMessage = DefaultSuccessMessage;

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
    /// Both messages are localized, but the Message is only localized if the   
    /// </summary>
    public string Message
    {
        get {
            if (HasErrors)
                return _localizerWithDefault.LocalizeFormattedMessage(LocalizeKeyFailedMessage, _cultureOfStrings,
                    $"Failed with {_errors.Count} error{(_errors.Count == 1 ? "" : "s")}");

            return _successMessage;
        }
        private set => _successMessage = value;
    }

    //------------------------------------------------------
    //methods

    public IStatusGenericLocalizer AddErrorString(string localizeKey, string errorMessage,
        params string[] propertyNames)
    {
        var errorString = _localizerWithDefault.LocalizeStringMessage(localizeKey, _cultureOfStrings, errorMessage);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString, propertyNames)));
        return this;
    }

    public IStatusGenericLocalizer AddErrorFormatted(string localizeKey, params FormattableString[] formattableStrings)
    {
        var errorString = _localizerWithDefault.LocalizeFormattedMessage(localizeKey, _cultureOfStrings, formattableStrings);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString)));
        return this;
    }

    public IStatusGenericLocalizer AddErrorFormattedWithParams(string localizeKey, FormattableString formattableString,
        params string[] propertyNames)
    {
        var errorString = _localizerWithDefault.LocalizeFormattedMessage(localizeKey, _cultureOfStrings, formattableString);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString, propertyNames)));
        return this;
    }

    public IStatusGenericLocalizer AddErrorFormattedWithParams(string localizeKey,
        FormattableString[] formattableStrings, params string[] propertyNames)
    {
        var errorString = _localizerWithDefault.LocalizeFormattedMessage(localizeKey, _cultureOfStrings, formattableStrings);
        _errors.Add(new ErrorGeneric(Header, new ValidationResult(errorString, propertyNames)));
        return this;
    }

    //---------------------------------------------------
    //Normal StatusGeneric methods

    /// <summary>
    /// This allows you to alter the <see cref="IStatusGenericLocalizer.Message"/> with a localized string.
    /// </summary>
    /// <param name="localizeKey">The</param>
    /// <param name="formattableStrings"></param>
    public IStatusGenericLocalizer SetMessageFormatted(string localizeKey, params FormattableString[] formattableStrings)
    {
        Message = _localizerWithDefault.LocalizeFormattedMessage(localizeKey, _cultureOfStrings, formattableStrings);
        return this;
    }

    /// <summary>
    /// This allows statuses to be combined. Copies over any errors and replaces the Message if the current message is null
    /// If you are using Headers then it will combine the headers in any errors in combines
    /// e.g. Status1 with header "MyClass" combines Status2 which has header "MyProp" and status2 has errors..
    /// The result would be error message in status2 would be updates to start with "MyClass>MyProp: This is my error message."
    /// </summary>
    /// <param name="status"></param>
    public IStatusGenericLocalizer CombineStatuses(IStatusGenericLocalizer status)
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
    public string GetAllErrors(string? separator = null)
    {
        separator ??= Environment.NewLine;
        return _errors.Any()
            ? string.Join(separator, Errors)
            : "No errors";
    }
}