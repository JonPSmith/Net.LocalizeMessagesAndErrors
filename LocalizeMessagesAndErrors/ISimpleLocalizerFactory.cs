// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This allows you to create a <see cref="ISimpleLocalizer"/> using a resource type
/// provided by you at runtime.
/// </summary>
public interface ISimpleLocalizerFactory
{
    /// <summary>
    /// This with create a <see cref="ISimpleLocalizer"/> linked to the TResource type 
    /// </summary>
    /// <param name="resourceSource">type of the resource class defining the localization resource files</param>
    /// <returns><see cref="IDefaultLocalizer"/></returns>
    /// <exception cref="NullReferenceException"></exception>
    ISimpleLocalizer Create(Type resourceSource);
}