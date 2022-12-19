// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using Test.StubClasses;
using TestSupport.Attributes;
using Xunit.Abstractions;

namespace Test.UnitCommands;

public class LocalizationCaptureCommands
{
    private readonly ITestOutputHelper _output;

    public LocalizationCaptureCommands(ITestOutputHelper output)
    {
        _output = output;
    }

    [RunnableInDebugOnly]
    public void WipeLocalizationCaptureDbAndSetToCapture()
    {
        var stub = new StubDefaultLocalizerWithLogging<LocalizationCaptureCommands>(String.Empty);
        stub.WipeLocalizationCaptureDb();
    }

    [RunnableInDebugOnly]
    public void DisplayCapturedLocalizations()
    {
        var stub = new StubDefaultLocalizerWithLogging<LocalizationCaptureCommands>(String.Empty);

        var entries = stub.ListLocalizationCaptureDb();

        _output.WriteLine($"There are {entries.Count} captured localizations, " +
                          $"with {entries.Count(x => x.PossibleErrors != null)} so problems.");
        foreach (var entry in entries)
        {
            _output.WriteLine($"ResourceClassFullName = {entry.ResourceClassFullName}");
            _output.WriteLine($"     LocalizeKey = {entry.LocalizeKey}, {entry.PossibleErrors ?? ""}");
            _output.WriteLine($"     Actual Message = {entry.ActualMessage}");
            if (entry.MessageFormat != null )
                _output.WriteLine($"     Message Format = {entry.MessageFormat}");
        }
        _output.WriteLine("END ------------------------------------------------------");
    }
}