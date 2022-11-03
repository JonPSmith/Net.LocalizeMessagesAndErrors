// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

[AttributeUsage(AttributeTargets.Method)]
public class LocalizeSetMethodNameAttribute : Attribute
{
    public LocalizeSetMethodNameAttribute(string classMethodName)
    {
        ClassMethodName = classMethodName;
    }

    public string ClassMethodName { get;}
}