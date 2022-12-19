// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This contains the options that the <see cref="DefaultLocalizer{T}"/> needs to work.
/// </summary>
public class DefaultLocalizerOptions
{
    /// <summary>
    /// This holds the culture of the messages provided
    /// </summary>
    public string DefaultCulture { get; set; }
}