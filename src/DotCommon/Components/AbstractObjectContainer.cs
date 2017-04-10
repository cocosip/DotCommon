#if !NET45
using System;
using Microsoft.Extensions.DependencyInjection;
namespace DotCommon.Components
{
    public abstract class AbstractObjectContainer : IObjectContainer
    {
        public IServiceProvider Provider { get; protected set; }
        public IServiceCollection CurrentServices { get; protected set; } = new ServiceCollection();

        public void RegisterType<TService, TImplementer>(LifeStyle life = LifeStyle.Singleton) where TService : class
            where TImplementer : class, TService
        {
            RegisterType(typeof(TService), typeof(TImplementer), life);
        }

        public void RegisterType(Type serviceType, Type implementationType, LifeStyle life = LifeStyle.Singleton)
        {
            CurrentServices.Add(new ServiceDescriptor(serviceType, implementationType, GetLifetime(life)));
        }

        public void RegisterTypeWithOptions<TOptions>(Type serviceType, Type implementationType,
            Action<TOptions> configureOptions,
            LifeStyle life = LifeStyle.Singleton) where TOptions : class
        {
            CurrentServices.AddOptions();
            CurrentServices.Configure(configureOptions);
            RegisterType(serviceType, implementationType, life);
        }

        public TService Resolve<TService>() where TService : class
        {
            return Provider.GetRequiredService<TService>();
        }

        public object Resolve(Type serviceType)
        {
            return Provider.GetRequiredService(serviceType);
        }

        protected ServiceLifetime GetLifetime(LifeStyle life)
        {
            switch (life)
            {
                case LifeStyle.Singleton:
                    return ServiceLifetime.Singleton;
                case LifeStyle.Transient:
                    return ServiceLifetime.Transient;
                default:
                    //   case LifeStyle.Scoped:
                    return ServiceLifetime.Scoped;
            }
        }

        public abstract IServiceProvider ApplyServices(IServiceCollection services);
    }
}
#endif