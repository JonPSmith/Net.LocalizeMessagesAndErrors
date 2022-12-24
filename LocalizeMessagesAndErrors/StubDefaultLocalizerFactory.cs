// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using LocalizeMessagesAndErrors.UnitTestingCode;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This is used to test the <see cref="SimpleLocalizer"/>
/// </summary>
public class StubDefaultLocalizerFactory : IDefaultLocalizerFactory
{
    public StubDefaultLocalizerFactory(StubDefaultLocalizer subDefaultLocalizer)
    {
        SubDefaultLocalizer = subDefaultLocalizer;
    }

    /// <summary>
    /// This allows you to access the 
    /// </summary>
    public StubDefaultLocalizer SubDefaultLocalizer { get; }

    /// <summary>
    /// This with create a <see cref="IDefaultLocalizer"/> linked to the TResource type 
    /// </summary>
    /// <param name="resourceSource">type of the resource class defining the localization resource files</param>
    /// <returns><see cref="IDefaultLocalizer"/></returns>
    /// <exception cref="NullReferenceException"></exception>
    public IDefaultLocalizer Create(Type resourceSource)
    {
        return SubDefaultLocalizer;
    }
}