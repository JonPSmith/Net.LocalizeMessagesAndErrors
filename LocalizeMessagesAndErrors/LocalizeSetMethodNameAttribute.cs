// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This is used with the <see cref="LocalizeKeyExtensions.ClassMethodLocalizeKey"/>
/// to provide a unique name instead of the method.
/// Useful if there are multiple methods with the same name.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class LocalizeSetMethodNameAttribute : Attribute
{
    /// <summary>
    /// ctor: sets the replacement name of the method
    /// </summary>
    /// <param name="classMethodName"></param>
    public LocalizeSetMethodNameAttribute(string classMethodName)
    {
        ClassMethodName = classMethodName;
    }
    /// <summary>
    /// Contains the replacement name of the method
    /// </summary>
    public string ClassMethodName { get;}
}