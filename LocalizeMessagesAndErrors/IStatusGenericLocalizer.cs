// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using StatusGeneric;

namespace LocalizeMessagesAndErrors;

public interface IStatusGenericLocalizer
{
    /// <summary>
    /// This holds the list of errors, with any localization applied.
    /// If the collection is empty, then there were no errors
    /// </summary>
    IReadOnlyList<ErrorGeneric> Errors { get; }

    /// <summary>
    /// This is true if there are no errors registered
    /// </summary>
    bool IsValid { get; }

    /// <summary>
    /// This is true if any errors have been added 
    /// </summary>
    bool HasErrors { get; }

    /// <summary>
    /// On success this returns any message set by GenericServices, or any method that returns a status
    /// If there are errors it contains the message "Failed with NN errors"
    /// </summary>
    string Message { get; }

    /// <summary>
    /// This allows you to alter the <see cref="Message"/> with a formattable string.
    /// Like the 
    /// </summary>
    /// <param name="localizeKey"></param>
    /// <param name="formattableStrings"></param>
    IStatusGenericLocalizer SetMessageFormatted(string localizeKey, params FormattableString[] formattableStrings);

    /// <summary>
    /// This allows statuses to be combined
    /// </summary>
    /// <param name="status"></param>
    IStatusGenericLocalizer CombineStatuses(IStatusGenericLocalizer status);

    /// <summary>
    /// This is a simple method to output all the errors as a single string - null if no errors
    /// Useful for feeding back all the errors in a single exception (also nice in unit testing)
    /// </summary>
    /// <param name="separator">if null then each errors is separated by Environment.NewLine, otherwise uses the separator you provide</param>
    /// <returns>a single string with all errors separated by the 'separator' string</returns>
    string GetAllErrors(string? separator = null);

    IStatusGenericLocalizer AddErrorString(string localizeKey, string errorMessage, params string[] propertyNames);
    IStatusGenericLocalizer AddErrorFormatted(string localizeKey, params FormattableString[] formattableStrings);
    IStatusGenericLocalizer AddErrorFormattedWithParams(string localizeKey, FormattableString formattableString,
        params string[] propertyNames);
    IStatusGenericLocalizer AddErrorFormattedWithParams(string localizeKey, FormattableString[] formattableStrings,
        params string[] propertyNames);
}