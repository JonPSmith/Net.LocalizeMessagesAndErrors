// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.Localization;

namespace LocalizeMessagesAndErrors.UnitTestingCode;

/// <summary>
/// This is a replacement for the <see cref="IStringLocalizer"/> service
/// which allows you to provide a dictionary to mimic the resource files.
/// Its very simple, and only uses the name of the localization, and not the culture, to lookup value.
/// </summary>
public class StubStringLocalizer : IStringLocalizer
{
    /// <summary>
    /// This allows you to provide the localized name + value entries to return
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="throwExceptionIfNoEntry">If name not found, then throw exception. Defaults onto true</param>
    /// <exception cref="ArgumentNullException"></exception>
    public StubStringLocalizer(Dictionary<string, string> resource, bool throwExceptionIfNoEntry = true)
    {
        Resource = resource ?? new Dictionary<string, string>();
        ThrowExceptionIfNoEntry = throwExceptionIfNoEntry;
    }

    /// <summary>
    /// Useful if you want to access the name / value you added to this class
    /// </summary>
    public Dictionary<string, string> Resource { get; set; }

    /// <summary>
    /// The last LocalizeKey
    /// </summary>
    public string LastLocalizeKey { get; private set; }

    /// <summary>
    /// If true, then throw exception if no entry
    /// </summary>
    public bool ThrowExceptionIfNoEntry { get; set; }

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
            LastLocalizeKey = name;
            if (Resource.ContainsKey(name))
            {
                return new LocalizedString(name, Resource[name], false, "dummy searched location");
            }

            if (ThrowExceptionIfNoEntry)
                throw new MissingFieldException($"There was no entry with the name '{name} in the dictionary.");

            return new LocalizedString(name, "", true, "dummy searched location");
        }
    }

    /// <summary>
    /// Gets the string resource with the given name and formatted with the supplied arguments.
    /// </summary>
    /// <param name="name">The name of the string resource.</param>
    /// <param name="arguments">The values to format the string with.</param>
    /// <returns>The formatted string resource as a <see cref="T:Microsoft.Extensions.Localization.LocalizedString" />.</returns>
    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            LastLocalizeKey = name;
            if (Resource.ContainsKey(name))
            {
                var message = string.Format(Resource[name], arguments);

                return new LocalizedString(name, message, false, "dummy searched location");
            }

            if (ThrowExceptionIfNoEntry)
                throw new MissingFieldException($"There was no entry with the name '{name} in the dictionary.");

            return new LocalizedString(name, "", true, "dummy searched location");
        }
    }
}