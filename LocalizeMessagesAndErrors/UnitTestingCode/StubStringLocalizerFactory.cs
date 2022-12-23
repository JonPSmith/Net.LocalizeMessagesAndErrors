// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.Localization;

namespace LocalizeMessagesAndErrors.UnitTestingCode;

/// <summary>
/// This is used for testing the <see cref="DefaultLocalizerFactory"/>
/// </summary>
public class StubStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly StubStringLocalizer _stubStringLocalizer;

    /// <summary>
    /// This allows you to provide the localized name + value entries you would 
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="throwExceptionIfNoEntry">If name not found, then throw exception. Defaults onto true</param>
    /// <exception cref="ArgumentNullException"></exception>
    public StubStringLocalizerFactory(Dictionary<string, string> resource, bool throwExceptionIfNoEntry = true)
    {
        _stubStringLocalizer = new StubStringLocalizer(resource, throwExceptionIfNoEntry);
    }

    /// <summary>
    /// Creates an <see cref="T:Microsoft.Extensions.Localization.IStringLocalizer" /> using the <see cref="T:System.Reflection.Assembly" /> and
    /// <see cref="P:System.Type.FullName" /> of the specified <see cref="T:System.Type" />.
    /// </summary>
    /// <param name="resourceSource">The <see cref="T:System.Type" />.</param>
    /// <returns>The <see cref="T:Microsoft.Extensions.Localization.IStringLocalizer" />.</returns>
    public IStringLocalizer Create(Type resourceSource)
    {
        return _stubStringLocalizer;
    }

    /// <summary>
    /// Creates an <see cref="T:Microsoft.Extensions.Localization.IStringLocalizer" />.
    /// </summary>
    /// <param name="baseName">The base name of the resource to load strings from.</param>
    /// <param name="location">The location to load resources from.</param>
    /// <returns>The <see cref="T:Microsoft.Extensions.Localization.IStringLocalizer" />.</returns>
    public IStringLocalizer Create(string baseName, string location)
    {
        throw new NotImplementedException();
    }
}