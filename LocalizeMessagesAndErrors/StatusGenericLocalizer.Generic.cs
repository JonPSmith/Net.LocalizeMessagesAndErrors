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

    public StatusGenericLocalizer(ILocalizeWithDefault<TResourceType> localizerLogger, string cultureOfStrings, string header = "")
        : base(localizerLogger, cultureOfStrings, header)
    {
    }

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