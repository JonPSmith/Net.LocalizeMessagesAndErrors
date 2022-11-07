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
    /// This creates a unique message name string in the form of {className}_{methodName}_{localKey}.
    /// The className part is taking the FullName of the callingClass object, but if a
    /// <see cref="LocalizeSetClassNameAttribute"/> is applied to the class, then it's value is used
    /// The methodName part is taken from the method that called it, but if a
    /// <see cref="LocalizeSetMethodNameAttribute"/> is applied to the method, then it's value is used
    /// </summary>
    /// <param name="localKey"></param>
    /// <param name="callingClass">Use 'this' for this parameter, which will contain the method you are calling</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <returns>localizeKey</returns>
    public static string ClassMethodMessageKey(this string localKey, object callingClass,
        [CallerMemberName] string memberName = "")
    {
        var callingClassType = callingClass.GetType();

        var classAttribute =   (LocalizeSetClassNameAttribute?)Attribute.GetCustomAttribute(callingClassType, 
            typeof(LocalizeSetClassNameAttribute));
        var methodInfo = callingClassType.GetMethod(memberName);
        var methodAttribute = methodInfo == null ? null :
            (LocalizeSetMethodNameAttribute?)Attribute.GetCustomAttribute(methodInfo, typeof(LocalizeSetMethodNameAttribute));

        return (classAttribute?.ClassUniqueName ?? callingClassType.FullName) + "_" +
               (methodAttribute?.ClassMethodName ?? memberName) + "_" +
               localKey;
    }

    /// <summary>
    /// This creates a unique message name string in the form of {methodName}_{localKey}.
    /// WARNING: your should not use this if you have multiple methods with the same name,
    /// ass it won't be a unique localizeKey.
    /// </summary>
    /// <param name="localKey"></param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <returns>localizeKey</returns>
    public static string MethodMessageKey(this string localKey,
        [CallerMemberName] string memberName = "")
    {
        return memberName + "_" + localKey;
    }
}