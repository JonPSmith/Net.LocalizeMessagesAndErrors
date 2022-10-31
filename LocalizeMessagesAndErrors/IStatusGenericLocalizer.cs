// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using StatusGeneric;

namespace LocalizeMessagesAndErrors;

public interface IStatusGenericLocalizer : IStatusGeneric
{
    IStatusGeneric AddError(string errorName, string errorMessage, params string[] propertyNames);
    IStatusGeneric AddErrorFormatted(string errorName, params FormattableString[] formattableStrings);
    IStatusGeneric AddErrorFormattedWithParams(string errorName, FormattableString formattableString, params string[] propertyNames);
    IStatusGeneric AddErrorFormattedWithParams(string errorName, FormattableString[] formattableStrings, params string[] propertyNames);
}