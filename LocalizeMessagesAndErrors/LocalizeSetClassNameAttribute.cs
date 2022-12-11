// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This is used to provide a unique name instead of the fullname of a class, struct or interface
/// if the nameIsUnique parameter in is false. Useful to make a unique localizeKey that is shorter.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
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