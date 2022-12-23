// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.


using System.Reflection;
using LocalizeMessagesAndErrors.UnitTestingCode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This allows you to create a <see cref="IDefaultLocalizer"/> when your resource class is
/// no known on startup.
/// </summary>
public class DefaultLocalizerFactory : IDefaultLocalizerFactory
{


    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="serviceProvider"></param>
    public DefaultLocalizerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDefaultLocalizer Create(Type resourceSource)
    {

        if (resourceSource == null)
            //If the resourceSource is null (which means DefaultLocalizer isn't set up), then return a stub version
            return new StubDefaultLocalizer();

        var localizeFactory = _serviceProvider.GetService<IStringLocalizerFactory>();

        if (localizeFactory == null)
            //If the localizeFactory is null (which means that StringLocalizer isn't configured), then return a stub version
            return new StubDefaultLocalizer();

        var options = _serviceProvider.GetService<DefaultLocalizerOptions>();
        if (options == null)
            throw new NullReferenceException(
                $"This failed because you haven't registered the {nameof(IDefaultLocalizer)} service.");

        var stringLocalizer = localizeFactory.Create(resourceSource);
        var defaultLocalizerType = typeof(DefaultLocalizer<>).MakeGenericType(resourceSource);
        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(defaultLocalizerType);
        var defaultLocalizer = Activator.CreateInstance(defaultLocalizerType, 
            BindingFlags.Instance | BindingFlags.NonPublic, null,
            new object[]{ options, stringLocalizer, logger }, null);

        return defaultLocalizer as IDefaultLocalizer;
    }

}