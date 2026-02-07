using System;
using DotCommon;
using DotCommon.DistributedLocking;
using Medallion.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for registering distributed locking services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds in-process distributed locking (for single instance applications).
        /// Uses <see cref="LocalDistributedLock"/> internally.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configureOptions">Optional action to configure <see cref="DistributedLockOptions"/></param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddDotCommonDistributedLocking(
            this IServiceCollection services,
            Action<DistributedLockOptions>? configureOptions = null)
        {
            // Configure options
            services.Configure<DistributedLockOptions>(options =>
            {
                options.KeyPrefix = "DistributedLock:";
                configureOptions?.Invoke(options);
            });

            // Register services
            services.AddSingleton<IDistributedLockKeyNormalizer, DistributedLockKeyNormalizer>();
            services.AddSingleton<DotCommon.DistributedLocking.IDistributedLock, LocalDistributedLock>();

            return services;
        }

        /// <summary>
        /// Adds distributed locking based on Medallion.Threading.
        /// Requires <see cref="IDistributedLockProvider"/> to be provided.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="lockProvider">The Medallion lock provider (e.g., Redis, SQL Server)</param>
        /// <param name="configureOptions">Optional action to configure <see cref="DistributedLockOptions"/></param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddDotCommonDistributedLocking(
            this IServiceCollection services,
            IDistributedLockProvider lockProvider,
            Action<DistributedLockOptions>? configureOptions = null)
        {
            Check.NotNull(lockProvider, nameof(lockProvider));

            // Configure options
            services.Configure<DistributedLockOptions>(options =>
            {
                options.KeyPrefix = "DistributedLock:";
                configureOptions?.Invoke(options);
            });

            // Register services
            services.AddSingleton<IDistributedLockKeyNormalizer, DistributedLockKeyNormalizer>();
            services.AddSingleton(lockProvider);
            services.AddSingleton<DotCommon.DistributedLocking.IDistributedLock, MedallionDistributedLock>();

            return services;
        }

        /// <summary>
        /// Adds distributed locking with a factory function for <see cref="IDistributedLockProvider"/>.
        /// The factory receives <see cref="IServiceProvider"/> and should return a lock provider instance.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="lockProviderFactory">Factory function to create the lock provider</param>
        /// <param name="configureOptions">Optional action to configure <see cref="DistributedLockOptions"/></param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddDotCommonDistributedLocking(
            this IServiceCollection services,
            Func<IServiceProvider, IDistributedLockProvider> lockProviderFactory,
            Action<DistributedLockOptions>? configureOptions = null)
        {
            Check.NotNull(lockProviderFactory, nameof(lockProviderFactory));

            // Configure options
            services.Configure<DistributedLockOptions>(options =>
            {
                options.KeyPrefix = "DistributedLock:";
                configureOptions?.Invoke(options);
            });

            // Register services
            services.AddSingleton<IDistributedLockKeyNormalizer, DistributedLockKeyNormalizer>();
            services.AddSingleton(lockProviderFactory);
            services.AddSingleton<DotCommon.DistributedLocking.IDistributedLock, MedallionDistributedLock>();

            return services;
        }
    }
}
