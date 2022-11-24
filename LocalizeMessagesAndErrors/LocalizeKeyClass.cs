// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This contains the parts to make a unique localize key for a message.
/// It also holds the the Type of the class in which the localize key is called from
/// </summary>
public class LocalizeKeyClass
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="className"></param>
    /// <param name="methodName"></param>
    /// <param name="localKey"></param>
    public LocalizeKeyClass(string className, string methodName, string localKey)
    {
        ClassName = className;
        MethodName = methodName;
        LocalKey = localKey;
    }

    /// <summary>
    /// This contains a name that defines a Class of which the message was sent from.
    /// It can be the FullName of the class, or if a <see cref="LocalizeSetClassNameAttribute"/> is
    /// added to the class, then the developer can define a shorter (but unique) name.  
    /// Can be null.
    /// </summary>
    public string ClassName { get; }

    /// <summary>
    /// This contains the name of the method in which the message was sent from.
    /// It can be the name of the calling method, or if a <see cref="LocalizeSetMethodNameAttribute"/> is
    /// added to the class, then the developer can define a shorter (but unique) name.  
    /// Can be null.
    /// </summary>
    public string MethodName { get; }

    /// <summary>
    /// This contains a string provided by the developer for a specific message. Can be null.
    /// </summary>
    public string LocalKey { get; }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        var result = "";
        if (ClassName != null)
            result += (result == "" ? "" : "_") + ClassName;
        if (MethodName != null)
            result += (result == "" ? "" : "_") + MethodName;
        if (LocalKey != null)
            result += (result == "" ? "" : "_") + LocalKey;

        if (result == "")
            throw new InvalidOperationException("The LocalizeKey was empty.");
        return result;
    }
}