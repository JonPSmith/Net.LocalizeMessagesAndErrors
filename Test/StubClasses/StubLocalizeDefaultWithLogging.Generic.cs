// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using LocalizeMessagesAndErrors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestSupport.Helpers;

namespace Test.StubClasses;

/// <summary>
/// This provides a simple replacement of the <see cref="DefaultLocalizer{TResource}"/> which
/// returns the the default message.
/// It also writes the information on each localized message to a database is the appsettings.json
/// file in your testing project contains "SaveLocalizesToDb": true.
/// If "SaveLocalizesToDb" is True, then there needs to be a connection string called "LocalizationCaptureDb"
/// which links to a SQL Server database server where the localized message information is saved to.
/// </summary>
/// <typeparam name="TResource"></typeparam>
public class StubDefaultLocalizerWithLogging<TResource> : StubDefaultLocalizerWithLogging, IDefaultLocalizer<TResource>
{
    public StubDefaultLocalizerWithLogging(string cultureOfMessage)
    : base(cultureOfMessage, typeof(TResource))
    {}
}