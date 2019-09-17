using DotCommon.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon
{
    /// <summary>应用信息
    /// </summary>
    public class DotCommonApplication : IDotCommonApplication
    {
        /// <summary>ServiceScope
        /// </summary>
        public IServiceScope ServiceScope { get; private set; }

        /// <summary>ServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>Services
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>Ctor
        /// </summary>
        public DotCommonApplication(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>设置ServiceProvider
        /// </summary>
        protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
        }

        /// <summary>初始化
        /// </summary>
        public void Initialize(IServiceProvider serviceProvider)
        {
            ServiceScope = serviceProvider.CreateScope();
            SetServiceProvider(ServiceScope.ServiceProvider);
        }

        /// <summary>释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>释放
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            ServiceScope.Dispose();
        }
    }
}
