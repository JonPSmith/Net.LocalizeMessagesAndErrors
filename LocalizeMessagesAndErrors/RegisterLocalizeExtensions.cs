// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace LocalizeMessagesAndErrors;

/// <summary>
/// Extension methods to register the <see cref="DefaultLocalizerLocalizerLocalizer{TResource}"/> and the <see cref="SimpleLocalizer"/>
/// </summary>
public static class RegisterLocalizeExtensions
{
    /// <summary>
    /// This registers the <see cref="DefaultLocalizerLocalizerLocalizer{TResource}"/> service. This service can handle multiple
    /// resource files
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterLocalizeDefault(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IDefaultLocalizer<>), typeof(DefaultLocalizer<>));
        return services;
    }

    /// <summary>
    /// This registers both the <see cref="DefaultLocalizerLocalizerLocalizer{TResource}"/> service. This service can handle multiple
    /// resource files, and the <see cref="ISimpleLocalizer"/> service, which uses a single resource file
    /// which is defined by the TResource type.
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    /// <param name="services"></param>
    /// <param name="defaultCultureForSimpleLocalizer"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterSimpleLocalizerAndLocalizeDefault<TResource>(this IServiceCollection services,
        string defaultCultureForSimpleLocalizer)
    {
        services.AddSingleton(typeof(IDefaultLocalizer<>), typeof(DefaultLocalizer<>));
        services.AddSingleton<ISimpleLocalizer>(options =>
            new SimpleLocalizer(options, typeof(TResource), defaultCultureForSimpleLocalizer));
        return services;
    }
}