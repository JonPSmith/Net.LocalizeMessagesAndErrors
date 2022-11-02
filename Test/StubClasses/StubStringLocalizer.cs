// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace Test.StubClasses;

public class StubStringLocalizer<TResourceType> : IStringLocalizer<TResourceType>
{
    public StubStringLocalizer(Dictionary<string, string> resource)
    {
        Resource = resource ?? throw new ArgumentNullException(nameof(resource));
    }

    public Dictionary<string,string> Resource { get; set; }

    /// <summary>Gets all string resources.</summary>
    /// <param name="includeParentCultures">
    /// A <see cref="T:System.Boolean" /> indicating whether to include strings from parent cultures.
    /// </param>
    /// <returns>The strings.</returns>
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return Resource.Select(x => new LocalizedString(x.Key, x.Value));
    }

    /// <summary>Gets the string resource with the given name.</summary>
    /// <param name="name">The name of the string resource.</param>
    /// <returns>The string resource as a <see cref="T:Microsoft.Extensions.Localization.LocalizedString" />.</returns>
    public LocalizedString this[string name]
    {
        get
        {
            if (Resource.ContainsKey(name))
            {
                return new LocalizedString(name, Resource[name], false, typeof(TResourceType).Name);
            }

            return new LocalizedString(name, "", true, typeof(TResourceType).Name);
        }
    }

    /// <summary>
    /// Gets the string resource with the given name and formatted with the supplied arguments.
    /// </summary>
    /// <param name="name">The name of the string resource.</param>
    /// <param name="arguments">The values to format the string with.</param>
    /// <returns>The formatted string resource as a <see cref="T:Microsoft.Extensions.Localization.LocalizedString" />.</returns>
    public LocalizedString this[string name, params object[] arguments] => throw new System.NotImplementedException();
}