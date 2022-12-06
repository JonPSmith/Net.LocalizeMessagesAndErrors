// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;

namespace Test.StubClasses;

public class LocalizedLog
{
    private LocalizedLog() {} //Needed by EF Core

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
    public string ResourceClassFullName { get; set; }
    public string LocalizeKey { get; set; }
    public string CultureOfMessage { get; set; }
    public string ActualMessage { get; set; }
    public string? MessageFormat { get; set; }
    public bool? SameKeyButDiffFormat { get; set; } //null means not set
    public string CallingClassName { get; set; }
    public string CallingMethodName { get; set; }
    public int SourceLineNumber { get; set; }
}