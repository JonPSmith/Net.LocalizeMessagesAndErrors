// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This contains the options that the <see cref="SimpleLocalizer"/> needs to work.
/// </summary>
public class SimpleLocalizerOptions
{
    /// <summary>
    /// THis holds the type used to define the start of the resource file name holding the different languages
    /// </summary>
    public Type ResourceType { get; set; }

    /// <summary>
    /// This holds a string which prefixes the localize key, i.e. $"{PrefixKeyString}({message})"
    /// If null, then just the message is used as the localize key
    /// </summary>
    public string PrefixKeyString { get; set; } = "SimpleLocalizer";
}