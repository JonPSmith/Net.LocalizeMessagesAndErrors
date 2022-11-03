// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

[AttributeUsage(AttributeTargets.Class)]
public class LocalizeSetClassNameAttribute : Attribute
{
    public LocalizeSetClassNameAttribute(string classUniqueName)
    {
        ClassUniqueName = classUniqueName;
    }

    public string ClassUniqueName { get; }
}