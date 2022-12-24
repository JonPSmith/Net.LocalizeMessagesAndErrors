// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// This allows you to create a <see cref="ISimpleLocalizer"/> using a resource type
/// provided by you at runtime.
/// </summary>
public class SimpleLocalizerFactory : ISimpleLocalizerFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="serviceProvider"></param>
    public SimpleLocalizerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// This with create a <see cref="ISimpleLocalizer"/> linked to the TResource type 
    /// </summary>
    /// <param name="resourceSource">type of the resource class defining the localization resource files</param>
    /// <returns><see cref="IDefaultLocalizer"/></returns>
    /// <exception cref="NullReferenceException"></exception>
    public ISimpleLocalizer Create(Type resourceSource)
    {
        var options = new SimpleLocalizerOptions { ResourceType = resourceSource };
        if (resourceSource == null)
            //If the resourceSource is null (which means DefaultLocalizer isn't set up), then return a stub version
            return new SimpleLocalizer(new DefaultLocalizerFactory(_serviceProvider),
                new SimpleLocalizerOptions { ResourceType = null });

        var localizeFactory = _serviceProvider.GetService<IStringLocalizerFactory>();

        if (localizeFactory == null)
            //If the localizeFactory is null (which means that StringLocalizer isn't configured), then return a stub version
            return new SimpleLocalizer(new DefaultLocalizerFactory(_serviceProvider),
                new SimpleLocalizerOptions { ResourceType = null });

        return new SimpleLocalizer(new DefaultLocalizerFactory(_serviceProvider), options);
    }

}