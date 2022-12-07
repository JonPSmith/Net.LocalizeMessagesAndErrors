// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This class is used by the StubLocalizeDefaultWithLogging found in
/// https://github.com/JonPSmith/Net.LocalizeMessagesAndErrors/blob/main/Test/StubClasses
/// which provides a way to capture information on each localized message.
/// NOTE: The StubLocalizeDefaultWithLogging isn't in the library because it uses EF Core,
/// which is not needed in this library. 
/// </summary>
public class LocalizedLog
{
    private LocalizedLog() {} //Needed by EF Core

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="resourceClass"></param>
    /// <param name="localizeKey"></param>
    /// <param name="cultureOfMessage"></param>
    /// <param name="actualMessage"></param>
    /// <param name="messageFormat"></param>
    /// <param name="callingClassName"></param>
    /// <param name="callingMethodName"></param>
    /// <param name="sourceLineNumber"></param>
    public LocalizedLog(Type resourceClass, string localizeKey, string cultureOfMessage,
        string actualMessage, string? messageFormat, string callingClassName, string callingMethodName, int sourceLineNumber)
    {
        ResourceClassFullName = resourceClass.FullName!;
        LocalizeKey = localizeKey;
        CultureOfMessage = cultureOfMessage;
        ActualMessage = actualMessage;
        MessageFormat = messageFormat;
        CallingClassName = callingClassName;
        CallingMethodName = callingMethodName;
        SourceLineNumber = sourceLineNumber;
    }

    public int Id { get; set; }
    /// <summary>
    /// The FullName of the resource class you used in localization code
    /// </summary>
    public string ResourceClassFullName { get; set; }
    /// <summary>
    /// This is the string that is used to match the resource file "Name" of the localized message
    /// </summary>
    public string LocalizeKey { get; set; }
    /// <summary>
    /// This contains the culture of the default message
    /// </summary>
    public string CultureOfMessage { get; set; }
    /// <summary>
    /// This contains the actual message 
    /// </summary>
    public string ActualMessage { get; set; }
    /// <summary>
    /// If the <see cref="ActualMessage"/> has dynamic parts, then this contains the Format, otherwise null
    /// </summary>
    public string MessageFormat { get; set; }
    /// <summary>
    /// This is true if another entry with the same <see cref="LocalizeKey"/>, but a different message / format
    /// This can be null if the the database wasn't used - null means "I don't know"
    /// </summary>
    public bool? SameKeyButDiffFormat { get; set; }
    /// <summary>
    /// The name of the class where the LocalizeKey was created
    /// </summary>
    public string CallingClassName { get; set; }
    /// <summary>
    /// This contains the name of the method / member that created the LocalizeKey
    /// </summary>
    public string CallingMethodName { get; set; }
    /// <summary>
    /// This has the source line of where the LocalizeKey was created
    /// </summary>
    public int SourceLineNumber { get; set; }
}