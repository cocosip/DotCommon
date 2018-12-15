using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon
{
    public interface IDotCommonApplication : IDisposable
    {

        /// <summary>
        /// List of services registered to this application.
        /// Can not add new services to this collection after application initialize.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Reference to the root service provider used by the application.
        /// This can not be used before initialize the application.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>Initialize
        /// </summary>
        void Initialize(IServiceProvider serviceProvider);
    }
}
