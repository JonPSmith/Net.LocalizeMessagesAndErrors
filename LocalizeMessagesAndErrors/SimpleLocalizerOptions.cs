﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This contains the options that the <see cref="SimpleLocalizer"/> needs to work.
/// </summary>
public class SimpleLocalizerOptions : CommonLocalizerOptions //This contains DefaultCulture and ExactCultureMatch
{
    /// <summary>
    /// THis holds the type used to define the start of the resource file name holding the different languages
    /// </summary>
    public Type ResourceType { get; set; }

    /// <summary>
    /// This holds the 
    /// </summary>
    public string PrefixKeyString { get; set; } = "SimpleLocalizer";
}