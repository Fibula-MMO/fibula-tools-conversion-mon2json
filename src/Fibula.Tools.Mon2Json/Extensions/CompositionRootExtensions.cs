// -----------------------------------------------------------------
// <copyright file="CompositionRootExtensions.cs" company="The Fibula Project">
// Copyright (c) | The Fibula Project.
// https://github.com/orgs/fibula-mmo/people
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Tools.Mon2Json
{
    using Fibula.Data.Contracts.Abstractions;
    using Fibula.Utilities.Validation;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Static class that adds convenient methods to add the concrete implementations contained in this library.
    /// </summary>
    public static class CompositionRootExtensions
    {
        /// <summary>
        /// Adds the monster files converter implementation contained in this library to the services collection.
        /// </summary>
        /// <param name="services">The services collection.</param>
        public static void AddInteractiveMonsterFilesConverter(this IServiceCollection services)
        {
            services.ThrowIfNull(nameof(services));

            services.AddSingleton((serviceProvider) =>
            {
                return new MonsterFilesConverter(Options.Create(new MonsterFilesConverterOptions()), serviceProvider.GetService<IItemTypesLoader>());
            });
        }

        /// <summary>
        /// Adds the monster files converter implementation contained in this library to the services collection.
        /// Additionally, registers the options related to the concrete implementations added, such as:
        ///     <see cref="MonsterFilesConverterOptions"/>.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <param name="configuration">The configuration reference.</param>
        public static void AddConfigurationMonsterFilesConverter(this IServiceCollection services, IConfiguration configuration)
        {
            services.ThrowIfNull(nameof(services));
            configuration.ThrowIfNull(nameof(configuration));

            // configure options
            services.Configure<MonsterFilesConverterOptions>(configuration.GetSection(nameof(MonsterFilesConverterOptions)));

            services.AddSingleton<MonsterFilesConverter>();
        }
    }
}
