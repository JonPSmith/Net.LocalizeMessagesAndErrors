// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This contains the options that the <see cref="DefaultLocalizer{T}"/> needs to work.
/// </summary>
public class DefaultLocalizerOptions : CommonLocalizerOptions //This contains DefaultCulture and ExactCultureMatch
{
    /// <summary>
    /// THis returns true if the application's culture matches the default culture from the options
    /// </summary>
    /// <returns></returns>
    public bool CultureMatches()
    {
        var appCulture = Thread.CurrentThread.CurrentUICulture.Name;
        return ExactCultureMatch
            ? appCulture == DefaultCulture
            : appCulture.StartsWith(DefaultCulture);
    }
}