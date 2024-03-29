﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This allows you to create a <see cref="IDefaultLocalizer"/> when your resource class is
/// no known on startup.
/// </summary>
public interface IDefaultLocalizerFactory
{
    /// <summary>
    /// This with create a <see cref="IDefaultLocalizer"/> linked to the TResource type 
    /// </summary>
    /// <param name="resourceSource">type of the resource class defining the localization resource files</param>
    /// <returns><see cref="IDefaultLocalizer"/></returns>
    /// <exception cref="NullReferenceException"></exception>
    IDefaultLocalizer Create(Type resourceSource);
}