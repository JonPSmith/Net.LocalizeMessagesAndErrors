﻿// Copyright (c) 2022 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

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
    /// <param name="defaultCulture">This defines the language of the messages you provide.</param>
    /// <returns></returns>
    public static IServiceCollection RegisterDefaultLocalizer(this IServiceCollection services, string defaultCulture)
    {
        if (string.IsNullOrWhiteSpace(defaultCulture))
            throw new ArgumentException("The DefaultCulture must be set to the culture of the default messages.", nameof(defaultCulture));

        services.AddSingleton(new DefaultLocalizerOptions { DefaultCulture = defaultCulture });
        services.AddSingleton(typeof(IDefaultLocalizer<>), typeof(DefaultLocalizer<>));
        return services;
    }

    /// <summary>
    /// This registers the <see cref="ISimpleLocalizer"/> service.
    /// NOTE: You must have registered the <see cref="DefaultLocalizer{T}"/> service via the
    /// <see cref="RegisterDefaultLocalizer"/> extension method to make the <see cref="ISimpleLocalizer"/>
    /// service work.
    /// </summary>
    /// <typeparam name="TResource">This defines the start of the resource files that this service
    /// will use to look up localized languages.</typeparam>
    /// <param name="services"></param>
    /// <param name="options">Use this to alter options in the <see cref="SimpleLocalizerOptions"/> class.</param>
    /// <returns></returns>
    public static IServiceCollection RegisterSimpleLocalizer<TResource>(this IServiceCollection services,
        Action<SimpleLocalizerOptions> options = null)
    {
        var localOptions = new SimpleLocalizerOptions
        {
            ResourceType = typeof(TResource),
        };
        options?.Invoke(localOptions);
        if (localOptions.ResourceType == null) throw new ArgumentNullException(nameof(SimpleLocalizerOptions.ResourceType));

        services.AddSingleton<ISimpleLocalizer>(services => new SimpleLocalizer(services, localOptions));
        return services;
    }
}