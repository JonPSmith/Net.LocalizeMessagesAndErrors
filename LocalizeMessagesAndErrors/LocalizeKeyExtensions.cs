﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
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
    /// This is useful when you have multiple messages that all have the same format in one class.
    /// The {className} part is effected by the nameIsUnique parameter and the
    /// <see cref="LocalizeSetClassNameAttribute"/>.
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClass">Use 'this' for this parameter, which will contain the class / struct you are calling from.
    ///     This is used to get the Class name.</param>
    /// <param name="nameIsUnique">If true, then the Name of callingClass is used, otherwise
    /// it looks for the the <see cref="LocalizeSetClassNameAttribute"/> for a {className}, otherwise it
    /// uses the FullName of the callingClass param</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData ClassLocalizeKey<TThis>(this string localKey, TThis callingClass,
        bool nameIsUnique,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return localKey.StaticClassLocalizeKey(callingClass.GetType(), nameIsUnique, memberName, sourceLineNumber);
    }

    /// <summary>
    /// This creates a localize key of the form of {className}_{localKey} when working in static methods.
    /// This is useful when you have multiple messages that all have the same format in one class.
    /// The {className} part is effected by the nameIsUnique parameter and the
    /// <see cref="LocalizeSetClassNameAttribute"/>.
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClassType">The type of the class / struct you are calling from.</param>
    /// <param name="nameIsUnique">If true, then the Name of callingClass is used, otherwise
    /// it looks for the the <see cref="LocalizeSetClassNameAttribute"/> for a {className}, otherwise it
    /// uses the FullName of the callingClass param</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData StaticClassLocalizeKey(this string localKey, Type callingClassType,
        bool nameIsUnique,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        string classPartOfKey;
        if (nameIsUnique)
            classPartOfKey = callingClassType.Name;
        else
        {
            var classAttribute = (LocalizeSetClassNameAttribute)Attribute.GetCustomAttribute(callingClassType,
                typeof(LocalizeSetClassNameAttribute));
            classPartOfKey = classAttribute?.ClassUniqueName ?? callingClassType.FullName;
        }

        var localizeKey = classPartOfKey + "_" + localKey;
        return new LocalizeKeyData(localizeKey, callingClassType, memberName, sourceLineNumber);
    }

    /// <summary>
    /// This creates a localize key of the form of {className}_{methodName}_{localKey}.
    /// This is useful if your Resource class is used over multiple classes, because this 
    /// method creates a unique localizeKey containing the class name.
    /// The {className} part is effected by the nameIsUnique parameter and the
    /// <see cref="LocalizeSetClassNameAttribute"/>.
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClass">Use 'this' for this parameter, which will contain the class / struct you are calling from.
    ///     This is used to get the Class name.</param>
    /// <param name="nameIsUnique">If true, then the Name of callingClass is used, otherwise
    /// it looks for the the <see cref="LocalizeSetClassNameAttribute"/> for a {className}, otherwise it
    /// uses the FullName of the callingClass param</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData ClassMethodLocalizeKey<TThis>(this string localKey, TThis callingClass,
        bool nameIsUnique,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return localKey.StaticClassMethodLocalizeKey(callingClass.GetType(), nameIsUnique, memberName,
            sourceLineNumber);
    }

    /// <summary>
    /// This creates a localize key of the form of {className}_{methodName}_{localKey} when working in static methods.
    /// This is useful if your Resource class is used over multiple classes, because this 
    /// method creates a unique localizeKey containing the class name.
    /// The {className} part is effected by the nameIsUnique parameter and the
    /// <see cref="LocalizeSetClassNameAttribute"/>.
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClassType">The type of the class / struct you are calling from</param>
    /// <param name="nameIsUnique">If true, then the Name of callingClass is used, otherwise
    /// it looks for the the <see cref="LocalizeSetClassNameAttribute"/> for a {className}, otherwise it
    /// uses the FullName of the callingClass param</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData StaticClassMethodLocalizeKey(this string localKey, Type callingClassType,
        bool nameIsUnique,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        string classPartOfKey = GetClassPartOfKey(nameIsUnique, callingClassType);
        var localizeKey = classPartOfKey + "_" + memberName + "_" + localKey;
        return new LocalizeKeyData(localizeKey, callingClassType, memberName, sourceLineNumber);
    }

    /// <summary>
    /// This creates a unique message name string in the form of {methodName}_{localKey}.
    /// This is useful if your Resource class is the same as the class containing the message
    /// (otherwise this message could create a non-unique localizeKey).
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClass">Use 'this' for this parameter, which will contain the class / struct you are calling from.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData MethodLocalizeKey<TThis>(this string localKey, TThis callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return localKey.StaticMethodLocalizeKey(callingClass.GetType(), memberName, sourceLineNumber);
    }

    /// <summary>
    /// This creates a unique message name string in the form of {methodName}_{localKey}.
    /// This is useful if your Resource class is the same as the class containing the message
    /// (otherwise this message could create a non-unique localizeKey).
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClassType">The type of the class / struct you are calling from</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData StaticMethodLocalizeKey(this string localKey, Type callingClassType,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return new LocalizeKeyData($"{memberName}_{localKey}", callingClassType, memberName, sourceLineNumber);
    }

    /// <summary>
    /// This creates a message name string in the form of {localKey}.
    /// This is useful if you have a message that is the same everywhere within the resource file,
    /// or you want to create your own localize key.
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClass">Use 'this' for this parameter, which will contain the class / struct you are calling from.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData JustThisLocalizeKey<TThis>(this string localKey, TThis callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return new LocalizeKeyData(localKey, callingClass.GetType(), memberName, sourceLineNumber);
    }

    /// <summary>
    /// This creates a message name string in the form of {localKey}.
    /// This is useful if you have a message that is the same everywhere within the resource file,
    /// or you want to create your own localize key.
    /// </summary>
    /// <param name="localKey">This is local key part of the localizedKey.</param>
    /// <param name="callingClassType">The type of the class / struct you are calling from</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData StaticJustThisLocalizeKey(this string localKey, Type callingClassType,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return new LocalizeKeyData(localKey, callingClassType, memberName, sourceLineNumber);
    }

    /// <summary>
    /// Use this if the message has already been localized, or you can't localize the message
    /// because it comes from an internal .NET message.
    /// </summary>
    /// <param name="callingClass">Use 'this' for this parameter, which will contain the class / struct you are calling from.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData AlreadyLocalized<TThis>(this TThis callingClass,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return new LocalizeKeyData(null, callingClass.GetType(), memberName, sourceLineNumber);
    }

    /// <summary>
    /// Use this if the message has already been localized, or you can't localize the message
    /// because it comes from an internal .NET message.
    /// </summary>
    /// <param name="callingClassType">The type of the class / struct you are calling from.</param>
    /// <param name="memberName">DO NOT use. This a filled by the calling method name</param>
    /// <param name="sourceLineNumber">DO NOT use. This a filled by the calling line number</param>
    /// <returns>LocalizeKeyData</returns>
    public static LocalizeKeyData StaticAlreadyLocalized(this Type callingClassType,
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
    {
        return new LocalizeKeyData(null, callingClassType, memberName, sourceLineNumber);
    }

    //-------------------------------------------------------------
    // private methods

    private static string GetClassPartOfKey(bool nameIsUnique, Type callingClassType)
    {
        string classPartOfKey;
        if (nameIsUnique)
            classPartOfKey = callingClassType.Name;
        else
        {
            var classAttribute = (LocalizeSetClassNameAttribute)Attribute.GetCustomAttribute(callingClassType,
                typeof(LocalizeSetClassNameAttribute));
            classPartOfKey = classAttribute?.ClassUniqueName ?? callingClassType.FullName;
        }

        return classPartOfKey;
    }
}