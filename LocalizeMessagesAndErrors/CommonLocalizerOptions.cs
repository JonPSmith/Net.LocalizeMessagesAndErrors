// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

public class CommonLocalizerOptions
{
    private bool _exactCultureMatch;

    /// <summary>
    /// This holds the culture of the messages provided
    /// </summary>
    public string DefaultCulture { get; set; }

    /// <summary>
    /// If true, then it will only use the given messages if the <see cref="DefaultCulture"/> exactly matches
    /// the current culture of the application.
    /// NOTE This will be false if the <see cref="DefaultCulture"/> length is 2.
    /// </summary>
    public bool ExactCultureMatch
    {
        get => DefaultCulture?.Length == 2 ? false : _exactCultureMatch;
        set => _exactCultureMatch = value;
    }
}