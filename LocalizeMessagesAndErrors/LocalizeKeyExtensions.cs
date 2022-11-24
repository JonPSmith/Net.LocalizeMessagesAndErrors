// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Runtime.CompilerServices;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// Extension methods to build unique localizeKeys
/// </summary>
public static class LocalizeKeyExtensions
{
    /// <summary>
    /// This creates a localize key of the form of {className}_{localKey}.
    /// This is useful when you have multiple messages that all have the same format. 
    /// The className part is taking the FullName of the callingClass object, but if a
    /// <see cref="LocalizeSetClassNameAttribute"/> is applied to the class, then attribute's
    /// <see cref="LocalizeSetClassNameAttribute.ClassUniqueName"/> value is used.
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClass">Use 'this' for this parameter, which will contain the class you are calling from.
    /// This is used to get the Class name.</param>
    /// <returns>localizeKey</returns>
    public static LocalizeKeyClass ClassLocalizeKey(this string localKey, object callingClass)
    {
        var callingClassType = callingClass.GetType();

        var classAttribute = (LocalizeSetClassNameAttribute)Attribute.GetCustomAttribute(callingClassType,
            typeof(LocalizeSetClassNameAttribute));

        return new LocalizeKeyClass(classAttribute?.ClassUniqueName ?? callingClassType.FullName,
            null, localKey);
    }

    /// <summary>
    /// This creates a localize key of the form of {className}_{methodName}_{localKey}.
    /// This is useful if your Resource class is used over multiple classes, because this 
    /// method creates a unique localizeKey containing the class name.
    /// The className part is taking the FullName of the callingClass object, but if a
    /// <see cref="LocalizeSetClassNameAttribute"/> is applied to the class, then it's value is used
    /// The methodName part is taken from the method that called it, but if a
    /// <see cref="LocalizeSetMethodNameAttribute"/> is applied to the method, then it's value is used
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClass">Use 'this' for this parameter, which will contain the class you are calling from.
    /// This is used to get the Class name.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <returns>localizeKey</returns>
    public static LocalizeKeyClass ClassMethodLocalizeKey(this string localKey, object callingClass,
        [CallerMemberName] string memberName = "")
    {
        var callingClassType = callingClass.GetType();

        var classAttribute =   (LocalizeSetClassNameAttribute)Attribute.GetCustomAttribute(callingClassType, 
            typeof(LocalizeSetClassNameAttribute));
        var methodInfo = callingClassType.GetMethod(memberName);
        var methodAttribute = methodInfo == null ? null :
            (LocalizeSetMethodNameAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(LocalizeSetMethodNameAttribute));

        return new LocalizeKeyClass(classAttribute?.ClassUniqueName ?? callingClassType.FullName,
            methodAttribute?.ClassMethodName ?? memberName, 
            localKey);
    }

    /// <summary>
    /// This creates a unique message name string in the form of {methodName}_{localKey}.
    /// This is useful if your Resource class is the same as the class containing the message
    /// (otherwise this message could create a non-unique localizeKey).
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClass">Use 'this' for this parameter. this is used find is there is a
    /// <see cref="LocalizeSetMethodNameAttribute"/> which changes the method name.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <returns>localizeKey</returns>
    public static LocalizeKeyClass MethodLocalizeKey(this string localKey, object callingClass,
        [CallerMemberName] string memberName = "")
    {
        var callingClassType = callingClass.GetType();

        var methodInfo = callingClassType.GetMethod(memberName);
        var methodAttribute = methodInfo == null ? null :
            (LocalizeSetMethodNameAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(LocalizeSetMethodNameAttribute));

        return new LocalizeKeyClass(null,
            methodAttribute?.ClassMethodName ?? memberName,
            localKey);
    }
}