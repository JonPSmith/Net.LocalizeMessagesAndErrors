// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This is used with the <see cref="LocalizeKeyExtensions.ClassMethodMessageKey"/>
/// to provide a unique name instead of the fullname of the class.
/// Useful to make the localizeKey shorter.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class LocalizeSetClassNameAttribute : Attribute
{
    /// <summary>
    /// ctor: define the replacement name of the class
    /// </summary>
    /// <param name="classUniqueName"></param>
    public LocalizeSetClassNameAttribute(string classUniqueName)
    {
        ClassUniqueName = classUniqueName;
    }

    /// <summary>
    /// Contains the replacement name of the class
    /// </summary>
    public string ClassUniqueName { get; }
}