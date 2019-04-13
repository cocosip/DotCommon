using DotCommon.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon
{
    public class DotCommonApplication : IDotCommonApplication
    {
        public IServiceScope ServiceScope { get; private set; }

        public IServiceProvider ServiceProvider { get; private set; }

        public IServiceCollection Services { get; }

        public DotCommonApplication(IServiceCollection services)
        {
            Services = services;
        }

        public virtual void Dispose()
        {
            ServiceScope.Dispose();
        }

        protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            ServiceScope = serviceProvider.CreateScope();
            SetServiceProvider(ServiceScope.ServiceProvider);
        }
    }
}
