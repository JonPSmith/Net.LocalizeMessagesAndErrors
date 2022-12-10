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
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData ClassLocalizeKey<TClass>(this string localKey, TClass callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) where TClass : class
    {
        var callingClassType = callingClass.GetType();

        var classAttribute = (LocalizeSetClassNameAttribute)Attribute.GetCustomAttribute(callingClassType,
            typeof(LocalizeSetClassNameAttribute));

        var localizeKey = (classAttribute?.ClassUniqueName ?? callingClassType.FullName) + "_" +
                          localKey;

        return new LocalizeKeyData(localizeKey, callingClassType, memberName, sourceLineNumber);
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
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData ClassMethodLocalizeKey<TClass>(this string localKey, TClass callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) where TClass : class
    {
        var callingClassType = callingClass.GetType();

        var classAttribute =   (LocalizeSetClassNameAttribute)Attribute.GetCustomAttribute(callingClassType, 
            typeof(LocalizeSetClassNameAttribute));
        var methodInfo = callingClassType.GetMethod(memberName);
        var methodAttribute = methodInfo == null ? null :
            (LocalizeSetMethodNameAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(LocalizeSetMethodNameAttribute));

        var localizeKey = (classAttribute?.ClassUniqueName ?? callingClassType.FullName) + "_" +
                          (methodAttribute?.MethodName ?? memberName) + "_" +
                          localKey;

        return new LocalizeKeyData(localizeKey, callingClassType, memberName, sourceLineNumber);
    }

    /// <summary>
    /// This creates a unique message name string in the form of {methodName}_{localKey}.
    /// This is useful if your Resource class is the same as the class containing the message
    /// (otherwise this message could create a non-unique localizeKey).
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClass">Use 'this' for this parameter. This is used to know what class it was called from.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData MethodLocalizeKey<TClass>(this string localKey, TClass callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) where TClass : class
    {
        var callingClassType = callingClass.GetType();

        var methodInfo = callingClassType.GetMethod(memberName);
        var methodAttribute = methodInfo == null ? null :
            (LocalizeSetMethodNameAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(LocalizeSetMethodNameAttribute));

        return new LocalizeKeyData((methodAttribute?.MethodName ?? memberName) + $"_{localKey}",
            callingClassType, memberName, sourceLineNumber);
    }

    /// <summary>
    /// This creates a message name string in the form of {localKey}.
    /// This is useful if you have a message that is the same everywhere within the resource file,
    /// or you want to create your own localize key.
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClass">Use 'this' for this parameter. This is used to know what class it was called from.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData JustThisLocalizeKey<TClass>(this string localKey, TClass callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) where TClass : class
    {
        return new LocalizeKeyData(localKey, callingClass.GetType(), memberName, sourceLineNumber);
    }

    /// <summary>
    /// Use this if the message has already been localized
    /// </summary>
    /// <param name="callingClass">Use 'this' for this parameter. This is used to know what class it was called from.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData AlreadyLocalized<TClass>(this TClass callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) where TClass : class
    {
        return new LocalizeKeyData(null, callingClass.GetType(), memberName, sourceLineNumber);
    }
}