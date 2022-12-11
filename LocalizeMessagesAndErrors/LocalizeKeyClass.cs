// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This contains the parts to make a unique localize key for a message.
/// It also holds information on where the message was added. This allows you to
/// collect this information when running your unit tests
/// </summary>
public class LocalizeKeyData
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="localizeKey">The unique localizeKey for a message/</param>
    /// <param name="callingClass"></param>
    /// <param name="methodName"></param>
    /// <param name="sourceLineNumber"></param>
    public LocalizeKeyData(string localizeKey, Type callingClass, string methodName, int sourceLineNumber)
    {
        LocalizeKey = localizeKey;
        CallingClass = callingClass;
        MethodName = methodName;
        SourceLineNumber = sourceLineNumber;
    }

    /// <summary>
    /// This contains the localization key for this message.
    /// </summary>
    public string LocalizeKey { get; }

    /// <summary>
    /// This contains a name that defines a Class of which the message was sent from.
    /// This is used to collect information during your unit tests on what localization resource you need.
    /// Can be null.
    /// </summary>
    public Type CallingClass { get; }

    /// <summary>
    /// This contains the name of the method in which the message was sent from.
    /// This is used to collect information during your unit tests on what localization resource you need
    /// </summary>
    public string MethodName { get; }

    /// <summary>
    /// This contains the line number where the LocalizeKeyExtensions were called.
    /// This is used to collect information during your unit tests on what localization resource you need
    /// </summary>
    public int SourceLineNumber { get; }

}