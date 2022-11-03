// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using StatusGeneric;
using System.ComponentModel.DataAnnotations;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This contains the error handling part of the GenericBizRunner
/// </summary>
public class StatusGenericLocalizer<TReturn, TResourceType> : StatusGenericLocalizer<TResourceType>, IStatusGenericLocalizer<TReturn>
{
    private TReturn? _result;

    /// <summary>
    /// Constructor to set up the StatusGenericLocalizer with Result
    /// </summary>
    /// <param name="cultureOfStrings">The culture of the errors/message strings in this service</param>
    /// <param name="localizerWithDefault">Logger to return warnings/errors of there localization problems</param>
    /// <param name="header">Optional: this will prefix each error with this string, e.g.
    /// "MyClass" would produce error messages such as "MyClass: This is my error message."</param>
    /// <exception cref="ArgumentNullException"></exception>
    public StatusGenericLocalizer(string cultureOfStrings, ILocalizeWithDefault<TResourceType> localizerWithDefault,
        string header = "")

        : base(cultureOfStrings, localizerWithDefault, header) { }

    /// <summary>
    /// This is the returned result
    /// </summary>
    public TReturn? Result => IsValid ? _result : default(TReturn);

    /// <summary>
    /// This sets the result to be returned
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IStatusGenericLocalizer<TReturn?> SetResult(TReturn result)
    {
        _result = result;
        return this;
    }


}