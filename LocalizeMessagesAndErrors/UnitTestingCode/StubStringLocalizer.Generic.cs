// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.Localization;

namespace LocalizeMessagesAndErrors.UnitTestingCode;

/// <summary>
/// This is a replacement for the <see cref="IStringLocalizer{TResource}"/> service
/// which allows you to provide a dictionary to mimic the resource files.
/// Its very simple, and only uses the name of the localization, and not the culture, to lookup value.
/// </summary>
/// <typeparam name="TResource"></typeparam>
public class StubStringLocalizer<TResource> : StubStringLocalizer, IStringLocalizer<TResource>
{
    /// <summary>
    /// This allows you to provide the localized name + value entries you would 
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="throwExceptionIfNoEntry">If name not found, then throw exception. Defaults onto true</param>
    /// <exception cref="ArgumentNullException"></exception>
    public StubStringLocalizer(Dictionary<string, string> resource, bool throwExceptionIfNoEntry = true)
        : base(resource, throwExceptionIfNoEntry)
    {
    }
}