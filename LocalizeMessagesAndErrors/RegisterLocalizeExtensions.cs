// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Security.AccessControl;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// Extension methods to register the <see cref="DefaultLocalizer{TResource}"/> and the <see cref="SimpleLocalizer"/>
/// </summary>
public static class RegisterLocalizeExtensions
{
    /// <summary>
    /// This registers the <see cref="DefaultLocalizer{TResource}"/> service. This service can handle multiple
    /// resource files.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options">Use this to set up the options of the localizer. You MUST set the DefaultCulture property </param>
    /// <returns></returns>
    public static IServiceCollection RegisterLocalizeDefault(this IServiceCollection services,
        Action<DefaultLocalizerOptions> options)
    {
        var localOptions = new DefaultLocalizerOptions();
        options?.Invoke(localOptions);
        if (string.IsNullOrWhiteSpace(localOptions.DefaultCulture))
            throw new ArgumentException("The DefaultCulture must be set to the culture of the default messages.", nameof(options));

        services.AddSingleton(localOptions);
        services.AddSingleton(typeof(IDefaultLocalizer<>), typeof(DefaultLocalizer<>));
        return services;
    }

    /// <summary>
    /// This registers the <see cref="ISimpleLocalizer"/> service, which uses a single resource file
    /// which is defined by the TResource type.
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    /// <param name="services"></param>
    /// <param name="options">Use this to set up the options of the localizer. You MUST set the DefaultCulture property </param>
    /// <returns></returns>
    public static IServiceCollection RegisterSimpleLocalizer<TResource>(this IServiceCollection services,
        Action<SimpleLocalizerOptions> options)
    {
        var localOptions = new SimpleLocalizerOptions
        {
            ResourceType = typeof(TResource),
        };
        options?.Invoke(localOptions);
        if (localOptions.ResourceType == null) throw new ArgumentNullException(nameof(options));
        if (string.IsNullOrWhiteSpace(localOptions.DefaultCulture))
            throw new ArgumentException("The DefaultCulture must be set to the culture of the default messages.", nameof(options));

        services.AddSingleton<ISimpleLocalizer>(options => new SimpleLocalizer(options, localOptions));
        return services;
    }
}