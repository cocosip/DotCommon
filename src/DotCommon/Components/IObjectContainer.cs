using System;
#if !NET45
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DotCommon.Components
{
    /// <summary>Represents an object container interface.
    /// </summary>
    public interface IObjectContainer
    {
        TService Resolve<TService>() where TService : class;
        object Resolve(Type serviceType);

#if NET45

        void RegisterType(Type implementationType, string serviceName = null, LifeStyle life = LifeStyle.Singleton);

        void RegisterType(Type serviceType, Type implementationType, string serviceName = null,
            LifeStyle life = LifeStyle.Singleton);

        void Register<TService, TImplementer>(string serviceName = null, LifeStyle life = LifeStyle.Singleton)
            where TService : class
            where TImplementer : class, TService;

        void RegisterInstance<TService, TImplementer>(TImplementer instance, string serviceName = null)
            where TService : class
            where TImplementer : class, TService;

        bool TryResolve<TService>(out TService instance) where TService : class;

        bool TryResolve(Type serviceType, out object instance);

        TService ResolveNamed<TService>(string serviceName) where TService : class;

        object ResolveNamed(string serviceName, Type serviceType);

        bool TryResolveNamed(string serviceName, Type serviceType, out object instance);

        IObjectContainer Apply();
#else
        IServiceProvider Provider { get; }
        IServiceCollection CurrentServices { get; }
        IServiceProvider ApplyServices(IServiceCollection services);

        void RegisterType<TService, TImplementer>(LifeStyle life = LifeStyle.Singleton)
            where TService : class
            where TImplementer : class, TService;

        void RegisterType(Type serviceType, Type implementationType, LifeStyle life = LifeStyle.Singleton);

        void RegisterTypeWithOptions<TOptions>(Type serviceType, Type implementationType,
            Action<TOptions> configureOptions,
            LifeStyle life = LifeStyle.Singleton) where TOptions : class;
#endif
    }
}
